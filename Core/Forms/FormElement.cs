using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace DynamicInterfaceBuilder
{   
    public abstract class FormElement<T>(Application application, string name, FormElementType type) : FormElementBase(application, name, type)
    {
        public virtual T? DefaultValue { get; set; }
        public virtual T? Value { get; set; }

        public override bool ValidateControl()
        {
            bool ok = true;

            RecolorLabelControl("Foreground", ColorType.Foreground);
            foreach (var rule in ValidationRules)
            {
                if (!ValidateRule(rule))
                {
                    if (rule.Message != null)
                    {
                        App.MessageManager.AddMessage(rule.Message, MessageType.Error);
                        RecolorLabelControl("Higlight", ColorType.Foreground);
                    }
                    
                    ok = false;   
                }
            }

            return ok;
        }

        public virtual T? GetValue()
        {
            return Value != null ? (T)Value : default;
        }
        
        public virtual bool ValidateRule(ValidationRule rule)
        {
            return true;
        }

        public override void RecolorLabelControl(string color = "", ColorType type = ColorType.Background)
        {
            if (App.AdvancedProperties.FormType == FormBaseType.WPF)
            {
                App.WpfHelper.RecolorObject(LabelControl, color, type);
            }
        }

        public override void RecolorValueControl(string color = "", ColorType type = ColorType.Background)
        {
            if (App.AdvancedProperties.FormType == FormBaseType.WPF)
            {
                App.WpfHelper.RecolorObject(ValueControl, color, type);
            }
        }

        public override void RecolorPanelControl(string color = "", ColorType type = ColorType.Background)
        {
            if (App.AdvancedProperties.FormType == FormBaseType.WPF)
            {
                App.WpfHelper.RecolorObject(PanelControl, color, type);
            }
        }
    }
}