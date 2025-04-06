using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DynamicInterfaceBuilder
{
    public class NumericElement(Application application, string name) : FormElement<int>(application, name, FormElementType.Numeric)
    {
        public override UIElement? BuildControl()
        {
            var panel = new Grid
            {
                Name = $"{Name}_Panel",
                VerticalAlignment = VerticalAlignment.Top
            };

            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            if (Label != null)
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
            }

            // WPF doesn't have NumericUpDown, creating a proper numeric textbox
            var numericUpDown = new TextBox
            {
                Name = $"{Name}_Numeric",
                Text = DefaultValue.ToString(),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            // Add validation logic for numeric only
            numericUpDown.PreviewTextInput += (s, e) =>
            {
                e.Handled = !int.TryParse(e.Text, out _);
            };

            // Prevent copying non-numeric text into the textbox
            numericUpDown.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Space)
                    e.Handled = true;
            };

            // Prevent paste of non-numeric content
            DataObject.AddPastingHandler(numericUpDown, (s, e) =>
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

            Grid.SetColumn(numericUpDown, 1);
            panel.Children.Add(numericUpDown);

            Control = panel;
            return panel;
        }
    }
}