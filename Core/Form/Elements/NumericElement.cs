using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Form.Enums;

namespace DynamicInterfaceBuilder.Core.Form.Elements
{
    [FormElement]
    public class NumericElement(App application, string name) : FormElement<int>(application, name, FormElementType.Numeric)
    {
        public override UIElement? BuildElement()
        {
            var panel = new Grid
            {
                Name = $"{Name}_Panel",
                VerticalAlignment = VerticalAlignment.Top
            };

            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            bool isLabelVisible = Label != null && Label.Length > 0;
            double spacing = isLabelVisible ? StyleProperties.Spacing : 0;

            if (isLabelVisible)
            {
                var label = new TextBlock
                {
                    Name = $"{Name}_Label",
                    Text = Label,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                Grid.SetColumn(label, 0);
                panel.Children.Add(label);

                SetupLabelControl(label);
            }

            // WPF doesn't have NumericUpDown, creating a proper numeric textbox
            var textBox = new TextBox
            {
                Name = $"{Name}_Numeric",
                Text = DefaultValue.ToString(),
                Margin = new Thickness(spacing, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            // Add validation logic for numeric only
            textBox.PreviewTextInput += (s, e) =>
            {
                e.Handled = !int.TryParse(e.Text, out _);
            };

            // Prevent copying non-numeric text into the textbox
            textBox.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Space)
                    e.Handled = true;
            };

            // Prevent paste of non-numeric content
            DataObject.AddPastingHandler(textBox, (s, e) =>
            {
                if (e.DataObject.GetDataPresent(typeof(string)))
                {
                    string text = (string)e.DataObject.GetData(typeof(string));
                    if (!int.TryParse(text, out _))
                    {
                        e.CancelCommand();
                    }
                }
                else
                {
                    e.CancelCommand();
                }
            });

            Grid.SetColumn(textBox, 1);
            panel.Children.Add(textBox);

            SetupControls(textBox, panel, null);

            return panel;
        }
    }
}