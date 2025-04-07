using DynamicInterfaceBuilder.Core.Form.Enums;
using DynamicInterfaceBuilder.Core.Form.Interfaces;
using DynamicInterfaceBuilder.Core.Form.Models;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Form.Structure
{
    public abstract class FormElementBase : AppBase, IFormElementBase
    {   
        public string Name { get; protected set; }
        public string? Label { get; set; }
        public string? Description { get; set; }
        public object? PanelControl { get; protected set; }
        public object? LabelControl { get; protected set; }
        public object? ValueControl { get; protected set; }
        public FormElementType Type { get; protected set; }
        public List<FormElementValidationRule> ValidationRules { get; protected set; } = [];

        protected FormElementBase(App application, string name, FormElementType type) : base(application)
        {
            Name = name;
            Type = type;
        }

        public abstract object? BuildControl();
        public abstract bool ValidateControl();
    }
}
