using System;
using System.Linq;
using DecisionesInteligentes.Colef.Sia.ApplicationServices;
using DecisionesInteligentes.Colef.Sia.Core;
using DecisionesInteligentes.Colef.Sia.Web.Controllers.Models;
using DecisionesInteligentes.Colef.Sia.Web.Extensions;
using SharpArch.Core.PersistenceSupport;

namespace DecisionesInteligentes.Colef.Sia.Web.Controllers.Mappers
{
    public class ArticuloDifusionMapper: AutoFormMapper<ArticuloDifusion, ArticuloDifusionForm>, IArticuloDifusionMapper
    {
        readonly ICatalogoService catalogoService;
        readonly ICoautorExternoProductoMapper<CoautorExternoProducto> coautorExternoArticuloMapper;
        readonly ICoautorInternoArticuloMapper coautorInternoArticuloMapper;
        readonly IProyectoService proyectoService;
        private Usuario usuarioArticulo;

        public ArticuloDifusionMapper(IRepository<ArticuloDifusion> repository,
                              ICoautorExternoProductoMapper<CoautorExternoProducto> coautorExternoArticuloMapper,
                              ICoautorInternoArticuloMapper coautorInternoArticuloMapper,
                              ICatalogoService catalogoService, IProyectoService proyectoService
        )
            : base(repository)
        {
            this.coautorExternoArticuloMapper = coautorExternoArticuloMapper;
            this.coautorInternoArticuloMapper = coautorInternoArticuloMapper;
            this.catalogoService = catalogoService;
            this.proyectoService = proyectoService;
        }

        protected override int GetIdFromMessage(ArticuloDifusionForm message)
        {
            return message.Id;
        }

        public override ArticuloDifusionForm Map(ArticuloDifusion model)
        {
            var message = base.Map(model);
            if (message.RevistaPublicacionId > 0)
                message.RevistaPublicacionTitulo = model.RevistaPublicacion.Titulo;

            message.CoautorExternoArticulos =
                coautorExternoArticuloMapper.Map(model.CoautorExternoArticulos.Cast<CoautorExternoProducto>().ToArray());

            return message;
        }

        protected override void MapToModel(ArticuloDifusionForm message, ArticuloDifusion model)
        {
            model.Titulo = message.Titulo;
            model.TieneProyecto = message.TieneProyecto;
            model.Volumen = message.Volumen;
            model.Numero = message.Numero;
            model.PalabraClave1 = message.PalabraClave1;
            model.PalabraClave2 = message.PalabraClave2;
            model.PalabraClave3 = message.PalabraClave3;
            model.EstadoProducto = message.EstadoProducto;
            model.PaginaInicial = message.PaginaInicial;
            model.PaginaFinal = message.PaginaFinal;
            model.TipoArticulo = message.TipoArticulo;
            model.CoautorSeOrdenaAlfabeticamente = message.CoautorSeOrdenaAlfabeticamente;
            model.Pais = catalogoService.GetPaisById(message.Pais);

            if (model.Usuario == null || model.Usuario == usuarioArticulo)
                model.PosicionCoautor = message.PosicionCoautor;

            if (message.EstadoProducto != 0 && message.EstadoProducto != 1)
            {
                if (message.FechaAceptacion.FromYearDateToDateTime() > DateTime.Parse("1910-01-01"))
                    model.FechaAceptacion = message.FechaAceptacion.FromYearDateToDateTime();
                if (message.FechaPublicacion.FromYearDateToDateTime() > DateTime.Parse("1910-01-01"))
                {
                    if (message.FechaAceptacion.FromYearDateToDateTime() == DateTime.Parse("1910-01-01"))
                        model.FechaAceptacion = message.FechaPublicacion.FromYearDateToDateTime();
                }
            }
            else
                model.FechaAceptacion = message.FechaAceptacion.FromYearDateToDateTime();

            model.FechaPublicacion = message.FechaPublicacion.FromYearDateToDateTime();

            var revistaPublicacion = catalogoService.GetRevistaPublicacionById(message.RevistaPublicacionId);
            if (revistaPublicacion != null && String.Compare(revistaPublicacion.Titulo, message.RevistaPublicacionTitulo) >= 0)
            {
                model.RevistaPublicacion = revistaPublicacion;
                model.RevistaPublicacionTitulo = String.Empty;
            }
            else
            {
                model.RevistaPublicacionTitulo = message.RevistaPublicacionTitulo;
                model.RevistaPublicacion = null;
            }
            
            model.AreaTematica = catalogoService.GetAreaTematicaById(message.AreaTematicaId);
            model.Proyecto = proyectoService.GetProyectoById(message.ProyectoId);

            model.Area = catalogoService.GetAreaById(message.AreaId);
            model.Disciplina = catalogoService.GetDisciplinaById(message.DisciplinaId);
            model.Subdisciplina = catalogoService.GetSubdisciplinaById(message.SubdisciplinaId);
        }

        public ArticuloDifusion Map(ArticuloDifusionForm message, Usuario usuario)
        {
            usuarioArticulo = usuario;
            var model = Map(message);

            model.ModificadoPor = usuario;

            return model;
        }

        public ArticuloDifusion Map(ArticuloDifusionForm message, Usuario usuario, Investigador investigador)
        {
            usuarioArticulo = usuario;
            var model = Map(message);

            if (model.IsTransient())
            {
                model.Usuario = usuario;
                model.CreadoPor = usuario;
                model.Sede = GetLatest(investigador.CargosInvestigador).Sede;
                model.Departamento = GetLatest(investigador.CargosInvestigador).Departamento;
            }

            if(model.Usuario != investigador.Usuario)
            {
                foreach (var coautor in model.CoautorInternoArticulos)
                {
                    if (coautor.Investigador == investigador)
                        coautor.Posicion = message.PosicionCoautor;
                }
            }

            model.ModificadoPor = usuario;

            return model;
        }

        public ArticuloDifusion Map(ArticuloDifusionForm message, Usuario usuario, Investigador investigador,
            CoautorExternoProductoForm[] coautoresExternos, CoautorInternoProductoForm[] coautoresInternos)
        {
            var model = Map(message, usuario, investigador);

            foreach (var coautoresExterno in coautoresExternos)
            {
                var coautor =
                    coautorExternoArticuloMapper.Map(coautoresExterno);
                
                coautor.CreadoPor = usuario;
                coautor.ModificadoPor = usuario;

                model.AddCoautorExterno(coautor);
            }

            foreach (var coautorInterno in coautoresInternos)
            {
                var coautor =
                    coautorInternoArticuloMapper.Map(coautorInterno);

                coautor.CreadoPor = usuario;
                coautor.ModificadoPor = usuario;

                model.AddCoautorInterno(coautor);
            }

            return model;
        }
    }
}