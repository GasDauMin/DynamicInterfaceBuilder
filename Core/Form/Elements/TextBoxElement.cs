using System.Windows;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Form.Enums;
using DynamicInterfaceBuilder.Core.Form.Helpers;
using DynamicInterfaceBuilder.Core.Form.Models;

namespace DynamicInterfaceBuilder.Core.Form.Elements
{
    [FormElement(true)]
    public class TextBoxElement(App application, string name) : FormElement<string>(application, name, FormElementType.TextBox)
    {
        public override UIElement? BuildElement()
        {
            var panel = new Grid
            {
                Name = $"{Name}_Panel",
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 0, 0, 0)
            };
            
            // Set up columns for label and textbox
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        
            bool isLabelVisible = Label != null && Label.Length > 0;
            double spacing = isLabelVisible ? App.Spacing : 0;

            if (isLabelVisible)
            {
                var label = new TextBlock
                {
                    Name = $"{Name}_Label",
                    Text = Label + ": ",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(0, 0, 0, 0),
                    TextAlignment = TextAlignment.Left,
                    TextWrapping = TextWrapping.NoWrap,
                    TextTrimming = TextTrimming.CharacterEllipsis
                };

                Grid.SetColumn(label, 0);
                panel.Children.Add(label);

                SetupLabelControl(label);
            }
            
            var textBox = new TextBox
            {
                Name = $"{Name}_TextBox",
                Text = DefaultValue,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(spacing, 0, 0, 0),
            };

            Grid.SetColumn(textBox, 1);
            panel.Children.Add(textBox);
            
            SetupControls(textBox, panel, null);

            return panel;
        }

        public override bool ValidateRule(FormElementValidationRule rule)
        {
            return ValidationHelper.ValidateText(rule, ControlValue!);
        }
    }
}