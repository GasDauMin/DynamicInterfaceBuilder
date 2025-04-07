using DynamicInterfaceBuilder.Core.Form.Enums;
using DynamicInterfaceBuilder.Core.Form.Models;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Form.Helpers
{
    public class ValidationHelper : AppBase {
        public ValidationHelper(App application) : base(application)
        {
        }

        #region Text control

        static public bool ValidateText(FormElementValidationRule rule, string value)
        {
            switch (rule.Type)
            {
                case FormElementValidationType.Required:
                    bool isRequired = rule.Value != null && (bool)rule.Value;
                    if (isRequired && string.IsNullOrEmpty(value))
                    {
                        return false;
                    }
                    break;
                case FormElementValidationType.Regex:
                    string regexValue = rule.Value?.ToString() ?? string.Empty;
                    if (!string.IsNullOrEmpty(regexValue) && !string.IsNullOrEmpty(value) && !System.Text.RegularExpressions.Regex.IsMatch(value, regexValue))
                    {
                        return false;
                    }
                    break;
                case FormElementValidationType.MinLength:
                    int minLength = rule.Value != null ? (int)rule.Value : 0;
                    if (value != null && value.Length < minLength)
                    {
                        return false;
                    }
                    break;
                case FormElementValidationType.MaxLength:
                    int maxLength = rule.Value != null ? (int)rule.Value : 0;
                    if (value != null && value.Length > maxLength)
                    {
                        return false;
                    }
                    break;
                case FormElementValidationType.OnlyNumbers:
                    if (value != null && !System.Text.RegularExpressions.Regex.IsMatch(value, @"^[\d.,]+$"))
                    {
                        return false;
                    }
                    break;
                case FormElementValidationType.OnlyDigits:
                    if (value != null && !System.Text.RegularExpressions.Regex.IsMatch(value, @"^\d+$"))
                    {
                        return false;
                    }
                    break;
                case FormElementValidationType.OnlyLetters:
                    if (value != null && !System.Text.RegularExpressions.Regex.IsMatch(value, @"^[a-zA-Z]+$"))
                    {
                        return false;
                    }
                    break;
                case FormElementValidationType.OnlySpecified:
                    string specifiedChars = rule.Value?.ToString() ?? string.Empty;
                    if (!string.IsNullOrEmpty(specifiedChars) && !string.IsNullOrEmpty(value))
                    {
                        foreach (char c in value)
                        {
                            if (specifiedChars.IndexOf(c) < 0)
                            {
                                return false;
                            }
                        }
                    }
                    break;
                case FormElementValidationType.FileExists:
                    var FileExistsCriteria = rule.Value != null && (bool)rule.Value;
                    if (!string.IsNullOrEmpty(value) && System.IO.File.Exists(value)!=FileExistsCriteria)
                    {
                        return false;
                    }
                    break;
                case FormElementValidationType.DirectoryExists:
                    var DirectoryExistsCriteria = rule.Value != null && (bool)rule.Value;
                    if (!string.IsNullOrEmpty(value) && !System.IO.Directory.Exists(value)!=DirectoryExistsCriteria)
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }

        #endregion
    }
}