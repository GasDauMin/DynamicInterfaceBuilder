using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Interfaces;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Forms
{
    public abstract class FormValidationBase(App application, ValidationProperties properties, ValidationType type) : ValidationRule, IAppBase, IFormValidationBase
    {
        public App App { get; init; } = application;
        public ValidationProperties Properties { get; protected set; } = properties;
        public ValidationType Type { get; protected set; } = type;
    }
}