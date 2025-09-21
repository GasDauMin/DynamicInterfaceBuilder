using System.Globalization;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Forms.Validations
{
    public class MinLengthValidation(App application, ValidationProperties properties, ValidationType type) : FormValidationBase(application, properties, type)
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!int.TryParse(Properties?.Value?.ToString(), out int minLength))
                return new ValidationResult(false, "Invalid minimum length specification");

            string stringValue = value?.ToString() ?? string.Empty;
            bool isValid = stringValue.Length >= minLength;
            
            return isValid ? ValidationResult.ValidResult : new ValidationResult(false, Properties?.Message ?? $"Minimum length is {minLength} characters");
        }
    }
}