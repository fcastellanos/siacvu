﻿using System;
using NHibernate.Validator.Engine;

namespace DecisionesInteligentes.Colef.Sia.Core.NHibernateValidator
{
    [AttributeUsage(AttributeTargets.Class)]
    [ValidatorClass(typeof(ParticipacionMedioValidator))]
    public class ParticipacionMedioValidatorAttribute : Attribute, IRuleArgs
    {
        public ParticipacionMedioValidatorAttribute()
        {
            Message = "Entidad invalidad";
        }

        public string Message { get; set; }
    }

    public class ParticipacionMedioValidator : BaseValidatorAttribute<ParticipacionMedioValidatorAttribute>
    {
        public override void Initialize(ParticipacionMedioValidatorAttribute parameters)
        {
        }

        public override bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
        {
            var isValid = true;
            var participacionMedio = value as ParticipacionMedio;

            if (!participacionMedio.IsTransient())
            {
                isValid &= !ValidateIsNullOrEmpty<ParticipacionMedio>(participacionMedio, x => x.MedioImpreso, constraintValidatorContext);
                isValid &= !ValidateIsNullOrEmpty<ParticipacionMedio>(participacionMedio, x => x.MedioElectronico, constraintValidatorContext);
                isValid &= !ValidateIsNullOrEmpty<ParticipacionMedio>(participacionMedio, x => x.Genero, constraintValidatorContext);
                isValid &= !ValidateIsNullOrEmpty<ParticipacionMedio>(participacionMedio, x => x.LineaTematica, "LineaTematicaNombre",
                                                            constraintValidatorContext);
                isValid &= !ValidateIsNullOrEmpty<ParticipacionMedio>(participacionMedio, x => x.Pais, constraintValidatorContext);
                isValid &= !ValidateIsNullOrEmpty<ParticipacionMedio>(participacionMedio, x => x.Tema, constraintValidatorContext);
                isValid &= !ValidateIsNullOrEmpty<ParticipacionMedio>(participacionMedio, x => x.Nombre, constraintValidatorContext);
                isValid &= !ValidateIsNullOrEmpty<ParticipacionMedio>(participacionMedio, x => x.Ciudad, constraintValidatorContext);
                isValid &= !ValidateIsNullOrEmpty<ParticipacionMedio>(participacionMedio, x => x.EstadoPais, constraintValidatorContext);
                isValid &= !ValidateIsNullOrEmpty<ParticipacionMedio>(participacionMedio, x => x.FechaDifusion, constraintValidatorContext);
                isValid &= !ValidateIsNullOrEmpty<ParticipacionMedio>(participacionMedio, x => x.Ambito, constraintValidatorContext);
            }

            if (participacionMedio.MedioElectronico != null)
                isValid &= ValidateMedioElectronico(participacionMedio, constraintValidatorContext);

            if (participacionMedio.MedioImpreso != null)
                isValid &= ValidateMedioImpreso(participacionMedio, constraintValidatorContext);

            return isValid;
        }

        bool ValidateMedioElectronico(ParticipacionMedio participacionMedio, IConstraintValidatorContext constraintValidatorContext)
        {
            var isValid = true;

            if (participacionMedio.MedioElectronico.Nombre.Contains("Otro"))
            {
                if (participacionMedio.EspecificacionMedioElectronico == null)
                {
                    constraintValidatorContext.AddInvalid(
                        "no debe ser nulo o vacío o cero|EspecificacionMedioElectronico", "EspecificacionMedioElectronico");

                    isValid = false;
                }
            }

            return isValid;
        }

        bool ValidateMedioImpreso(ParticipacionMedio participacionMedio, IConstraintValidatorContext constraintValidatorContext)
        {
            var isValid = true;

            if (participacionMedio.MedioImpreso.Nombre.Contains("Otro"))
            {
                if (participacionMedio.EspecificacionMedioImpreso == null)
                {
                    constraintValidatorContext.AddInvalid(
                        "no debe ser nulo o vacío o cero|EspecificacionMedioImpreso", "EspecificacionMedioImpreso");

                    isValid = false;
                }
            }

            return isValid;
        }
    }
}
