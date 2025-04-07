namespace DynamicInterfaceBuilder.Core.Form.Interfaces
{
    public interface IFormElementBase
    {
        string Name { get; }
        string? Label { get; set; }
        string? Description { get; set; }
    }
}