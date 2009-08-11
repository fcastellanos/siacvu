using System;
using SharpArch.Core.DomainModel;

namespace DecisionesInteligentes.Colef.Sia.Core
{
    public class TipoActividadMovilidadAcademica : Entity, IBaseEntity
    {
        public virtual TipoActividad TipoActividad { get; set; }

        public virtual Usuario CreadorPor { get; set; }

        public virtual DateTime CreadorEl { get; set; }

        public virtual Usuario ModificadoPor { get; set; }

        public virtual DateTime ModificadoEl { get; set; }

        public virtual bool Activo { get; set; }
    }
}
