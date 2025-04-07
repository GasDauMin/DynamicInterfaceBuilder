using DynamicInterfaceBuilder.Core.Form.Enums;

namespace DynamicInterfaceBuilder.Core.Form.Models
{
    public class FormElementValidationRule
    {
        public FormElementValidationType Type { get; set; }
        public object? Value { get; set; }
        public string? Message { get; set; }
        public bool? Runtime { get; set; }
    }
}