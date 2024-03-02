using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hl7.Fhir.Model;
using Fhir_Profile.US_Core;


namespace Fhir_Profile.Validation
{
    /// <summary>
    /// Class to validate Observation Objects against the USCore Vital Signs Profile
    /// https://build.fhir.org/ig/HL7/US-Core/StructureDefinition-us-core-blood-pressure.html
    /// </summary>
    public class UsCoreBloodPressureValidator : AbstractValidator<Observation>
    {
        /// <summary>
        /// Create a default instance of the US Core Blood Pressure Validator
        /// </summary>
        public UsCoreBloodPressureValidator()
        {
            // validate against US Core Vital Signs 
            RuleFor(observation => observation)
                .SetValidator(new UsCoreVitalSignsValidator());

            RuleFor(observation => observation.Code)
                .ConceptConatains(UsCoreBloodPressure.UrlCodeSystemLoinc, UsCoreBloodPressure.LoincCodeBloodPressurePanel);

            RuleFor(observation => observation.Component)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .Must(list =>
                    list.Any(component => 
                        component.Code.Coding.Any(coding =>
                            (coding.System == UsCoreBloodPressure.UrlCodeSystemLoinc) &&
                            (coding.Code == UsCoreBloodPressure.LoincCodeSystolic))))
                .WithMessage($"UsCoreBloodPressure requires a component: {UsCoreBloodPressure.UrlCodeSystemLoinc}#{UsCoreBloodPressure.LoincCodeSystolic}");

            RuleFor(observation => observation.Component)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .Must(list =>
                    list.Any(component =>
                        component.Code.Coding.Any(coding =>
                            (coding.System == UsCoreBloodPressure.UrlCodeSystemLoinc) &&
                            (coding.Code == UsCoreBloodPressure.LoincCodeDiastolic))))
                .WithMessage($"UsCoreBloodPressure requires a component: {UsCoreBloodPressure.UrlCodeSystemLoinc}#{UsCoreBloodPressure.LoincCodeDiastolic}");
        }
    }
}
