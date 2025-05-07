using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Models;
using DynamicInterfaceBuilder.Core.Constants;
using Newtonsoft.Json;
using DynamicInterfaceBuilder.Core.Helpers;
using DynamicInterfaceBuilder.Core.Interfaces;

namespace DynamicInterfaceBuilder.Core.Form
{
    public abstract class FormElementBase : AppBase, IFormElementBase
    {   
        public string Name { get; protected set; }
        public string? Label { get; set; }
        public string? Description { get; set; }
        public string? Tooltip { get; set; }
        
        [JsonIgnore]
        public FormElementBase? Parent { get; protected set; }

        protected FormElementBase(App application, string name, FormElementType type) : base(application)
        {
            Name = name;
            Type = type;

            InheritStyle();
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
        
        public void SetParent(FormElementBase parent)
        {
            Parent = parent;
            InheritStyle();
        }
        
        public void InheritStyle()
        {
        }

        public abstract object? BuildElement();
        public abstract void SetupElement();
        public abstract void ResetElement();

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
