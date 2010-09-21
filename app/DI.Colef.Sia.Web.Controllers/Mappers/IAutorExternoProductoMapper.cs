using DecisionesInteligentes.Colef.Sia.Core;
using DecisionesInteligentes.Colef.Sia.Web.Controllers.Models;
using SharpArch.Core.DomainModel;

namespace DecisionesInteligentes.Colef.Sia.Web.Controllers.Mappers
{
    public interface IAutorExternoProductoMapper<TModel> : IMapper<AutorExternoProducto, AutorExternoProductoForm> where TModel : Entity, new()
    {
    }
}
