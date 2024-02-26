using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;

namespace Fhir_Profile.US_Core
{

    /// <summary>
    /// Class with Observation extensions for US-Core Vital Signs objects
    /// https://www.hl7.org/fhir/us/core/StructureDefinition-us-core-vital-signs.html
    /// </summary>
    public static class UsCoreVitalSigns
    {

        /// <summary>
        /// Official url for the us-core Vital Signs profile , used to assert conformance. 
        /// </summary>
        public const string ProfileUrl = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-vital-signs";

        public const string UrlCodeSystemObservationCategory = "http://terminology.hl7.org/CodeSystem/observation-category";

        public const string ObservationCategoryVitalSigns = "vital-signs";

        /// <summary>
        /// Set or clear the assertion that a resource object confirms to the US core Observation Vital Signs Profile.
        /// </summary>
        /// <param name="resource"></param>
        public static void UsCoreVitalSignsProfileSet(this Observation resource)
        {
            if( resource == null ) { throw new ArgumentNullException(nameof(resource)); }

            if(resource.Meta == null)
            {
                // metadata that provides technical and workflow context to the resource
                resource.Meta = new Meta();
            }

            // set last updated so that meta is never empty
            resource.Meta.LastUpdated = DateTimeOffset.Now;

            if((resource.Meta.Profile == null)||(resource.Meta.Profile.Count() == 0))
            {
                resource.Meta.Profile = new List<string>()
                {
                    ProfileUrl,
                };

                return;
            }

            if(resource.Meta.Profile.Contains( ProfileUrl ))
            {
                return;
            }

            resource.Meta.Profile = resource.Meta.Profile.Append(ProfileUrl);
        }

        /// <summary>
        /// Set or clear the assertion that a resource object confirms to the US core Vital Signs Profile.
        /// </summary>
        /// <param name="resource"></param>
        public static void UsCoreVitalSignsProfileClear(this Observation resource)
        {
            if (resource == null) { throw new ArgumentNullException(nameof(resource)); }

            if (resource.Meta == null)
            {
                return ;
            }

            // set last updated so that meta is never empty
            resource.Meta.LastUpdated = DateTimeOffset.Now;

            if ((resource.Meta.Profile == null) || (resource.Meta.Profile.Count() == 0))
            {
                return;
            }

            if (resource.Meta.Profile.Contains(ProfileUrl))
            {

                int index = 0;
                foreach (string profile in resource.Meta.Profile)
                {
                    if (profile.Equals(ProfileUrl, StringComparison.Ordinal))
                    {
                        break;
                    }
                    index++;
                }

                resource.Meta.ProfileElement.RemoveAt(index);
            }

        }

        /// <summary>
        /// Set the required code for a US Core Vital Signs observation
        /// </summary>
        /// <param name="resource"></param>
        public static void UsCoreVitalSignsCategorySet(this Observation resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            resource.Category.RemoveAll(
                cat => (cat.Coding?.Any(
                    coding => (coding.System == UrlCodeSystemObservationCategory) && (coding.Code == ObservationCategoryVitalSigns))) == true);

            resource.Category.Add(new CodeableConcept(UrlCodeSystemObservationCategory, ObservationCategoryVitalSigns));
        }

 
    }
}
