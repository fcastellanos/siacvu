using System;
using System.Linq;
using System.Web.Mvc;
using DecisionesInteligentes.Colef.Sia.ApplicationServices;
using DecisionesInteligentes.Colef.Sia.Core;
using DecisionesInteligentes.Colef.Sia.Web.Controllers.Collections;
using DecisionesInteligentes.Colef.Sia.Web.Controllers.Helpers;
using DecisionesInteligentes.Colef.Sia.Web.Controllers.Mappers;
using DecisionesInteligentes.Colef.Sia.Web.Controllers.Models;
using DecisionesInteligentes.Colef.Sia.Web.Controllers.Security;
using DecisionesInteligentes.Colef.Sia.Web.Controllers.ViewData;

namespace DecisionesInteligentes.Colef.Sia.Web.Controllers.Productos
{
    public class ArticuloDifusionController : BaseController<ArticuloDifusion, ArticuloDifusionForm>
    {
        readonly IAreaMapper areaMapper;
        readonly IArticuloDifusionMapper articuloMapper;
        readonly IArticuloDifusionService articuloService;
        readonly ICoautorExternoArticuloMapper coautorExternoArticuloMapper;
        readonly ICoautorInternoArticuloMapper coautorInternoArticuloMapper;
        readonly ICustomCollection customCollection;
        readonly IInvestigadorService investigadorService;
        readonly ILineaTematicaMapper lineaTematicaMapper;
        readonly IProductoService productoService;
        readonly IRevistaPublicacionMapper revistaPublicacionMapper;
        readonly ITipoArchivoMapper tipoArchivoMapper;

        public ArticuloDifusionController(IArticuloDifusionService articuloService,
                                          IArticuloDifusionMapper articuloMapper,
                                          ICatalogoService catalogoService,
                                          IUsuarioService usuarioService,
                                          ICoautorExternoArticuloMapper coautorExternoArticuloMapper,
                                          ICoautorInternoArticuloMapper coautorInternoArticuloMapper,
                                          ISearchService searchService, ITipoArchivoMapper tipoArchivoMapper,
                                          IAreaTematicaMapper areaTematicaMapper, ICustomCollection customCollection,
                                          ILineaTematicaMapper lineaTematicaMapper, IAreaMapper areaMapper,
                                          IDisciplinaMapper disciplinaMapper, ISubdisciplinaMapper subdisciplinaMapper,
                                          IRevistaPublicacionMapper revistaPublicacionMapper,
                                          IInvestigadorExternoMapper investigadorExternoMapper,
                                          IArchivoService archivoService,
                                          IInvestigadorService investigadorService,
                                          IProductoService productoService,
                                          IPaisMapper paisMapper
            ) : base(usuarioService, searchService, catalogoService, disciplinaMapper, subdisciplinaMapper)
        {
            base.paisMapper = paisMapper;

            this.coautorInternoArticuloMapper = coautorInternoArticuloMapper;
            this.articuloService = articuloService;
            this.articuloMapper = articuloMapper;
            this.coautorExternoArticuloMapper = coautorExternoArticuloMapper;
            this.tipoArchivoMapper = tipoArchivoMapper;
            this.areaTematicaMapper = areaTematicaMapper;
            this.customCollection = customCollection;
            this.lineaTematicaMapper = lineaTematicaMapper;
            this.areaMapper = areaMapper;
            this.revistaPublicacionMapper = revistaPublicacionMapper;
            this.investigadorExternoMapper = investigadorExternoMapper;
            this.investigadorService = investigadorService;
            this.productoService = productoService;
            this.paisMapper = paisMapper;
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            var data = new GenericViewData<ArticuloDifusionForm>();
            var productos = productoService.GetProductosByUsuario<ArticuloDifusion>(CurrentUser(), x => x.Titulo,
                                                                                    x => x.TipoArticulo);
            data.ProductList = productos;

            return View(data);
        }

