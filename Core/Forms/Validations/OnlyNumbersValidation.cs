using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Forms.Validations
{
    public class OnlyNumbersValidation(App application, ValidationProperties properties, ValidationType type) : FormValidationBase(application, properties, type)
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string stringValue = value?.ToString() ?? string.Empty;
            
            if (string.IsNullOrEmpty(stringValue))
                return ValidationResult.ValidResult; // Allow empty values, use Required validation for mandatory fields
            
            // Allow decimal numbers (including negative and floating point)
            var regex = new Regex(@"^-?\d*\.?\d+$");
            bool isValid = regex.IsMatch(stringValue);
            
            return isValid ? ValidationResult.ValidResult : new ValidationResult(false, Properties?.Message ?? "Value must be a valid number");
        }
    }
}