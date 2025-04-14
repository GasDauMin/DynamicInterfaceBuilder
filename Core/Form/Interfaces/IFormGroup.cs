namespace DynamicInterfaceBuilder.Core.Form.Interfaces
{
    public interface IFormGroup
    {
        IReadOnlyList<FormElementBase> Children { get; }

        void AddChild(FormElementBase child);
    }
}
