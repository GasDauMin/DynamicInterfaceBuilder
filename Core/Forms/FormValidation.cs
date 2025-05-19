using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Forms
{
    public abstract class FormValidation(App application, ValidationProperties properties, ValidationType type) : FormValidationBase(application, properties, type)
    {
    }
}