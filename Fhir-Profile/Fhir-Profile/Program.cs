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
using Validator = Hl7.Fhir.Validation.Validator;
using Fhir_Profile.US_Core;
using Fhir_Profile;
using static Fhir_Profile.UsCoreRace;


namespace cs_fhir_profile
{
    /// <summary>
    ///  Program to test working with IGs/Profile
    /// </summary>
    /// <param name="resourceJsonFilename">Output JSON file of our patient</param>
    /// <param name="outcomeJsonFilename">Output JSON file of our OperationOutcome</param>
    /// <param name="profileDirectory">Directory containing expanded profile packages</param>
    public class Program
    {
        public static void Main(
            string resourceJsonFilename = "",
            string outcomeJsonFilename = "",
            string profileDirectory = ""
        )
        {
            // works on visual studio and vs code both
            string rootDir = System.IO.Path.GetFullPath(
            System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
            

            // only works on vs code
            //string rootDir = Directory.GetCurrentDirectory();
            //Console.WriteLine("root path: "+rootDir);
            //Console.WriteLine(ZipSource.CreateValidationSource());

            if (string.IsNullOrEmpty(resourceJsonFilename))
            {
               resourceJsonFilename = Path.Combine(rootDir, "resource.json");
            }

            if (string.IsNullOrEmpty(outcomeJsonFilename))
            {
               outcomeJsonFilename = Path.Combine(rootDir, "outcome.json");
            }

            if (string.IsNullOrEmpty(profileDirectory))
            {
               profileDirectory = Path.Combine(rootDir, "profiles");
            }


#if PATIENT
            //Create a FHIR Patient
            Patient resource = new Patient()
            {
                //
                //Meta = new Meta()
                //{
                //    // An assertion that the content conforms to a resource profile
                //    Profile = new List<string>()
                //    {
                //        // specifically, we want to confirm with us-core Patient
                //        UsCorePatient.ProfileUrl,
                //    }
                //},
                // add a simple extension - US-core Birthsex to the root of the patient
                //Extension = new List<Extension>()
                //{
                //    new Extension("http://hl7.org/fhir/us/core/StructureDefinition/us-core-birthsex", new Code("UNK"))
                //},

                // us-core requires an Identifier
                Identifier = new List<Identifier>()
                {
                    //new Identifier("http://example.org/fhir/patient/identifier", "ABC123"),
                    new Identifier("http://hospital.smarthealthit.org", "1032704")
                },

                //us-core requires a patient name with a: Given, Family, or Data Absent Reason
                Name = new List<HumanName>()
                {
                    new HumanName()
                    {
                        Given = new List<string>
                        {
                            "test",
                        }
                    }
                },
                Gender = AdministrativeGender.Unknown,
            };

            //Extension raceExt = new Extension("http://hl7.org/fhir/us/core/StructureDefinition/us-core-race", null);
            //raceExt.Extension = new List<Extension>()
            //{
            //    new Extension("ombCategory", new Coding("urn:oid:2.16.840.1.113883.6.238", "1002-5", "American Indian or Alaska Native")),
            //    new Extension("text", new FhirString("Race default text"))
            //};

            //patient.Extension.Add(raceExt);

            // add US Core Patient profile conformance
            resource.UsCorePatientProfileSet();
            //patient.UsCorePatientProfileClear();


            //add a US Core Birthsex
            resource.UsCoreBirthsexSet(UsCoreBirthsex.UsCoreBirthsexValues.Female);
            //patient.UsCoreBirthsexClear();

            resource.UsCoreRaceSet(
                "Race default text",
                new UsCoreRace.UsCoreOmbRaceCategoryValues[] { UsCoreRace.UsCoreOmbRaceCategoryValues.White });

            resource.UsCoreRaceTextSet("Just test!");

            resource.UsCoreRaceOmbCategoryAdd(UsCoreRace.UsCoreOmbRaceCategoryValues.NativeHawaiianOrOtherPacificIslander);

            if (resource.UsCoreBirthsexTryGet(out UsCoreBirthsex.UsCoreBirthsexValues? birthsex))
            {
                Console.WriteLine($"Found US Core Birthsex: {birthsex}");
            }
            else
            {
                Console.WriteLine("US Core Birthsex not found!");
            }

            if (resource.UsCoreRaceOMBCategoryTryGet(out List<UsCoreOmbRaceCategoryValues>? ombCategoryCodes))
            {
                foreach (var item in ombCategoryCodes)
                {
                    Console.WriteLine($"Found US Core ombCategory: {item}");
                }
            }
            else
            {
                Console.WriteLine("US Core ombCategory not found!");
            }

#endif

            Observation resource = new Observation()
            {
                Status = ObservationStatus.Unknown,
                Subject = new ResourceReference("Patient/test"),
                Effective = new FhirDateTime(2024, 02, 26, 10, 0 , 0, new TimeSpan()),
            };

            resource.UsCoreVitalSignsProfileSet();
            resource.UsCoreVitalSignsCategorySet(); 

            resource.UsCoreBloodPressureProfileSet();
            resource.UsCoreBloodPressureCodeSet();
            resource.UsCoreBloodPressureSystolicSet(125);
            resource.UsCoreBloodPressureDiastolicSet(75);

            // create a FHIR JSON serializer, using pretty-printing (nice formatting)
            FhirJsonSerializer fhirJsonSerializer = new FhirJsonSerializer(new SerializerSettings()
            {
                Pretty = true,
            });

            string resourceJson = fhirJsonSerializer.SerializeToString(resource);

            File.WriteAllText(resourceJsonFilename, resourceJson);

            // display our patient in console
            Console.WriteLine(resourceJson);


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

            OperationOutcome outcome = validator.Validate(resource);

            string outcomJson = fhirJsonSerializer.SerializeToString(outcome);

            File.WriteAllText(outcomeJsonFilename, outcomJson);

            Console.WriteLine(outcomJson);
        }
    }
}
