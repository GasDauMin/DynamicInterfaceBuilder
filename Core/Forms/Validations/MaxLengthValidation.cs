using System.Globalization;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Forms.Validations
{
    public class MaxLengthValidation(App application, ValidationProperties properties, ValidationType type) : FormValidationBase(application, properties, type)
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!int.TryParse(Properties?.Value?.ToString(), out int maxLength))
                return new ValidationResult(false, "Invalid maximum length specification");

            string stringValue = value?.ToString() ?? string.Empty;
            bool isValid = stringValue.Length <= maxLength;
            
            return isValid ? ValidationResult.ValidResult : new ValidationResult(false, Properties?.Message ?? $"Maximum length is {maxLength} characters");
        }
    }
}