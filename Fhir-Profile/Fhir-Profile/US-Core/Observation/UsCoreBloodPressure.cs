﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;

namespace Fhir_Profile.US_Core
{

    /// <summary>
    /// Class with Observation extensions for US-Core blood pressure objects
    /// https://www.hl7.org/fhir/us/core/StructureDefinition-us-core-blood-pressure.html
    /// </summary>
    public static class UsCoreBloodPressure
    {

        /// <summary>
        /// Official url for the us-core Blood Pressure profile , used to assert conformance. 
        /// </summary>
        public const string ProfileUrl = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-blood-pressure";

        public const string UrlCodeSystemLoinc = "http://loinc.org";

        /// <summary>
        /// Blood pressure panel with all children optional
        /// https://loinc.org/85354-9
        /// </summary>
        public const string LoincCodeBloodPressurePanel = "85354-9";

        /// <summary>
        /// Systolic blood pressure
        /// </summary>
        public const string LoincCodeSystolic = "8480-6";

        /// <summary>
        /// Diastolic blood pressure
        /// </summary>
        public const string LoincCodeDiastolic = "8462-4";

        public const string UrlBloodPressureMeasure = "http://unitsofmeasure.org";

        public const string BloodPressureUnit = "mm[Hg]";

        /// <summary>
        /// Set or clear the assertion that a resource object confirms to the US core Observation blood pressure Profile.
        /// </summary>
        /// <param name="resource"></param>
        public static void UsCoreBloodPressureProfileSet(this Observation resource)
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
            
           resource.Meta.Profile =  resource.Meta.Profile.Append( ProfileUrl );
        }

        /// <summary>
        /// Set or clear the assertion that a resource object confirms to the US core Blood Pressure Profile.
        /// </summary>
        /// <param name="resource"></param>
        public static void UsCoreBloodPressureProfileClear(this Observation resource)
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
        /// Set the required code for a US Core Blood Pressure observation
        /// </summary>
        /// <param name="resource"></param>
        public static void UsCoreBloodPressureCodeSet(this Observation resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            resource.Code = new CodeableConcept(UrlCodeSystemLoinc, LoincCodeBloodPressurePanel);
        }

        /// <summary>
        /// Add or update a Systolic BP value
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="value"></param>
        public static void UsCoreBloodPressureSystolicSet(this Observation resource, decimal value)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            resource.Component.RemoveAll(
                component => (component.Code?.Coding?.Any(coding => (coding.System == UrlCodeSystemLoinc) && (coding.Code == LoincCodeSystolic))) == true);

            resource.Component.Add(new Observation.ComponentComponent()
            {
                Code = new CodeableConcept(UrlCodeSystemLoinc, LoincCodeSystolic),
                Value = new Quantity(value, BloodPressureUnit)
            });
        }

        /// <summary>
        /// Remove all Systolic BP values
        /// </summary>
        /// <param name="resource"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void UsCoreBloodPressureSystolicClear(this Observation resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            resource.Component.RemoveAll(
                component => (component.Code?.Coding?.Any(coding => (coding.System == UrlCodeSystemLoinc) && (coding.Code == LoincCodeSystolic))) == true);
        }

        /// <summary>
        /// Add or update a Diastolic BP value
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="value"></param>
        public static void UsCoreBloodPressureDiastolicSet(this Observation resource, decimal value)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            resource.Component.RemoveAll(
                component => (component.Code?.Coding?.Any(coding => (coding.System == UrlCodeSystemLoinc) && (coding.Code == LoincCodeDiastolic))) == true);

            resource.Component.Add(new Observation.ComponentComponent()
            {
                Code = new CodeableConcept(UrlCodeSystemLoinc, LoincCodeDiastolic),
                Value = new Quantity(value, BloodPressureUnit)
            });
        }

        /// <summary>
        /// Remove all Diastolic BP values
        /// </summary>
        /// <param name="resource"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void UsCoreBloodPressureDiastolicClear(this Observation resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            resource.Component.RemoveAll(
                component => (component.Code?.Coding?.Any(coding => (coding.System == UrlCodeSystemLoinc) && (coding.Code == LoincCodeDiastolic))) == true);
        }


        /// <summary>
        /// Create a new, valid, UsCore Blood Pressure Object
        /// </summary>
        /// <param name="status"></param>
        /// <param name="subject"></param>
        /// <param name="effective">FhirDateTime or Period</param>
        /// <param name="systolic"></param>
        /// <param name="diastolic"></param>
        /// <returns></returns>
        public static Observation Create(
            ObservationStatus status,
            ResourceReference subject,
            DataType effective,
            decimal systolic,
            decimal diastolic)
        {
            Observation resource = new Observation()
            {
                Status = status,
                Subject = subject,
                Effective = effective,
            };

            resource.UsCoreVitalSignsProfileSet();
            resource.UsCoreVitalSignsCategorySet();

            resource.UsCoreBloodPressureProfileSet();
            resource.UsCoreBloodPressureCodeSet();
            resource.UsCoreBloodPressureSystolicSet(systolic);
            resource.UsCoreBloodPressureDiastolicSet(diastolic);

            return resource;
        }
    }
}
