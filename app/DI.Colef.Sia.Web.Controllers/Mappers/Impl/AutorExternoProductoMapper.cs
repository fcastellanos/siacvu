using System;
using DecisionesInteligentes.Colef.Sia.ApplicationServices;
using DecisionesInteligentes.Colef.Sia.Core;
using DecisionesInteligentes.Colef.Sia.Web.Controllers.Models;
using SharpArch.Core.DomainModel;
using SharpArch.Core.PersistenceSupport;

namespace DecisionesInteligentes.Colef.Sia.Web.Controllers.Mappers
{
    public class AutorExternoProductoMapper<TModel> : AutorExternoAutoMapper<TModel>, IAutorExternoProductoMapper<TModel> where TModel : Entity, new()
    {
        public AutorExternoProductoMapper(IRepository<TModel> repository, ICatalogoService catalogoService) : base(repository, catalogoService)
        {}
    }

    public class AutorExternoAutoMapper<TModel> : AutoFormMapper<AutorExternoProducto, AutorExternoProductoForm> where TModel : Entity, new()
    {
        readonly IRepository<TModel> repository;
        readonly ICatalogoService catalogoService;

        public AutorExternoAutoMapper(IRepository<TModel> repository, ICatalogoService catalogoService): base(null)
        {
            this.repository = repository;
            this.catalogoService = catalogoService;
        }

        protected override int GetIdFromMessage(AutorExternoProductoForm message)
        {
            return message.Id;
        }

        public override AutorExternoProducto Map(AutorExternoProductoForm message)
        {
            var model = repository.Get(GetIdFromMessage(message)) ?? new TModel();
            MapToModel(message, model as AutorExternoProducto);

            return model as AutorExternoProducto;
        }

        protected override void MapToModel(AutorExternoProductoForm message, AutorExternoProducto model)
        {
            model.InvestigadorExterno = catalogoService.GetInvestigadorExternoById(message.InvestigadorExternoId);
            model.AutorSeOrdenaAlfabeticamente = message.AutorSeOrdenaAlfabeticamente;
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

        public new AutorExternoProductoForm[] Map(AutorExternoProducto[] model)
        {
            var messages = Map<AutorExternoProducto[], AutorExternoProductoForm[]>(model);

            for (var i = 0; i < model.Length; i++)
            {
                if (messages[i].InstitucionId > 0)
                    messages[i].InstitucionNombre = model[i].Institucion.Nombre;
            }

            return messages;
        }
    }
}
