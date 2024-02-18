using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.Specification;
using Hl7.Fhir.Specification.Source;
using Hl7.Fhir.Validation;
using System.ComponentModel.DataAnnotations;
using Validator = Hl7.Fhir.Validation.Validator;

namespace cs_fhir_profile
{
    /// <summary>
    ///  Program to test working with IGs/Profile
    /// </summary>
    /// <param name="patientJsonFilename">Output JSON file of our patient</param>
    /// <param name="outcomeJsonFilename">Output JSON file of our OperationOutcome</param>
    /// <param name="profileDirectory">Directory containing expanded profile packages</param>
    public class Program
    {
        public static void Main(
            string patientJsonFilename = "",
            string outcomeJsonFilename = "",
            string profileDirectory = ""
        )
        {

            //string rootDir = Directory.GetCurrentDirectory();
            string rootDir = "C:/Users/lgao142/OneDrive - The University of Auckland/Desktop/c#/cs_fhir/cs_fhir_profile/cs_fhir_profile/";
            Console.WriteLine(rootDir);
            //Console.WriteLine(ZipSource.CreateValidationSource());

            //if (string.IsNullOrEmpty(patientJsonFilename))
            //{
            //    patientJsonFilename = Path.Combine(rootDir, "patient.json");
            //}

            //if (string.IsNullOrEmpty(outcomeJsonFilename))
            //{
            //    outcomeJsonFilename = Path.Combine(rootDir, "outcome.json");
            //}

            //if (string.IsNullOrEmpty(profileDirectory))
            //{
            //    profileDirectory = Path.Combine(rootDir, "profiles");
            //    Console.WriteLine(profileDirectory);
            //}

            if (string.IsNullOrEmpty(patientJsonFilename))
            {
                patientJsonFilename = "C:/Users/lgao142/OneDrive - The University of Auckland/Desktop/c#/cs_fhir/Fhir-Profile/patient.json";
            }

            if (string.IsNullOrEmpty(outcomeJsonFilename))
            {
                outcomeJsonFilename = "C:/Users/lgao142/OneDrive - The University of Auckland/Desktop/c#/cs_fhir/Fhir-Profile/outcome.json";
            }

            if (string.IsNullOrEmpty(profileDirectory))
            {
                profileDirectory = "C:/Users/lgao142/OneDrive - The University of Auckland/Desktop/c#/cs_fhir/Fhir-Profile/profiles";
            }

            Patient patient = new Patient()
            {
                Meta = new Meta()
                {
                    Profile = new List<string>()
                    {
                        "http://hl7.org/fhir/us/core/StructureDefinition/us-core-patient",
                    }
                }
            };

            FhirJsonSerializer fhirJsonSerializer = new FhirJsonSerializer(new SerializerSettings()
            {
                Pretty = true,
            });

            string patientJson = fhirJsonSerializer.SerializeToString(patient);

            File.WriteAllText(patientJsonFilename, patientJson);

            // create a cached resolver for resource validation
            IResourceResolver resolver = new CachedResolver(
              // create a multi-resolver, which can resolve resources from more than one source
              new MultiResolver(
                // create the default FHIR specification resolver (specification.zip included in HL7.fhir.specification.r4)
                ZipSource.CreateValidationSource(),
                // create the directory source resolver, which points to our profiles directory
                new DirectorySource(profileDirectory, new DirectorySourceSettings()
                {
                    IncludeSubDirectories = true,
                })
              )
            );

            ValidationSettings validationSettings = new ValidationSettings()
            {
                ResourceResolver = resolver,
            };

            Validator validator = new Validator(validationSettings);

            OperationOutcome outcome = validator.Validate(patient);

            string outcomJson = fhirJsonSerializer.SerializeToString(outcome);

            File.WriteAllText(outcomeJsonFilename, outcomJson);

            Console.WriteLine(outcomJson);
        }
    }
}
