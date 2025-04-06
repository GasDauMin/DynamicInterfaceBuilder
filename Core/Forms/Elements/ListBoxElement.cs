using System.Windows;
using System.Windows.Controls;

namespace DynamicInterfaceBuilder
{
    public class ListBoxElement(Application application, string name) : FormElement<string>(application, name, FormElementType.ListBox), ISelectableList<string>
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

            var listBox = new System.Windows.Controls.ListBox
            {
                Name = $"{Name}_ListBox",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            if (Items != null)
            {
                foreach (var item in Items)
                {
                    listBox.Items.Add(item);
                }
                
                if (DefaultIndex >= 0 && DefaultIndex < listBox.Items.Count)
                {
                    listBox.SelectedIndex = DefaultIndex;
                }
            }

            Grid.SetColumn(listBox, 1);
            panel.Children.Add(listBox);

            Control = panel;
            return panel;
        }
    }
}