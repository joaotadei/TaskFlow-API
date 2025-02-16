using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace API.Extensions
{
    public class EmailAtribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string email = Convert.ToString(value);

            if (!EmailIsValid(email))
                return new ValidationResult("Email is invalid");

            return ValidationResult.Success;

        }
        public static bool EmailIsValid(string email)
        {
            Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

            if (rg.IsMatch(email))
                return true;

            else
                return false;
        }
    }
}