        [Authorize(Roles = "Investigadores")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult New()
        {
            if (CurrentInvestigador() == null)
                return NoInvestigadorProfile("Por tal motivo no puede crear nuevos productos.");

            var data = new GenericViewData<ArticuloDifusionForm> {Form = SetupNewForm()};
            ViewData["Pais"] = (from p in data.Form.Paises where p.Nombre == "M�xico" select p.Id).FirstOrDefault();

            return View(data);
        }

        [Authorize(Roles = "Investigadores, DGAA")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            var data = new GenericViewData<ArticuloDifusionForm>();
            var articulo = articuloService.GetArticuloById(id);

            var verifyMessage = VerifyProductoStatus(articulo.Firma, articulo.Titulo);
            if (!String.IsNullOrEmpty(verifyMessage))
                return RedirectToHomeIndex(verifyMessage);

            var verifyOwnershipMessage = VerifyProductoOwnership(CurrentUser().Investigador, articulo.Usuario.Id,
                                                                 CurrentUser().Id);
            if (!String.IsNullOrEmpty(verifyOwnershipMessage))
                return RedirectToHomeIndex(verifyOwnershipMessage);

            CoautorInternoArticulo coautorInternoArticulo;
            var posicionAutor = 0;
            var coautorExists = 0;

            if (User.IsInRole("Investigadores"))
            {
                coautorExists =
                    articulo.CoautorInternoArticulos.Where(
                        x => x.Investigador.Id == CurrentInvestigador().Id).Count();

                if (articulo.Usuario.Id != CurrentUser().Id && coautorExists == 0)
                    return RedirectToHomeIndex("no lo puede modificar");
            }

            var articuloForm = articuloMapper.Map(articulo);
            if (articulo.AreaTematica != null)
                articuloForm.LineaTematicaId = articulo.AreaTematica.LineaTematica.Id;

            data.Form = SetupNewForm(articuloForm);
            FormSetCombos(data.Form);

            if (coautorExists != 0)
            {
                coautorInternoArticulo =
                    articulo.CoautorInternoArticulos.Where(x => x.Investigador.Id == CurrentInvestigador().Id).
                        FirstOrDefault();

                if (coautorInternoArticulo != null) posicionAutor = coautorInternoArticulo.Posicion;
            }
            else
                posicionAutor = data.Form.PosicionCoautor;

            data.Form.PosicionCoautor = posicionAutor;

            ViewData.Model = data;
            return View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Show(int id)
        {
            var data = new GenericViewData<ArticuloDifusionForm>();

            var articulo = articuloService.GetArticuloById(id);

            var articuloForm = articuloMapper.Map(articulo);

            data.Form = SetupShowForm(articuloForm);

            ViewData.Model = data;
            return View();
        }

        [CookieLessAuthorize]
        [CustomTransaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddFile(FormCollection form)
        {
            var id = Convert.ToInt32(form["Id"]);
            var articulo = articuloService.GetArticuloById(id);

            var archivo = MapArchivo<ArchivoArticuloDifusion>();
            articulo.AddArchivo(archivo);

            articuloService.SaveArticulo(articulo);

            return Content("Uploaded");
        }

        [CustomTransaction]
        [Authorize(Roles = "DGAA")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DgaaValidateProduct(FirmaForm firmaForm)
        {
            var articulo = articuloService.GetArticuloById(firmaForm.ProductoId);

            articulo.Firma.Aceptacion2 = 1;
            articulo.Firma.Usuario2 = CurrentUser();

            articuloService.SaveArticulo(articulo);

            var data = new FirmaForm
                           {
                               TipoProducto = firmaForm.TipoProducto,
                               Aceptacion2 = 1
                           };

            return Rjs("DgaaSign", data);
        }

        [Authorize(Roles = "DGAA")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DgaaRejectProduct(FirmaForm firmaForm)
        {
            var articulo = articuloService.GetArticuloById(firmaForm.ProductoId);
            articulo.Firma.Aceptacion1 = 0;
            articulo.Firma.Aceptacion2 = 2;
            articulo.Firma.Descripcion = firmaForm.Descripcion;
            articulo.Firma.Usuario1 = CurrentUser();
            articulo.Firma.Usuario2 = CurrentUser();

            ModelState.AddModelErrors(articulo.ValidationResults(), false, "Firma");
            if (!ModelState.IsValid)
            {
                return Rjs("ModelError");
            }

            articuloService.SaveArticulo(articulo, true);

            var data = new FirmaForm
                           {
                               TipoProducto = firmaForm.TipoProducto,
                               Aceptacion2 = 2
                           };

            return Rjs("DgaaSign", data);
        }

        [CustomTransaction]
        [Authorize(Roles = "Investigadores")]
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([Bind(Prefix = "CoautorInterno")] CoautorInternoProductoForm[] coautorInterno,
                                   [Bind(Prefix = "CoautorExterno")] CoautorExternoProductoForm[] coautorExterno,
                                   ArticuloDifusionForm form)
        {
            coautorExterno = coautorExterno ?? new CoautorExternoProductoForm[] {};
            coautorInterno = coautorInterno ?? new CoautorInternoProductoForm[] {};

            var articulo = articuloMapper.Map(form, CurrentUser(), CurrentInvestigador(),
                                              coautorExterno, coautorInterno);

            ModelState.AddModelErrors(articulo.ValidationResults(), true, "ArticuloDifusion");

            if (!ModelState.IsValid)
            {
                return Rjs("ModelError");
            }

            articuloService.SaveArticulo(articulo);
            SetMessage(String.Format("Art�culo en revistas de difusi�n {0} ha sido registrado", articulo.Titulo));

            return Rjs("Save", articulo.Id);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(ArticuloDifusionForm form)
        {
            var articulo = new ArticuloDifusion();

            if (User.IsInRole("Investigadores"))
                articulo = articuloMapper.Map(form, CurrentUser(), CurrentInvestigador());
            if (User.IsInRole("DGAA"))
                articulo = articuloMapper.Map(form, CurrentUser());

            ModelState.AddModelErrors(articulo.ValidationResults(), true, "ArticuloDifusion");

            if (!ModelState.IsValid)
            {
                return Rjs("ModelError");
            }

            articuloService.SaveArticulo(articulo, true);
            SetMessage(String.Format("Art�culo en revistas de difusi�n {0} ha sido modificado", articulo.Titulo));

            return Rjs("Save", articulo.Id);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public override ActionResult Search(string q)
        {
            var data = searchService.Search<ArticuloDifusion>(x => x.Titulo, q);
            return Content(data);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ChangeRevista(int select)
        {
            var revistaForm = revistaPublicacionMapper.Map(catalogoService.GetRevistaPublicacionById(select));

            var form = new ShowFieldsForm
                           {
                               RevistaPublicacionId = revistaForm.Id,
                               RevistaPublicacionInstitucionNombre = revistaForm.InstitucionNombre,
                               RevistaPublicacionIndice1Nombre = revistaForm.Indice1Nombre,
                               RevistaPublicacionIndice2Nombre = revistaForm.Indice2Nombre,
                               RevistaPublicacionIndice3Nombre = revistaForm.Indice3Nombre
                           };

            return Rjs("ChangeRevista", form);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ChangeAreaTematica(int select)
        {
            // TODO: Dependencias
            return Rjs("", null);
            //var areaTematicaForm = areaTematicaMapper.Map(catalogoService.GetAreaTematicaById(select));
            //var lineaTematicaForm =
            //    lineaTematicaMapper.Map(catalogoService.GetLineaTematicaById(areaTematicaForm.LineaTematicaId));

            //var form = new ShowFieldsForm
            //               {
            //                   AreaTematicaLineaTematicaNombre = lineaTematicaForm.Nombre,
            //                   AreaTematicaId = areaTematicaForm.Id
            //               };

            //return Rjs("ChangeAreaTematica", form);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult NewCoautorInterno(int id, bool esAlfabeticamente)
        {
            var articulo = articuloService.GetArticuloById(id);
            var form = new CoautorForm
                           {
                               Controller = "ArticuloDifusion",
                               IdName = "ArticuloId",
                               CoautorSeOrdenaAlfabeticamente = esAlfabeticamente
                           };

            if (User.IsInRole("Investigadores"))
                form.CreadoPorId = CurrentInvestigador().Id;

            if (articulo != null)
            {
                form.Id = articulo.Id;
                var investigador = investigadorService.GetInvestigadorByUsuario(articulo.CreadoPor.UsuarioNombre);
                form.CreadoPorId = investigador.Id;
            }

            return Rjs("NewCoautorInterno", form);
        }

        [CustomTransaction]
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddCoautorInterno([Bind(Prefix = "CoautorInterno")] CoautorInternoProductoForm form,
                                              int articuloId)
        {
            var coautorInternoArticulo = coautorInternoArticuloMapper.Map(form);

            ModelState.AddModelErrors(coautorInternoArticulo.ValidationResults(), false, "CoautorInterno", String.Empty);
            if (!ModelState.IsValid)
            {
                return Rjs("ModelError");
            }

            if (articuloId != 0)
            {
                coautorInternoArticulo.CreadoPor = CurrentUser();
                coautorInternoArticulo.ModificadoPor = CurrentUser();

                var articulo = articuloService.GetArticuloById(articuloId);
                var alreadyHasIt =
                    articulo.CoautorInternoArticulos.Where(
                        x => x.Investigador.Id == coautorInternoArticulo.Investigador.Id).Count();

                if (alreadyHasIt == 0)
                {
                    articulo.AddCoautorInterno(coautorInternoArticulo);
                    articuloService.SaveArticulo(articulo);
                }
            }

            var coautorInternoArticuloForm = coautorInternoArticuloMapper.Map(coautorInternoArticulo);
            coautorInternoArticuloForm.ParentId = articuloId;

            return Rjs("AddCoautorInterno", coautorInternoArticuloForm);
        }

        [CustomTransaction]
        [Authorize]
        [AcceptVerbs(HttpVerbs.Delete)]
        public ActionResult DeleteCoautorInterno(int id, int investigadorId)
        {
            var articulo = articuloService.GetArticuloById(id);

            if (articulo != null)
            {
                var coautor = articulo.CoautorInternoArticulos.Where(x => x.Investigador.Id == investigadorId).First();
                articulo.DeleteCoautorInterno(coautor);

                articuloService.SaveArticulo(articulo);
            }

            var form = new CoautorForm {ModelId = id, InvestigadorId = investigadorId};

            return Rjs("DeleteCoautorInterno", form);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Deactivate(int id)
        {
            var articulo = articuloService.GetArticuloById(id);

            articulo.Activo = false;
            articulo.ModificadoPor = CurrentUser();

            articuloService.SaveArticulo(articulo, true);

            var articuloForm = articuloMapper.Map(articulo);

            return Rjs("Deactivate", articuloForm);
        }

        protected override void DeleteCoautorExternoInModel(ArticuloDifusion model, int coautorExternoId)
        {
            if (model == null) return;
            var coautor =
                model.CoautorExternoArticulos.Where(x => x.InvestigadorExterno.Id == coautorExternoId).First();
            model.DeleteCoautorExterno(coautor);

            articuloService.SaveArticulo(model, true);
        }

        protected override bool SaveCoautorExternoToModel(ArticuloDifusion model, CoautorExternoProducto coautorExternoProducto)
        {
            ModelState.AddModelErrors(model.ValidationResults(), true, "ArticuloDifusion");
            if (!ModelState.IsValid)
            {
                return false;
            }

            var alreadyHasIt =
                model.CoautorExternoArticulos.Where(
                    x => x.InvestigadorExterno.Id == coautorExternoProducto.InvestigadorExterno.Id).Count();

            if (alreadyHasIt == 0)
            {
                model.AddCoautorExterno(coautorExternoProducto);
                articuloService.SaveArticulo(model);
            }

            return alreadyHasIt == 0;
        }

        protected override CoautorExternoProducto MapCoautorExternoProductoMessage(CoautorExternoProductoForm form)
        {
            return coautorExternoArticuloMapper.Map(form);
        }

        protected override CoautorExternoProductoForm MapCoautorExternoProductoModel(CoautorExternoProducto model, int parentId)
        {
            var coautorExternoArticuloform = coautorExternoArticuloMapper.Map(model as CoautorExternoArticulo);
            coautorExternoArticuloform.ParentId = parentId;

            if (model.Institucion != null)
                coautorExternoArticuloform.InstitucionNombre = model.Institucion.Nombre;

            return coautorExternoArticuloform;
        }

        protected override ArticuloDifusion GetModelById(int id)
        {
            return articuloService.GetArticuloById(id);
        }

        ArticuloDifusionForm SetupNewForm()
        {
            return SetupNewForm(null);
        }

        ArticuloDifusionForm SetupNewForm(ArticuloDifusionForm form)
        {
            form = form ?? new ArticuloDifusionForm();

            form.LineasTematicas = lineaTematicaMapper.Map(catalogoService.GetActiveLineaTematicas());

            form.TipoArchivos = tipoArchivoMapper.Map(catalogoService.GetActiveTipoArchivos());
            form.EstadosProductos = customCollection.EstadoProductoCustomCollection();

            form.Areas = areaMapper.Map(catalogoService.GetActiveAreas());
            form.Disciplinas = GetDisciplinasByAreaId(form.AreaId);
            form.Subdisciplinas = GetSubdisciplinasByDisciplinaId(form.DisciplinaId);

            form.Paises = paisMapper.Map(catalogoService.GetActivePaises());

            if (form.Id == 0)
            {
                form.CoautorExternoArticulos = new CoautorExternoProductoForm[] {};
                form.CoautorInternoArticulos = new CoautorInternoProductoForm[] {};

                if (User.IsInRole("Investigadores"))
                {
                    form.UsuarioNombre = CurrentInvestigador().Usuario.Nombre;
                    form.UsuarioApellidoPaterno = CurrentInvestigador().Usuario.ApellidoPaterno;
                    form.UsuarioApellidoMaterno = CurrentInvestigador().Usuario.ApellidoMaterno;
                }
                form.PosicionCoautor = 1;
            }
            else
            {
                form.AreasTematicas =
                    areaTematicaMapper.Map(catalogoService.GetAreaTematicasByLineaTematicaId(form.LineaTematicaId));
            }

            return form;
        }

        void FormSetCombos(ArticuloDifusionForm form)
        {
            ViewData["TipoArticulo"] = form.TipoArticulo;
            ViewData["EstadoProducto"] = form.EstadoProducto;
            ViewData["Pais"] = form.PaisId;

            ViewData["AreaId"] = form.AreaId;
            ViewData["DisciplinaId"] = form.DisciplinaId;
            ViewData["SubdisciplinaId"] = form.SubdisciplinaId;

            ViewData["LineaTematicaId"] = form.LineaTematicaId;
            ViewData["AreaTematicaId"] = form.AreaTematicaId;

            ViewData["Pais"] = form.PaisId;
        }

        static ArticuloDifusionForm SetupShowForm(ArticuloDifusionForm form)
        {
            form = form ?? new ArticuloDifusionForm();

            form.ShowFields = new ShowFieldsForm
                                  {
                                      RevistaPublicacionTitulo = form.RevistaPublicacion.Titulo,
                                      RevistaPublicacionInstitucionNombre = form.RevistaPublicacion.InstitucionNombre,
                                      RevistaPublicacionIndice1Nombre = form.RevistaPublicacion.Indice1Nombre,
                                      RevistaPublicacionIndice2Nombre = form.RevistaPublicacion.Indice2Nombre,
                                      RevistaPublicacionIndice3Nombre = form.RevistaPublicacion.Indice3Nombre,
                                      SubdisciplinaNombre = form.SubdisciplinaNombre,
                                      DisciplinaNombre = form.DisciplinaNombre,
                                      AreaNombre = form.AreaNombre,
                                      ProyectoNombre = form.Proyecto.Nombre,
                                      EstadoProducto = form.EstadoProducto,
                                      FechaAceptacion = form.FechaAceptacion,
                                      FechaPublicacion = form.FechaPublicacion,
                                      ModelId = form.Id,
                                      PalabraClave1 = form.PalabraClave1,
                                      PalabraClave2 = form.PalabraClave2,
                                      PalabraClave3 = form.PalabraClave3,
                                      IsShowForm = true,
                                      RevistaLabel = "Nombre de la revista"
                                  };

            return form;
        }
    }
}