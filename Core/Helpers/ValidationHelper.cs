namespace DynamicInterfaceBuilder
{
    public class ValidationHelper  : ApplicationBase
    {
        public ValidationHelper(Application application) : base(application)
        {
        }

        #region Text control

        public bool Validate_Text(ValidationRule rule, String value)
        {
            switch (rule.Type)
            {
                case ValidationType.Required:
                    bool isRequired = rule.Value != null && (bool)rule.Value;
                    if (isRequired && string.IsNullOrEmpty(value))
                    {
                        return false;
                    }
                    break;
                case ValidationType.Regex:
                    string regexValue = rule.Value?.ToString() ?? string.Empty;
                    if (!string.IsNullOrEmpty(regexValue) && !string.IsNullOrEmpty(value) && !System.Text.RegularExpressions.Regex.IsMatch(value, regexValue))
                    {
                        return false;
                    }
                    break;
                case ValidationType.MinLength:
                    int minLength = rule.Value != null ? (int)rule.Value : 0;
                    if (value != null && value.Length < minLength)
                    {
                        return false;
                    }
                    break;
                case ValidationType.MaxLength:
                    int maxLength = rule.Value != null ? (int)rule.Value : 0;
                    if (value != null && value.Length > maxLength)
                    {
                        return false;
                    }
                    break;
                case ValidationType.OnlyNumbers:
                    if (value != null && !System.Text.RegularExpressions.Regex.IsMatch(value, @"^[\d.,]+$"))
                    {
                        return false;
                    }
                    break;
                case ValidationType.OnlyDigits:
                    if (value != null && !System.Text.RegularExpressions.Regex.IsMatch(value, @"^\d+$"))
                    {
                        return false;
                    }
                    break;
                case ValidationType.OnlyLetters:
                    if (value != null && !System.Text.RegularExpressions.Regex.IsMatch(value, @"^[a-zA-Z]+$"))
                    {
                        return false;
                    }
                    break;
                case ValidationType.OnlySpecified:
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
                case ValidationType.FileExists:
                    var FileExistsCriteria = rule.Value != null && (bool)rule.Value;
                    if (!string.IsNullOrEmpty(value) && System.IO.File.Exists(value)!=FileExistsCriteria)
                    {
                        return false;
                    }
                    break;
                case ValidationType.DirectoryExists:
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