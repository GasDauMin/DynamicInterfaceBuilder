using DynamicInterfaceBuilder.Core.Form.Elements;
using DynamicInterfaceBuilder.Core.Form.Enums;

namespace DynamicInterfaceBuilder.Core.Form
{
    public static class FormElementFactory
    {
        private static readonly Dictionary<FormElementType, Func<App, string, FormElementBase>> _factories = new()
        {
            { FormElementType.Group, (fb, name) => new GroupElement(fb, name) },
            { FormElementType.TextBox, (fb, name) => new TextBoxElement(fb, name) },
            { FormElementType.Numeric, (fb, name) => new NumericElement(fb, name) },
            { FormElementType.CheckBox, (fb, name) => new CheckBoxElement(fb, name) },
            { FormElementType.FileBox, (fb, name) => new FileBoxElement(fb, name) },
            { FormElementType.FolderBox, (fb, name) => new FolderBoxElement(fb, name) },
            { FormElementType.ListBox, (fb, name) => new ListBoxElement(fb, name) },
            { FormElementType.ComboBox, (fb, name) => new ComboBoxElement(fb, name) },
            { FormElementType.RadioButton, (fb, name) => new RadioButtonElement(fb, name) }
        };

        public static FormElementBase Create(FormElementType type, string name, App application)
        {
            if (_factories.TryGetValue(type, out var factory))
            {
                return factory(application, name);
            }
            throw new ArgumentException($"No factory registered for type {type}");
        }
    }    
}
