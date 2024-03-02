using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using Hl7.Fhir.Model;

namespace Fhir_Profile.Validation
{
    /// <summary>
    /// Class to hold validator extensions
    /// </summary>
    public static class ValidatorExtensions
    {
        /// <summary>
        /// Validator to see if a list codeable concepts contains a value with a specific system and code
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="system"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, IList<TElement>> ConceptListConatains<T, TElement>(
            this IRuleBuilder<T, IList<TElement>> ruleBuilder,
            string system,
            string code)
            where TElement : CodeableConcept
        {

            return ruleBuilder.SetValidator(new ConceptListContainsValidator<T, TElement>(system, code));
        }

        /// <summary>
        ///  Validator to see if a codeable concepts contains a value with a specific system and code
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="system"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TElement> ConceptConatains<T, TElement>(
           this IRuleBuilder<T, TElement> ruleBuilder,
           string system,
           string code)
           where TElement : CodeableConcept
        {

            return ruleBuilder.SetValidator(new ConceptContainsValidator<T, TElement>(system, code));
        }
    }
}
