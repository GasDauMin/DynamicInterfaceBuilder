using DynamicInterfaceBuilder.Core.Enums;

namespace DynamicInterfaceBuilder.Core.Models
{
    public class ValidationProperties
    {
        public ValidationType Type { get; set; }
        public object? Value { get; set; }
        public string? Message { get; set; }
        public bool? Runtime { get; set; }
    }
}