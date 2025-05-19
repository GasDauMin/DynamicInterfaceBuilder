using System.Windows;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Forms;

namespace DynamicInterfaceBuilder.Core.Forms.Elements
{
    [FormElement]
    public class CheckBoxElement(App application, string name, FormElementType type) : FormElement<bool>(application, name, type)
    {
        public override UIElement? BuildElement()
        {
            var panel = new Grid
            {
                Name = $"{Name}_Panel",
                VerticalAlignment = VerticalAlignment.Top,
            };

            var checkBox = new CheckBox
            {
                Name = $"{Name}_CheckBox",
                Content = Label,
                IsChecked = DefaultValue,
                VerticalAlignment = VerticalAlignment.Top,
            };

            panel.Children.Add(checkBox);

            SetupControls(checkBox, panel, null);

            return panel;
        }
    }
}