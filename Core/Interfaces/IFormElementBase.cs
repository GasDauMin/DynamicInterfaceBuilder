using DynamicInterfaceBuilder.Core.Enums;

namespace DynamicInterfaceBuilder.Core.Interfaces
{
    public interface IFormElementBase
    {
        string Name { get; }
        FormElementType Type { get; }
        string? Label { get; set; }
        string? Description { get; set; }
    }
}