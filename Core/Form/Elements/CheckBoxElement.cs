using System.Windows;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Form.Enums;

namespace DynamicInterfaceBuilder.Core.Form.Elements
{
    [FormElement]
    public class CheckBoxElement(App application, string name) : FormElement<bool>(application, name, FormElementType.CheckBox)
    {
        public override UIElement? BuildElement()
        {
            var panel = new Grid
            {
                Name = $"{Name}_Panel",
                VerticalAlignment = VerticalAlignment.Top
            };

            var checkBox = new CheckBox
            {
                Name = $"{Name}_CheckBox",
                Content = Label,
                IsChecked = DefaultValue,
                VerticalAlignment = VerticalAlignment.Top
            };

            panel.Children.Add(checkBox);

            SetupControls(checkBox, panel, null);

            return checkBox;
        }
    }
}