using System.Globalization;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Forms.Validations
{
    public class RangeValidation(App application, ValidationProperties properties, ValidationType type) : FormValidationBase(application, properties, type)
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (Properties?.Value?.ToString() is not string rangeValue)
                return new ValidationResult(false, "Invalid range specification");

            // Parse range value - expected format: "min,max" or "min-max"
            string[] parts = rangeValue.Split(new char[] { ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                return new ValidationResult(false, "Range must be specified as 'min,max' or 'min-max'");

            if (!double.TryParse(parts[0].Trim(), out double min) || !double.TryParse(parts[1].Trim(), out double max))
                return new ValidationResult(false, "Range values must be numeric");

            if (!double.TryParse(value?.ToString(), out double numericValue))
                return new ValidationResult(false, "Value must be numeric for range validation");

            bool isValid = numericValue >= min && numericValue <= max;
            return isValid ? ValidationResult.ValidResult : new ValidationResult(false, Properties?.Message ?? $"Value must be between {min} and {max}");
        }
    }
}