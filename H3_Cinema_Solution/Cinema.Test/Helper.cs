using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Test
{
    internal class Helper
    {
        /// <summary>
        /// Checks if there is attributes that are not met.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A list of validations that weren't met.</returns>
        public static IList<ValidationResult> Validate(object model)
        {
            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, results, true);
            if (model is IValidatableObject) (model as IValidatableObject).Validate(validationContext);
            return results;
        }

        /// <summary>
        /// Checks if there was any validation errors on specific property.
        /// </summary>
        /// <param name="model">Model to check for errors</param>
        /// <param name="Name">Property to check for</param>
        /// <returns>true if there was an error</returns>
        public static bool HasError(object model, string Name)
        {
            // Returns true if there was an error
            var errorList = Helper.Validate(model);
            return errorList.Any(x => x.MemberNames.Contains(Name));
        }
    }
}
