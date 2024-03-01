using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fhir_Profile.US_Core;
using FluentValidation;
using Hl7.Fhir.Model;

namespace Fhir_Profile.Validation
{
    /// <summary>
    /// Class to validate Observation Objects against the USCore Vital Signs Profile
    /// https://build.fhir.org/ig/HL7/US-Core/StructureDefinition-us-core-vital-signs.html
    /// </summary>
    public class UsCoreVitalSignsValidator : AbstractValidator<Observation>
    {

        /// <summary>
        /// Create default instance of the US Core Vital Signs Validator
        /// </summary>
        public UsCoreVitalSignsValidator()
        {
            //RuleFor(observation => observation.Category)
            //     .Cascade(CascadeMode.Stop)
            //     .NotNull()
            //     .NotEmpty()
            //     .Must(categories => categories.Any(
            //         category => category.Coding.Any(
            //             code => (code.System == UsCoreVitalSigns.UrlCodeSystemObservationCategory) && 
            //                     (code.Code == UsCoreVitalSigns.ObservationCategoryVitalSigns))));

            RuleFor(observation => observation.Category)
                .ConceptListConatains(UsCoreVitalSigns.UrlCodeSystemObservationCategory, UsCoreVitalSigns.ObservationCategoryVitalSigns);
        }
    }
}
