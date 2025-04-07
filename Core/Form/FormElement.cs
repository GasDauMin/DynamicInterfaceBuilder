using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Form.Enums;
using DynamicInterfaceBuilder.Core.Form.Models;
using DynamicInterfaceBuilder.Core.Managers;

namespace DynamicInterfaceBuilder.Core.Form
{   
    public abstract class FormElement<T>(App application, string name, FormElementType type) : FormElementBase(application, name, type)
    {
        public virtual T? DefaultValue { get; set; }
        public virtual T? Value { get; set; }

        public override bool ValidateControl()
        {
            bool ok = true;

            var valueControl = ValueControl as TextBox;
            if (valueControl != null)
            {
                valueControl.ClearValue(Control.BackgroundProperty);
                valueControl.ClearValue(Control.BorderBrushProperty);
            }

            foreach (var rule in ValidationRules)
            {
                if (!ValidateRule(rule))
                {
                    if (rule.Message != null)
                    {
                        App.MessageHelper.AddMessage(rule.Message, MessageType.Error);

                        if (valueControl != null)
                        {
                            valueControl.Background = ThemeManager.GetBrush("ABrush.AlertTone2");
                            valueControl.BorderBrush = ThemeManager.GetBrush("ABrush.AlertTone3");
                        }
                    }
                    
                    ok = false;   
                }
            }

            return ok;
        }

        public virtual T? GetValue()
        {
            return Value != null ? Value : default;
        }
        
        public virtual bool ValidateRule(FormElementValidationRule rule)
        {
            return true;
        }
    }
}