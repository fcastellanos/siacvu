using System;
using DecisionesInteligentes.Colef.Sia.ApplicationServices;
using DecisionesInteligentes.Colef.Sia.Core;
using DecisionesInteligentes.Colef.Sia.Web.Controllers.Models;
using SharpArch.Core.DomainModel;
using SharpArch.Core.PersistenceSupport;

namespace DecisionesInteligentes.Colef.Sia.Web.Controllers.Mappers
{
    public class CoautorExternoProductoMapper<TModel> : CoautorExternoAutoMapper<TModel>, ICoautorExternoProductoMapper<TModel> where TModel : Entity, new()
    {
        public CoautorExternoProductoMapper(IRepository<TModel> repository, ICatalogoService catalogoService) : base(repository, catalogoService)
        {
        }
    }

    public class CoautorExternoAutoMapper<TModel> : AutoFormMapper<CoautorExternoProducto, CoautorExternoProductoForm> where TModel : Entity, new()
    {
        readonly IRepository<TModel> repository;
        readonly ICatalogoService catalogoService;

        public CoautorExternoAutoMapper(IRepository<TModel> repository, ICatalogoService catalogoService): base(null)
        {
            this.repository = repository;
            this.catalogoService = catalogoService;
        }

        protected override int GetIdFromMessage(CoautorExternoProductoForm message)
        {
            return message.Id;
        }

        public override CoautorExternoProducto Map(CoautorExternoProductoForm message)
        {
            TModel model = repository.Get(GetIdFromMessage(message)) ?? new TModel();
            MapToModel(message, model as CoautorExternoProducto);
            return model as CoautorExternoProducto;
        }

        protected override void MapToModel(CoautorExternoProductoForm message, CoautorExternoProducto model)
        {
            model.InvestigadorExterno = catalogoService.GetInvestigadorExternoById(message.InvestigadorExternoId);
            model.CoautorSeOrdenaAlfabeticamente = message.CoautorSeOrdenaAlfabeticamente;
            model.Posicion = message.Posicion;

            var institucion = catalogoService.GetInstitucionById(message.InstitucionId);
            if (institucion != null && string.Compare(institucion.Nombre, message.Institucion) >= 0)
            {
                model.Institucion = institucion;
                model.InstitucionNombre = string.Empty;
            }
            else
            {
                model.InstitucionNombre = message.Institucion;
                model.Institucion = null;
            }

            if (model.IsTransient())
            {
                model.Activo = true;
                model.CreadoEl = DateTime.Now;
            }

            model.ModificadoEl = DateTime.Now;
        }
        
        public new CoautorExternoProductoForm[] Map(CoautorExternoProducto[] model)
        {
            var messages = Map<CoautorExternoProducto[], CoautorExternoProductoForm[]>(model);
            for (int i = 0; i < model.Length; i++)
            {
                if (messages[i].InstitucionId > 0)
                    messages[i].InstitucionNombre = model[i].Institucion.Nombre;
            }

            return messages;
        }
    }
}
