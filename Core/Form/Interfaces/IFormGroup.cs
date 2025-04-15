using DynamicInterfaceBuilder.Core.Form;

namespace DynamicInterfaceBuilder.Core.Form.Interfaces
{
    public interface IFormGroup
    {
        string Name { get; }
        Dictionary<string, FormElementBase> Elements { get; }
        IReadOnlyList<FormElementBase> Children { get; }
        void AddChild(FormElementBase child);
    }
}
