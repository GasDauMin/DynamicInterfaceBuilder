using System.Windows;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Form.Enums;
using DynamicInterfaceBuilder.Core.Form.Interfaces;
using DynamicInterfaceBuilder.Core.Form.Models;

namespace DynamicInterfaceBuilder.Core.Form.Elements
{
    public class ListBoxElement(App application, string name) : FormElement<string>(application, name, FormElementType.ListBox), ISelectableList<string>
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

            bool isLabelVisible = Label != null && Label.Length > 0;
            double spacing = isLabelVisible ? App.Spacing : 0;

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

                LabelControl = label;
            }

            var listBox = new ListBox
            {
                Name = $"{Name}_ListBox",
                Margin = new Thickness(spacing, 0, 0, 0),
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

            PanelControl = panel;
            ValueControl = listBox;
            
            return panel;
        }
    }
}