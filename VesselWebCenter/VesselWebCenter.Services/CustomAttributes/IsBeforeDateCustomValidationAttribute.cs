using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace VesselWebCenter.Data.Constants.CustomAttributes
{
    public class IsBeforeDateCustomValidationAttribute : ValidationAttribute
    {
        private const string Format_DateTime = "dd/MM/yyyy";
        private readonly DateTime date;

        public IsBeforeDateCustomValidationAttribute(string dateInput)
        {
            this.date = DateTime.ParseExact(dateInput,Format_DateTime,CultureInfo.InvariantCulture);
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if ((DateTime)value >= this.date)
            {
                //this.ErrorMessage = "Date input doesnt comply with the requirements"
                return new ValidationResult(this.ErrorMessage); 
            }
            return ValidationResult.Success;
        }
    }
}
