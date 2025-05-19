using DynamicInterfaceBuilder.Core.Forms.Elements;
using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Forms.Validations;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Forms
{
    public static class FormElementFactory
    {
        private static readonly Dictionary<FormElementType, Func<App, string, FormElementType, FormElementBase>> _elementFactories = new()
        {
            { FormElementType.Group, (app, name, type) => new GroupElement(app, name, type) },
            { FormElementType.TextBox, (app, name, type) => new TextBoxElement(app, name, type) },
            { FormElementType.Numeric, (app, name, type) => new NumericElement(app, name, type) },
            { FormElementType.CheckBox, (app, name, type) => new CheckBoxElement(app, name, type) },
            { FormElementType.FileBox, (app, name, type) => new FileBoxElement(app, name, type) },
            { FormElementType.FolderBox, (app, name, type) => new FolderBoxElement(app, name, type) },
            { FormElementType.ListBox, (app, name, type) => new ListBoxElement(app, name, type) },
            { FormElementType.ComboBox, (app, name, type) => new ComboBoxElement(app, name, type) },
            { FormElementType.RadioButton, (app, name, type) => new RadioButtonElement(app, name, type) }
        };

        private static readonly Dictionary<ValidationType, Func<App, ValidationProperties, ValidationType, FormValidationBase>> _validationFactories = new()
        {
            { ValidationType.Required, (app, prop, type) => new RequiredValidation(app, prop, type) }
        };

        public static FormElementBase CreateElement(FormElementType type, string name, App application)
        {
            if (_elementFactories.TryGetValue(type, out var factory))
            {
                return factory(application, name, type);
            }
            throw new ArgumentException($"No factory registered for element type {type}");
        }

        public static FormValidationBase CreateValidation(ValidationType type, ValidationProperties properties, App application)
        {
            if (_validationFactories.TryGetValue(type, out var factory))
            {
                return factory(application, properties, type);
            }
            throw new ArgumentException($"No factory registered for validation type {type}");
        }
    }    
}
