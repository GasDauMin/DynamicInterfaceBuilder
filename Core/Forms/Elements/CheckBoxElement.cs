using System.Windows;
using System.Windows.Controls;

namespace DynamicInterfaceBuilder
{
    public class CheckBoxElement(Application application, string name) : FormElement<bool>(application, name, FormElementType.CheckBox)
    {
        public override UIElement? BuildControl()
        {
            var checkBox = new CheckBox
            {
                Name = $"{Name}_CheckBox",
                Content = Label,
                IsChecked = DefaultValue,
                VerticalAlignment = VerticalAlignment.Top
            };

            Control = checkBox;
            return checkBox;
        }
    }
}