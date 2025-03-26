namespace DynamicInterfaceBuilder
{
    public class FormElementValidationRule
    {
        public FormElementValidationType Type { get; set; }
        public object? Value { get; set; }
        public string? Message { get; set; }
    }
}