namespace DynamicInterfaceBuilder.Core.Enums
{
    public enum ValidationType
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