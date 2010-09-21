using System;
using DecisionesInteligentes.Colef.Sia.Core.NHibernateValidator;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;

namespace DecisionesInteligentes.Colef.Sia.Core
{
    [AutorExternoProductoValidator]
    public class AutorExternoProducto : Entity, IBaseEntity
    {
        [NotNull]
        public virtual InvestigadorExterno InvestigadorExterno { get; set; }

        public virtual Institucion Institucion { get; set; }

        public virtual string InstitucionNombre { get; set; }

        public virtual int TipoProducto { get; set; }

        public virtual int Posicion { get; set; }

        public virtual bool AutorSeOrdenaAlfabeticamente { get; set; }

        public virtual Usuario CreadoPor { get; set; }

        public virtual DateTime CreadoEl { get; set; }

        public virtual Usuario ModificadoPor { get; set; }

        public virtual DateTime ModificadoEl { get; set; }

        public virtual bool Activo { get; set; }
    }

    public class AutorExternoCapitulo : AutorExternoProducto
    {

    }

    public class AutorExternoResena : AutorExternoProducto
    {

    }

    public class AutorExternoObraTraducida : AutorExternoProducto
    {

    }
}