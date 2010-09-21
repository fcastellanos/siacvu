using System;
using System.Linq;
using DecisionesInteligentes.Colef.Sia.ApplicationServices;
using DecisionesInteligentes.Colef.Sia.Core;
using DecisionesInteligentes.Colef.Sia.Web.Controllers.Models;
using DecisionesInteligentes.Colef.Sia.Web.Extensions;
using SharpArch.Core.PersistenceSupport;

namespace DecisionesInteligentes.Colef.Sia.Web.Controllers.Mappers
{
    public class ReporteMapper : AutoFormMapper<Reporte, ReporteForm>, IReporteMapper
    {
        readonly ICatalogoService catalogoService;
        readonly ICoautorExternoProductoMapper<CoautorExternoProducto> coautorExternoReporteMapper;
        readonly ICoautorInternoReporteMapper coautorInternoReporteMapper;
        readonly IProyectoService proyectoService;
        readonly IInstitucionProductoMapper<InstitucionReporte> institucionReporteMapper;
        private Usuario usuarioReporte;

        public ReporteMapper(IRepository<Reporte> repository, ICatalogoService catalogoService,
                             ICoautorExternoProductoMapper<CoautorExternoProducto> coautorExternoReporteMapper, ICoautorInternoReporteMapper coautorInternoReporteMapper,
                             IProyectoService proyectoService, IInstitucionProductoMapper<InstitucionReporte> institucionReporteMapper)
            : base(repository)
        {
            this.catalogoService = catalogoService;
            this.coautorExternoReporteMapper = coautorExternoReporteMapper;
            this.coautorInternoReporteMapper = coautorInternoReporteMapper;
            this.proyectoService = proyectoService;
            this.institucionReporteMapper = institucionReporteMapper;
        }

        protected override int GetIdFromMessage(ReporteForm message)
        {
            return message.Id;
        }

        public override ReporteForm Map(Reporte model)
        {
            var message = base.Map(model);
            message.InstitucionReportes = institucionReporteMapper.Map(model.InstitucionReportes.Cast<InstitucionProducto>().ToArray());
            message.CoautorExternoReportes = coautorExternoReporteMapper.Map(model.CoautorExternoReportes.Cast<CoautorExternoProducto>().ToArray());

            return message;
        }

        protected override void MapToModel(ReporteForm message, Reporte model)
        {
            model.Titulo = message.Titulo;
            model.NoPaginas = message.NoPaginas;
            model.Descripcion = message.Descripcion;
            model.Objetivo = message.Objetivo;
            model.PalabraClave1 = message.PalabraClave1;
            model.PalabraClave2 = message.PalabraClave2;
            model.PalabraClave3 = message.PalabraClave3;
            model.TieneProyecto = message.TieneProyecto;
            model.TipoReporte = message.TipoReporte;
            model.Numero = message.Numero;
            model.CoautorSeOrdenaAlfabeticamente = message.CoautorSeOrdenaAlfabeticamente;

            if (model.Usuario == null || model.Usuario == usuarioReporte)
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

            model.EstadoProducto = message.EstadoProducto;
            model.Proyecto = proyectoService.GetProyectoById(message.ProyectoId);
            //model.Institucion = catalogoService.GetInstitucionById(message.InstitucionId);
            model.AreaTematica = catalogoService.GetAreaTematicaById(message.AreaTematicaId);
        }

        public Reporte Map(ReporteForm message, Usuario usuario)
        {
            usuarioReporte = usuario;
            var model = Map(message);

            model.ModificadoPor = usuario;

            return model;
        }

        public Reporte Map(ReporteForm message, Usuario usuario, Investigador investigador)
        {
            usuarioReporte = usuario;
            var model = Map(message);

            if (model.IsTransient())
            {
                model.Usuario = usuario;
                model.CreadoPor = usuario;
                model.Sede = GetLatest(investigador.CargosInvestigador).Sede;
                model.Departamento = GetLatest(investigador.CargosInvestigador).Departamento;
            }

            if (model.Usuario != investigador.Usuario)
            {
                foreach (var coautor in model.CoautorInternoReportes)
                {
                    if (coautor.Investigador == investigador)
                        coautor.Posicion = message.PosicionCoautor;
                }
            }

            model.ModificadoPor = usuario;

            return model;
        }

        public Reporte Map(ReporteForm message, Usuario usuario, Investigador investigador,
            CoautorExternoProductoForm[] coautoresExternos, CoautorInternoProductoForm[] coautoresInternos,
            InstitucionProductoForm[] instituciones)
        {
            var model = Map(message, usuario, investigador);

            foreach (var coautorExterno in coautoresExternos)
            {
                var coautor =
                    coautorExternoReporteMapper.Map(coautorExterno);

                coautor.CreadoPor = usuario;
                coautor.ModificadoPor = usuario;

                model.AddCoautorExterno(coautor);
            }

            foreach (var coautorInterno in coautoresInternos)
            {
                var coautor =
                    coautorInternoReporteMapper.Map(coautorInterno);

                coautor.CreadoPor = usuario;
                coautor.ModificadoPor = usuario;

                model.AddCoautorInterno(coautor);
            }

            foreach (var institucion in instituciones)
            {
                var institucionReporte =
                    institucionReporteMapper.Map(institucion);

                institucionReporte.CreadoPor = usuario;
                institucionReporte.ModificadoPor = usuario;

                model.AddInstitucion(institucionReporte);
            }

            return model;
        }
    }
}