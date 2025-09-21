using System.Globalization;
using System.IO;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Forms.Validations
{
    public class FileExistsValidation(App application, ValidationProperties properties, ValidationType type) : FormValidationBase(application, properties, type)
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string filePath = value?.ToString() ?? string.Empty;
            
            if (string.IsNullOrEmpty(filePath))
                return ValidationResult.ValidResult; // Allow empty values, use Required validation for mandatory fields
            
            try
            {
                bool fileExists = File.Exists(filePath);
                return fileExists ? ValidationResult.ValidResult : new ValidationResult(false, Properties?.Message ?? $"File does not exist: {filePath}");
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, Properties?.Message ?? $"Invalid file path: {ex.Message}");
            }
        }
    }
}