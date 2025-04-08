using DynamicInterfaceBuilder.Core.Form.Enums;
using DynamicInterfaceBuilder.Core.Form.Interfaces;
using DynamicInterfaceBuilder.Core.Form.Models;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Form
{
    public abstract class FormElementBase : AppBase, IFormElementBase
    {   
        public string Name { get; protected set; }
        public string? Label { get; set; }
        public string? Description { get; set; }
        public string? Tooltip { get; set; }
        public bool Valid { get; set; } = true;

        public List<FormElementValidationRule> ValidationRules { get; protected set; } = [];

        protected FormElementBase(App application, string name, FormElementType type) : base(application)
        {
            Name = name;
            Type = type;
        }

        public bool TrySetProperty(string propertyName, object? value)
        {
            try
            {
                var property = GetType().GetProperty(propertyName);
                if (property != null && property.CanWrite)
                {
                    property.SetValue(this, value);
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting property {propertyName}: {ex.Message}");
            }
            
            return false;
        }

        public abstract object? BuildElement();
        public abstract void SetupElement();
        public abstract void ResetElement();
        public abstract bool ValidateElement();

        public object? PanelControl { get; protected set; }
        public object? LabelControl { get; protected set; }
        public object? ValueControl { get; protected set; }
        public FormElementType Type { get; protected set; }

        public abstract void SetupControls(object? PanelControl, object? LabelControl, object? ValueControl);
        public abstract bool SetupValueControl(object? control);
        public abstract bool SetupPanelControl(object? control);
        public abstract bool SetupLabelControl(object? control);

        public abstract void ResetControls(bool doResetPanelControl, bool doResetLabelControl, bool doResetValueControl);
        public abstract void ResetValueControl();
        public abstract void ResetPanelControl();
        public abstract void ResetLabelControl();
    }
}
