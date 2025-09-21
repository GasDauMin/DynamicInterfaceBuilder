using System.Globalization;
using System.IO;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Forms.Validations
{
    public class DirectoryExistsValidation(App application, ValidationProperties properties, ValidationType type) : FormValidationBase(application, properties, type)
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string directoryPath = value?.ToString() ?? string.Empty;
            
            if (string.IsNullOrEmpty(directoryPath))
                return ValidationResult.ValidResult; // Allow empty values, use Required validation for mandatory fields
            
            try
            {
                bool directoryExists = Directory.Exists(directoryPath);
                return directoryExists ? ValidationResult.ValidResult : new ValidationResult(false, Properties?.Message ?? $"Directory does not exist: {directoryPath}");
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, Properties?.Message ?? $"Invalid directory path: {ex.Message}");
            }
        }
    }
}