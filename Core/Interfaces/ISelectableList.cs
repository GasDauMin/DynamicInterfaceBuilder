namespace DynamicInterfaceBuilder.Core.Interfaces
{
    public interface ISelectableList<T>
    {
        T[]? Items { get; set; }
        int DefaultIndex { get; set; }
        T? DefaultValue { get; set; }
    }
}