using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Form.Enums;
using DynamicInterfaceBuilder.Core.Form.Models;

namespace DynamicInterfaceBuilder.Core.Form.Structure
{   
    public abstract class FormElement<T>(App application, string name, FormElementType type) : FormElementBase(application, name, type)
    {
        public virtual T? DefaultValue { get; set; }
        public virtual T? Value { get; set; }

        public override bool ValidateControl()
        {
            bool ok = true;

            foreach (var rule in ValidationRules)
            {
                if (!ValidateRule(rule))
                {
                    if (rule.Message != null)
                    {
                        App.MessageHelper.AddMessage(rule.Message, MessageType.Error);
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