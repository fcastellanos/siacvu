using System;
using System.Collections.Generic;
using DecisionesInteligentes.Colef.Sia.Core.NHibernateValidator;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using SharpArch.Core.NHibernateValidator;

namespace DecisionesInteligentes.Colef.Sia.Core
{
    [HasUniqueDomainSignature]
    [TesisDirigidaValidator]
    public class TesisDirigida : Entity, IBaseEntity
    {
        const int tipoProducto = 13; // 13 Representa Tesis Dirigida

        public TesisDirigida()
        {
            ArchivoTesisDirigidas = new List<ArchivoTesisDirigida>();
        }

        public virtual void AddArchivo(Archivo archivo)
        {
            archivo.TipoProducto = tipoProducto;
            ArchivoTesisDirigidas.Add((ArchivoTesisDirigida) archivo);
        }

        public virtual void DeleteArchivo(Archivo archivo)
        {
            ArchivoTesisDirigidas.Remove((ArchivoTesisDirigida) archivo);
        }

        [Valid]
        public virtual IList<ArchivoTesisDirigida> ArchivoTesisDirigidas { get; private set; }

        public virtual int TipoEstudiante { get; set; }

        [DomainSignature]
		public virtual string Titulo { get; set; }

        public virtual VinculacionAPyD VinculacionAPyD { get; set; }

        public virtual FormaParticipacion FormaParticipacion { get; set; }

		public virtual DateTime FechaConclusion { get; set; }

        public virtual bool Concluida { get; set; }

        public virtual Alumno Alumno { get; set; }

        public virtual string NombreAlumno { get; set; }

		public virtual DateTime FechaGrado { get; set; }

        public virtual int Puntuacion { get; set; }

		public virtual GradoAcademico GradoAcademico { get; set; }

		public virtual Pais Pais { get; set; }

		public virtual Institucion Institucion { get; set; }

		public virtual PeriodoReferencia PeriodoReferencia { get; set; }

		public virtual Sector Sector { get; set; }

		public virtual Organizacion Organizacion { get; set; }

		public virtual Nivel Nivel2 { get; set; }

		public virtual Area Area { get; set; }

		public virtual Disciplina Disciplina { get; set; }

		public virtual Subdisciplina Subdisciplina { get; set; }

        [NotNull]
        public virtual Usuario Usuario { get; set; }

        public virtual Departamento DepartamentoInvestigador { get; set; }

        public virtual Sede Sede { get; set; }

		public virtual Usuario CreadorPor { get; set; }

		public virtual DateTime CreadorEl { get; set; }

		public virtual Usuario ModificadoPor { get; set; }

		public virtual DateTime ModificadoEl { get; set; }

		public virtual bool Activo { get; set; }
    }
}
