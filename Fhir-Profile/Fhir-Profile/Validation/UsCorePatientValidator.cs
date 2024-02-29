using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Hl7.Fhir.Model;

namespace Fhir_Profile.Validation
{
    /// <summary>
    /// Class to validate Patient Objects against the USCore Patient Profile
    /// https://www.hl7.org/fhir/us/core/StructureDefinition-us-core-patient.html
    /// </summary>
    internal class UsCorePatientValidator : AbstractValidator<Patient>
    {
        /// <summary>
        /// Canonical URL for the Data Absent Reason extension
        /// https://hl7.org/fhir/extensions/StructureDefinition-data-absent-reason.html
        /// </summary>
        public const string UrlExtensionDataAbsentReason = "http://hl7.org/fhir/StructureDefinition/data-absent-reason";

        /// <summary>
        /// Create default instance of the US Core Patient Validator
        /// </summary>
        public UsCorePatientValidator()
        {
            // Patient.Identifier: minimum one with a system and value
            RuleFor(patient => patient.Identifier)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .Must(list => list.Any(id => (!string.IsNullOrEmpty(id.System)) && (!string.IsNullOrEmpty(id.Value))))
                .WithMessage("Patient.identifier requires one element with both a system and a value.");

            //Patient.name: minimum one with a family, given, or DAR
            RuleFor(patient => patient.Name)
                .Must(list => list.Any(name => TestUsCore6(name)))
                .WithMessage("Patient.name requires one name with a family, given, or Data Absent Reason");

            //Patient.name: minimum one, All with a family, given, or DAR
            RuleFor(patient => patient.Name)
                .Must(list => !list.Any(name => !TestUsCore6(name)))
                .WithMessage("Patient.name requires all names have a family, given, or Data Absent Reason");

            // Patient.gender: required
            RuleFor(patient => patient.Gender)
                .NotNull()
                .WithMessage("Patient.gender is required.");
        }

        /// <summary>
        /// Test a HumanNmae against the invariant us-core-6
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool TestUsCore6(HumanName name)
        {

            if (!string.IsNullOrEmpty(name.Family))
            {
                return true;
            }

            if((name.Given != null) && (name.Given.Any(given => !string.IsNullOrEmpty(given))))
            {
                return true;
            }

            if (name.GetExtensions(UrlExtensionDataAbsentReason).Any())
            {
                return true;
            }

            return false;
        }
    }
}
