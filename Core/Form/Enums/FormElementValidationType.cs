namespace DynamicInterfaceBuilder.Core.Form.Enums
{
    public enum FormElementValidationType
    {
        None,
        Required,
        Regex,
        Range,
        MinLength,
        MaxLength,
        OnlyNumbers,
        OnlyDigits,
        OnlyLetters,
        OnlySpecified,
        FileExists,
        DirectoryExists
    }
}