using System.Windows;
using System.Windows.Controls;

namespace DynamicInterfaceBuilder
{
    public class ComboBoxElement(Application application, string name) : FormElement<string>(application, name, FormElementType.ComboBox), ISelectableList<string>
    {
        private readonly SelectableList<string> _selectableList = new();

        public string[]? Items { get => _selectableList.Items; set => _selectableList.Items = value; }
        public int DefaultIndex { get => _selectableList.DefaultIndex; set => _selectableList.DefaultIndex = value; }
        public override string? DefaultValue { get => _selectableList.DefaultValue; set => _selectableList.DefaultValue = value; }

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

            var comboBox = new ComboBox
            {
                Name = $"{Name}_ComboBox",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            if (Items != null)
            {
                foreach (var item in Items)
                {
                    comboBox.Items.Add(item);
                }
                
                if (DefaultIndex >= 0 && DefaultIndex < comboBox.Items.Count)
                {
                    comboBox.SelectedIndex = DefaultIndex;
                }
            }

            Grid.SetColumn(comboBox, 1);
            panel.Children.Add(comboBox);

            Control = panel;
            return panel;
        }
    }
}