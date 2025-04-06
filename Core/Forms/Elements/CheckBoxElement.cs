using System.Windows;
using System.Windows.Controls;

namespace DynamicInterfaceBuilder
{
    public class CheckBoxElement(Application application, string name) : FormElement<bool>(application, name, FormElementType.CheckBox)
    {
        public override UIElement? BuildControl()
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

            PanelControl = panel;
            LabelControl = null;
            ValueControl = checkBox;
            
            return checkBox;
        }
    }
}