using System.Windows;
using System.Windows.Controls;

namespace DynamicInterfaceBuilder
{
    public abstract class FormElementBase : IFormElement
    {
        protected readonly FormBuilder FB;
        
        public string Name { get; protected set; }
        public string? Label { get; set; }
        public string? Description { get; set; }
        public UIElement? Control { get; protected set; }
        public FormElementType Type { get; protected set; }
        public List<ValidationRule> ValidationRules { get; protected set; } = [];

        protected FormElementBase(FormBuilder formBuilder, string name, FormElementType type)
        {
            FB = formBuilder;
            Name = name;
            Type = type;
        }

        public abstract UIElement? BuildControl();
        public abstract bool ValidateControl();
    }
}
