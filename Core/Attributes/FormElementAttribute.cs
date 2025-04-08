using DynamicInterfaceBuilder.Core.Form.Enums;
using DynamicInterfaceBuilder.Core.Constants;

namespace DynamicInterfaceBuilder.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FormElementAttribute : Attribute
    {       
        public bool AllowAlertControl { get; }
        public bool AllowValidationControl { get; }

        public FormElementAttribute(
            bool allowAlertControl = Default.AllowAlertControl, 
            bool allowValidationControl = Default.AllowValidationControl)
        {            
            AllowAlertControl = allowAlertControl;
            AllowValidationControl = allowValidationControl;
        }
    }
}