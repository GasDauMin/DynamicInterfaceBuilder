namespace DynamicInterfaceBuilder
{
    public interface IFormElement
    {
        string Name { get; }
        string? Label { get; set; }
        string? Description { get; set; }
        Control? BuildControl();
    }
}