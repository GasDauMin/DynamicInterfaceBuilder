using System.Windows;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Form.Enums;
using DynamicInterfaceBuilder.Core.Form.Interfaces;
using DynamicInterfaceBuilder.Core.Form.Models;

namespace DynamicInterfaceBuilder.Core.Form.Elements
{
    [FormElement]
    public class RadioButtonElement(App application, string name) : FormElement<string>(application, name, FormElementType.RadioButton), ISelectableList<string>
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

            var radioPanel = new StackPanel
            {
                Name = $"{Name}_RadioPanel",
                Margin = new Thickness(spacing, 0, 0, 0),
                Orientation = Orientation.Vertical
            };

            if (Items != null)
            {
                var idx = DefaultIndex >= 0 
                    ? DefaultIndex 
                    : Items.Length + DefaultIndex;

                for (int i = 0; i < Items.Length; i++)
                {
                    var isChecked =  (idx >= 0 && idx < Items.Length && i == idx);
                    var radioButton = new RadioButton
                    {
                        Name = $"{Name}_RadioButton_{i}",
                        Content = Items[i],
                        GroupName = Name,
                        IsChecked = isChecked,
                        Margin = new Thickness(2),
                    };
                    
                    radioPanel.Children.Add(radioButton);
                }
            }

            Grid.SetColumn(radioPanel, 1);
            panel.Children.Add(radioPanel);

            SetupControls(radioPanel, panel, null);
            
            return panel;
        }
    }
}