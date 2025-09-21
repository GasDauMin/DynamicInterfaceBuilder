using System.Globalization;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Forms.Validations
{
    public class OnlySpecifiedValidation(App application, ValidationProperties properties, ValidationType type) : FormValidationBase(application, properties, type)
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (Properties?.Value?.ToString() is not string allowedValues)
                return new ValidationResult(false, "No allowed values specified");

            string stringValue = value?.ToString() ?? string.Empty;
            
            if (string.IsNullOrEmpty(stringValue))
                return ValidationResult.ValidResult; // Allow empty values, use Required validation for mandatory fields
            
            // Parse allowed values - expected format: "value1,value2,value3" or "value1;value2;value3"
            string[] allowedArray = allowedValues.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                                 .Select(v => v.Trim())
                                                 .ToArray();
            
            bool isValid = allowedArray.Contains(stringValue, StringComparer.OrdinalIgnoreCase);
            
            return isValid ? ValidationResult.ValidResult : new ValidationResult(false, Properties?.Message ?? $"Value must be one of: {string.Join(", ", allowedArray)}");
        }
    }
}