using System.Windows;
using System.Windows.Controls;

namespace DynamicInterfaceBuilder
{
    public class TextBoxElement(Application application, string name) : FormElement<string>(application, name, FormElementType.TextBox)
    {
        public override UIElement? BuildControl()
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

                LabelControl = label;
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

            PanelControl = panel;
            //LabelControl = isLabelVisible ? panel.Children[0] : null;
            ValueControl = textBox;
            
            return panel;
        }

        public override string? GetValue()
        {
            if (ValueControl is TextBox textBox)
            {
                Value = textBox.Text ?? string.Empty;
            }

            return base.GetValue();
        }

        public override bool ValidateRule(ValidationRule rule)
        {
            var value = GetValue() ?? string.Empty;
            return App.ValidationHelper.Validate_Text(rule, value);
        }
    }
}