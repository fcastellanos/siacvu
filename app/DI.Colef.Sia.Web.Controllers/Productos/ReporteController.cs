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
    [HandleError]
    public class ReporteController : BaseController<Reporte, ReporteForm>
    {
        readonly ICoautorExternoProductoMapper<CoautorExternoReporte> coautorExternoReporteMapper;
        readonly ICoautorInternoReporteMapper coautorInternoReporteMapper;
        readonly ICustomCollection customCollection;
        readonly IInstitucionProductoMapper<InstitucionReporte> institucionReporteMapper;
        readonly IInvestigadorMapper investigadorMapper;
        readonly IInvestigadorService investigadorService;
        readonly ILineaTematicaMapper lineaTematicaMapper;
        readonly IProductoService productoService;
        readonly IProyectoMapper proyectoMapper;
        readonly IProyectoService proyectoService;
        readonly IReporteMapper reporteMapper;
        readonly IReporteService reporteService;

        public ReporteController(IReporteService reporteService,
                                 IReporteMapper reporteMapper,
                                 ICatalogoService catalogoService,
                                 IUsuarioService usuarioService,
                                 IProyectoMapper proyectoMapper,
                                 IInvestigadorMapper investigadorMapper,
                                 IInvestigadorService investigadorService,
                                 ICoautorExternoProductoMapper<CoautorExternoReporte> coautorExternoReporteMapper,
                                 ICustomCollection customCollection,
                                 ICoautorInternoReporteMapper coautorInternoReporteMapper,
                                 ISearchService searchService,
                                 IArchivoService archivoService,
                                 IProyectoService proyectoService,
                                 IAreaTematicaMapper areaTematicaMapper,
                                 ILineaTematicaMapper lineaTematicaMapper,
                                 IInstitucionMapper institucionMapper,
                                 IInvestigadorExternoMapper investigadorExternoMapper,
                                 IInstitucionProductoMapper<InstitucionReporte> institucionReporteMapper, IProductoService productoService)
            : base(usuarioService, searchService, catalogoService)
        {
            base.catalogoService = catalogoService;
            base.institucionMapper = institucionMapper;
            this.customCollection = customCollection;
            this.reporteService = reporteService;
            this.reporteMapper = reporteMapper;
            this.proyectoMapper = proyectoMapper;
            this.investigadorExternoMapper = investigadorExternoMapper;
            this.investigadorMapper = investigadorMapper;
            this.investigadorService = investigadorService;
            this.coautorExternoReporteMapper = coautorExternoReporteMapper;
            this.coautorInternoReporteMapper = coautorInternoReporteMapper;
            this.proyectoService = proyectoService;
            this.areaTematicaMapper = areaTematicaMapper;
            this.lineaTematicaMapper = lineaTematicaMapper;
            this.institucionReporteMapper = institucionReporteMapper;
            this.productoService = productoService;
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            var data = new GenericViewData<ReporteForm>();
            var productos = productoService.GetProductosByUsuario<Reporte>(CurrentUser(), x => x.Titulo,
                                                                           x => x.TipoReporte);
            data.ProductList = productos;

            return View(data);
        }

        [Authorize(Roles = "Investigadores")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult New()
        {
            if (CurrentInvestigador() == null)
                return NoInvestigadorProfile("Por tal motivo no puede crear nuevos productos.");

            var data = new GenericViewData<ReporteForm> {Form = SetupNewForm()};

            return View(data);
        }

        [Authorize(Roles = "Investigadores, DGAA")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            var data = new GenericViewData<ReporteForm>();
            var reporte = reporteService.GetReporteById(id);

            var verifyMessage = VerifyProductoStatus(reporte.Firma, reporte.Titulo);
            if (!String.IsNullOrEmpty(verifyMessage))
                return RedirectToHomeIndex(verifyMessage);

            var verifyOwnershipMessage = VerifyProductoOwnership(CurrentUser().Investigador, reporte.Usuario.Id,
                                                                 CurrentUser().Id);
            if (!String.IsNullOrEmpty(verifyOwnershipMessage))
                return RedirectToHomeIndex(verifyOwnershipMessage);

            CoautorInternoReporte coautorInternoReporte;
            int posicionAutor;
            var coautorExists = 0;

            if (User.IsInRole("Investigadores"))
            {
                coautorExists =
                    reporte.CoautorInternoReportes.Where(
                        x => x.Investigador.Id == CurrentInvestigador().Id).Count();

                if (reporte.Usuario.Id != CurrentUser().Id && coautorExists == 0)
                    return RedirectToHomeIndex("no lo puede modificar");
            }

            var reporteForm = reporteMapper.Map(reporte);
            if (reporte.AreaTematica != null)
                reporteForm.LineaTematicaId = reporte.AreaTematica.LineaTematica.Id;

            data.Form = SetupNewForm(reporteForm);
            FormSetCombos(data.Form);

            if (coautorExists != 0)
            {
                coautorInternoReporte =
                    reporte.CoautorInternoReportes.Where(x => x.Investigador.Id == CurrentInvestigador().Id).
                        FirstOrDefault();

                posicionAutor = coautorInternoReporte.Posicion;
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
            var data = new GenericViewData<ReporteForm>();

            var reporte = reporteService.GetReporteById(id);
            var reporteForm = reporteMapper.Map(reporte);

            data.Form = SetupShowForm(reporteForm);

            ViewData.Model = data;
            return View();
        }

        [CustomTransaction]
        [Authorize(Roles = "Investigadores")]
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([Bind(Prefix = "CoautorInterno")] CoautorInternoProductoForm[] coautorInterno,
                                   [Bind(Prefix = "CoautorExterno")] CoautorExternoProductoForm[] coautorExterno,
                                   [Bind(Prefix = "Institucion")] InstitucionProductoForm[] institucion,
                                   ReporteForm form)
        {
            coautorExterno = coautorExterno ?? new CoautorExternoProductoForm[] {};
            coautorInterno = coautorInterno ?? new CoautorInternoProductoForm[] {};
            institucion = institucion ?? new InstitucionProductoForm[] {};

            var reporte = reporteMapper.Map(form, CurrentUser(), CurrentInvestigador(),
                                            coautorExterno, coautorInterno, institucion);
            ModelState.AddModelErrors(reporte.ValidationResults(), true, "Reporte");
            if (!ModelState.IsValid)
            {
                return Rjs("ModelError");
            }

            reporteService.SaveReporte(reporte);
            SetMessage(String.Format("Reporte {0} ha sido registrado", reporte.Titulo));

            return Rjs("Save", reporte.Id);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(ReporteForm form)
        {
            var reporte = new Reporte();

            if (User.IsInRole("Investigadores"))
                reporte = reporteMapper.Map(form, CurrentUser(), CurrentInvestigador());
            if (User.IsInRole("DGAA"))
                reporte = reporteMapper.Map(form, CurrentUser());

            ModelState.AddModelErrors(reporte.ValidationResults(), true, "Reporte");
            if (!ModelState.IsValid)
            {
                return Rjs("ModelError");
            }

            reporteService.SaveReporte(reporte, true);
            SetMessage(String.Format("Reporte {0} ha sido modificado", reporte.Titulo));

            return Rjs("Save", reporte.Id);
        }

        [CookieLessAuthorize]
        [CustomTransaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddFile(FormCollection form)
        {
            var id = Convert.ToInt32(form["Id"]);
            var reporte = reporteService.GetReporteById(id);

            var archivo = MapArchivo<ArchivoReporte>();
            reporte.AddArchivo(archivo);

            reporteService.SaveReporte(reporte);

            return Content("Uploaded");
        }

        [CustomTransaction]
        [Authorize(Roles = "DGAA")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DgaaValidateProduct(FirmaForm firmaForm)
        {
            var reporte = reporteService.GetReporteById(firmaForm.ProductoId);

            reporte.Firma.Aceptacion2 = 1;
            reporte.Firma.Usuario2 = CurrentUser();

            reporteService.SaveReporte(reporte);

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
            var reporte = reporteService.GetReporteById(firmaForm.ProductoId);
            reporte.Firma.Aceptacion1 = 0;
            reporte.Firma.Aceptacion2 = 2;
            reporte.Firma.Descripcion = firmaForm.Descripcion;
            reporte.Firma.Usuario1 = CurrentUser();
            reporte.Firma.Usuario2 = CurrentUser();

            ModelState.AddModelErrors(reporte.ValidationResults(), false, "Firma");
            if (!ModelState.IsValid)
            {
                return Rjs("ModelError");
            }

            reporteService.SaveReporte(reporte, true);

            var data = new FirmaForm
                           {
                               TipoProducto = firmaForm.TipoProducto,
                               Aceptacion2 = 2
                           };

            return Rjs("DgaaSign", data);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public override ActionResult Search(string q)
        {
            var data = searchService.Search<Reporte>(x => x.Titulo, q);
            return Content(data);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ChangeInstitucionShort(int select)
        {
            var institucionForm = institucionMapper.Map(catalogoService.GetInstitucionById(select));

            var form = new ShowFieldsForm
                           {
                               InstitucionId = institucionForm.Id,
                               InstitucionPaisNombre = institucionForm.PaisNombre
                           };

            return Rjs("ChangeInstitucionShort", form);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ChangeAreaTematica(int select)
        {
            // TODO: Dependencias
            return Rjs("", null);
            //var areaTematicaForm = areaTematicaMapper.Map(catalogoService.GetAreaTematicaById(select));
            //var lineaTematicaForm = lineaTematicaMapper.Map(catalogoService.GetLineaTematicaById(areaTematicaForm.LineaTematicaId));

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
            var reporte = reporteService.GetReporteById(id);
            var form = new CoautorForm
                           {
                               Controller = "Reporte",
                               IdName = "ReporteId",
                               CoautorSeOrdenaAlfabeticamente = esAlfabeticamente
                           };

            if (User.IsInRole("Investigadores"))
                form.CreadoPorId = CurrentInvestigador().Id;

            if (reporte != null)
            {
                form.Id = reporte.Id;
                var investigador = investigadorService.GetInvestigadorByUsuario(reporte.CreadoPor.UsuarioNombre);
                form.CreadoPorId = investigador.Id;
            }

            return Rjs("NewCoautorInterno", form);
        }

        [CustomTransaction]
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddCoautorInterno([Bind(Prefix = "CoautorInterno")] CoautorInternoProductoForm form,
                                              int reporteId)
        {
            var coautorInternoReporte = coautorInternoReporteMapper.Map(form);

            ModelState.AddModelErrors(coautorInternoReporte.ValidationResults(), false, "CoautorInterno", String.Empty);
            if (!ModelState.IsValid)
            {
                return Rjs("ModelError");
            }

            if (reporteId != 0)
            {
                coautorInternoReporte.CreadoPor = CurrentUser();
                coautorInternoReporte.ModificadoPor = CurrentUser();

                var reporte = reporteService.GetReporteById(reporteId);

                var alreadyHasIt =
                    reporte.CoautorInternoReportes.Where(
                        x => x.Investigador.Id == coautorInternoReporte.Investigador.Id).Count();

                if (alreadyHasIt == 0)
                {
                    reporte.AddCoautorInterno(coautorInternoReporte);
                    reporteService.SaveReporte(reporte);
                }
            }

            var coautorInternoReporteForm = coautorInternoReporteMapper.Map(coautorInternoReporte);
            coautorInternoReporteForm.ParentId = reporteId;

            return Rjs("AddCoautorInterno", coautorInternoReporteForm);
        }

        [CustomTransaction]
        [Authorize]
        [AcceptVerbs(HttpVerbs.Delete)]
        public ActionResult DeleteCoautorInterno(int id, int investigadorId)
        {
            var reporte = reporteService.GetReporteById(id);

            if (reporte != null)
            {
                var coautor = reporte.CoautorInternoReportes.Where(x => x.Investigador.Id == investigadorId).First();
                reporte.DeleteCoautorInterno(coautor);

                reporteService.SaveReporte(reporte);
            }

            var form = new CoautorForm {ModelId = id, InvestigadorId = investigadorId};

            return Rjs("DeleteCoautorInterno", form);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Deactivate(int id)
        {
            var reporte = reporteService.GetReporteById(id);

            reporte.Activo = false;
            reporte.ModificadoPor = CurrentUser();

            reporteService.SaveReporte(reporte, true);

            var reporteForm = reporteMapper.Map(reporte);

            return Rjs("Deactivate", reporteForm);
        }

        protected override void DeleteCoautorExternoInModel(Reporte model, int coautorExternoId)
        {
            if (model == null) return;
            var coautor =
                model.CoautorExternoReportes.Where(x => x.InvestigadorExterno.Id == coautorExternoId).First();
            model.DeleteCoautorExterno(coautor);

            reporteService.SaveReporte(model, true);
        }

        protected override bool SaveCoautorExternoToModel(Reporte model, CoautorExternoProducto coautorExternoProducto)
        {
            ModelState.AddModelErrors(model.ValidationResults(), true, "Reporte");
            if (!ModelState.IsValid)
            {
                return false;
            }

            var alreadyHasIt =
                model.CoautorExternoReportes.Where(
                    x => x.InvestigadorExterno.Id == coautorExternoProducto.InvestigadorExterno.Id).Count();

            if (alreadyHasIt == 0)
            {
                model.AddCoautorExterno(coautorExternoProducto);
                reporteService.SaveReporte(model);
            }

            return alreadyHasIt == 0;
        }

        protected override CoautorExternoProducto MapCoautorExternoProductoMessage(CoautorExternoProductoForm form)
        {
            return coautorExternoReporteMapper.Map(form);
        }

        protected override CoautorExternoProductoForm MapCoautorExternoProductoModel(CoautorExternoProducto model, int parentId)
        {
            var coautorExternoReporteform = coautorExternoReporteMapper.Map(model as CoautorExternoReporte);
            coautorExternoReporteform.ParentId = parentId;

            if (model.Institucion != null)
                coautorExternoReporteform.InstitucionNombre = model.Institucion.Nombre;

            return coautorExternoReporteform;
        }

        protected override void DeleteInstitucionInModel(Reporte model, int institucionId)
        {
            if (model != null)
            {
                var institucion = model.InstitucionReportes.Where(x => x.Id == institucionId).First();
                model.DeleteInstitucion(institucion);

                reporteService.SaveReporte(model);
            }
        }

        protected override bool SaveInstitucionToModel(Reporte model, InstitucionProducto institucionProducto)
        {
            var alreadyHasIt = AlreadyHasIt(model.Id, institucionProducto);
            var added = false;

            if (!alreadyHasIt)
            {
                model.AddInstitucion(institucionProducto);
                reporteService.SaveReporte(model, true);
                added = true;
            }

            return added;
        }

        protected override InstitucionProducto MapInstitucionMessage(InstitucionProductoForm form)
        {
            return institucionReporteMapper.Map(form);
        }

        protected override InstitucionProductoForm MapInstitucionModel(InstitucionProducto model, int parentId)
        {
            var institucionEventoForm = institucionReporteMapper.Map(model as InstitucionReporte);
            institucionEventoForm.ParentId = parentId;

            return institucionEventoForm;
        }

        protected override Reporte GetModelById(int id)
        {
            return reporteService.GetReporteById(id);
        }

        protected override bool AlreadyHasIt(int reporteId, InstitucionProducto institucionProducto)
        {
            var evento = reporteService.GetReporteById(reporteId);

            var institucionId = institucionProducto.Institucion != null ? institucionProducto.Institucion.Id : 0;

            var count =
                evento.InstitucionReportes.Where(
                    x => ((x.Institucion != null && institucionId > 0 && x.Institucion.Id == institucionId) &&
                          (x.InstitucionNombre == institucionProducto.InstitucionNombre))).Count();

            return count > 0;
        }

        ReporteForm SetupNewForm()
        {
            return SetupNewForm(null);
        }

        ReporteForm SetupNewForm(ReporteForm form)
        {
            form = form ?? new ReporteForm();

            form.LineasTematicas = lineaTematicaMapper.Map(catalogoService.GetActiveLineaTematicas());

            form.TiposReportes = customCollection.TipoReporteCustomCollection();
            form.EstadosProductos = customCollection.EstadoProductoCustomCollection();
            form.Proyectos = proyectoMapper.Map(proyectoService.GetActiveProyectos());
            form.CoautoresExternos = investigadorExternoMapper.Map(catalogoService.GetActiveInvestigadorExternos());
            form.CoautoresInternos = investigadorMapper.Map(investigadorService.GetActiveInvestigadores());

            if (form.Id == 0)
            {
                form.CoautorExternoReportes = new CoautorExternoProductoForm[] {};
                form.CoautorInternoReportes = new CoautorInternoProductoForm[] {};

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

        void FormSetCombos(ReporteForm form)
        {
            ViewData["TipoReporte"] = form.TipoReporte;
            ViewData["EstadoProducto"] = form.EstadoProducto;

            ViewData["LineaTematicaId"] = form.LineaTematicaId;
            ViewData["AreaTematicaId"] = form.AreaTematicaId;
        }

        static ReporteForm SetupShowForm(ReporteForm form)
        {
            form = form ?? new ReporteForm();

            form.ShowFields = new ShowFieldsForm
                                  {
                                      EstadoProducto = form.EstadoProducto,
                                      FechaAceptacion = form.FechaAceptacion,
                                      FechaPublicacion = form.FechaPublicacion,
                                      ModelId = form.Id,
                                      PalabraClave1 = form.PalabraClave1,
                                      PalabraClave2 = form.PalabraClave2,
                                      PalabraClave3 = form.PalabraClave3,
                                      ProyectoNombre = form.ProyectoNombre,
                                      IsShowForm = true,
                                      InstitucionLabel = "Instancia a la que se presenta el reporte"
                                  };

            return form;
        }
    }
}