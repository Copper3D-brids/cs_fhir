using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;

namespace cs_fhir_profile
{
    /// <summary>
    ///  Program to test working with IGs/Profile
    /// </summary>
    /// <param name="patientJsonFilename">Output JSON file of our patient</param>
    /// 
    public class Program
    {
        public static void Main(
            string patientJsonFilename = ""    
        )
        {

            if (string.IsNullOrEmpty(patientJsonFilename))
            {
                patientJsonFilename = "C:/Users/lgao142/OneDrive - The University of Auckland/Desktop/c#/cs_fhir/cs_fhir_profile/cs_fhir_profile/patient.json";
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

            string json = fhirJsonSerializer.SerializeToString(patient);

            File.WriteAllText(patientJsonFilename, json);
        }
    }
}
