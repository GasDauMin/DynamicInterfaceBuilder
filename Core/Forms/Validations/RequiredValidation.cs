using System.Globalization;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Forms.Validations
{
    public class RequiredValidation(App application, ValidationProperties properties, ValidationType type) : FormValidationBase(application, properties, type)
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            // Handle different data types for required validation
            bool isValid = value switch
            {
                null => false,
                string stringValue => !string.IsNullOrEmpty(stringValue) && !string.IsNullOrWhiteSpace(stringValue),
                bool boolValue => boolValue, // For checkboxes, require them to be checked
                int intValue => true, // Numbers are always valid if not null
                double doubleValue => true, // Numbers are always valid if not null
                decimal decimalValue => true, // Numbers are always valid if not null
                DateTime dateValue => dateValue != default, // Dates are valid if not default
                _ => value.ToString() is string str && !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str)
            };

            return isValid ? ValidationResult.ValidResult : new ValidationResult(false, Properties?.Message ?? "This field is required");
        }
    }
}