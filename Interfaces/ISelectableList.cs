namespace DynamicInterfaceBuilder
{
    public interface ISelectableList
    {
        string[]? Value { get; set; }
        int DefaultIndex { get; set; }
        string DefaultValue { get; set; }
    }
}