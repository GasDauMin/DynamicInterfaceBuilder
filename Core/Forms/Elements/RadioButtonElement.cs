using System.Windows;
using System.Windows.Controls;

namespace DynamicInterfaceBuilder
{
    public class RadioButtonElement(Application application, string name) : FormElement<string>(application, name, FormElementType.RadioButton), ISelectableList<string>
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

            var radioPanel = new StackPanel
            {
                Name = $"{Name}_RadioPanel",
                Orientation = Orientation.Vertical
            };

            if (Items != null)
            {
                for (int i = 0; i < Items.Length; i++)
                {
                    var radioButton = new RadioButton
                    {
                        Name = $"{Name}_RadioButton_{i}",
                        Content = Items[i],
                        GroupName = Name,
                        IsChecked = (i == DefaultIndex)
                    };
                    
                    radioPanel.Children.Add(radioButton);
                }
            }

            Grid.SetColumn(radioPanel, 1);
            panel.Children.Add(radioPanel);

            Control = panel;
            return panel;
        }
    }
}