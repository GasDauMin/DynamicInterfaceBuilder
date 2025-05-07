using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Constants;

namespace DynamicInterfaceBuilder.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FormElementAttribute : Attribute
    {       
        public FormElementAttribute()
        {            
        }
    }
}