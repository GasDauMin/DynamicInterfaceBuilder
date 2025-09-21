using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Forms.Validations
{
    public class RegexValidation(App application, ValidationProperties properties, ValidationType type) : FormValidationBase(application, properties, type)
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (Properties?.Value?.ToString() is not string pattern)
                return new ValidationResult(false, "Invalid regex pattern");

            string stringValue = value?.ToString() ?? string.Empty;
            
            try
            {
                var regex = new Regex(pattern);
                bool isValid = regex.IsMatch(stringValue);
                return isValid ? ValidationResult.ValidResult : new ValidationResult(false, Properties?.Message);
            }
            catch (ArgumentException)
            {
                return new ValidationResult(false, "Invalid regex pattern");
            }
        }
    }
}