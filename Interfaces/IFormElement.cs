using System.Windows;

namespace DynamicInterfaceBuilder
{
    public interface IFormElement
    {
        string Name { get; }
        string? Label { get; set; }
        string? Description { get; set; }
        UIElement? BuildControl();
    }
}