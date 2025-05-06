using System.Windows;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Form.Enums;
using DynamicInterfaceBuilder.Core.Form.Interfaces;
using DynamicInterfaceBuilder.Core.Form.Models;

namespace DynamicInterfaceBuilder.Core.Form.Elements
{
    [FormElement]
    public class ComboBoxElement(App application, string name) : FormElement<string>(application, name, FormElementType.ComboBox), ISelectableList<string>
    {
        private readonly SelectableList<string> _selectableList = new();

        public string[]? Items { get => _selectableList.Items; set => _selectableList.Items = value; }
        public int DefaultIndex { get => _selectableList.DefaultIndex; set => _selectableList.DefaultIndex = value; }
        public override string? DefaultValue { get => _selectableList.DefaultValue; set => _selectableList.DefaultValue = value; }

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
            double spacing = isLabelVisible ? Constants.Default.Spacing : 0;

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

            var comboBox = new ComboBox
            {
                Name = $"{Name}_ComboBox",
                Margin = new Thickness(spacing, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            if (Items != null)
            {
                foreach (var item in Items)
                {
                    comboBox.Items.Add(item);
                }
                
                var idx = DefaultIndex >= 0 
                    ? DefaultIndex 
                    : Items.Length + DefaultIndex;

                if (idx >= 0 && idx < Items.Length)
                {
                    comboBox.SelectedIndex = idx;
                }
            }

            Grid.SetColumn(comboBox, 1);
            panel.Children.Add(comboBox);

            SetupControls(comboBox, panel, null);
            
            return panel;
        }
    }
}