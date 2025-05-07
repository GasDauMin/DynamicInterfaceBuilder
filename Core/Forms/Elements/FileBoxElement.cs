using System.Windows;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Forms;
using DynamicInterfaceBuilder.Core.Helpers;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Forms.Elements
{
    [FormElement]
    public class FileBoxElement(App application, string name) : FormElement<string>(application, name, FormElementType.FileBox)
    {
        public override UIElement? BuildElement()
        {
            // Implement file box control using Grid with TextBox and Button
            var panel = new Grid
            {
                Name = $"{Name}_Panel",
                VerticalAlignment = VerticalAlignment.Top
            };

            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
;
            bool isLabelVisible = Label != null && Label.Length > 0;
            double spacing = isLabelVisible ? Constants.Default.Spacing : 0;

            if (isLabelVisible)
            {
                var label = new TextBlock
                {
                    Name = $"{Name}_Label",
                    Text = Label + ": ",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                Grid.SetColumn(label, 0);
                panel.Children.Add(label);

                SetupLabelControl(label);
            }

            var textBox = new TextBox
            {
                Name = $"{Name}_FilePath",
                Text = DefaultValue,
                Margin = new Thickness(spacing, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            Grid.SetColumn(textBox, 1);
            panel.Children.Add(textBox);

            var browseButton = new Button
            {
                Name = $"{Name}_BrowseButton",
                Content = "...",
                Width = 30,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            browseButton.Click += (s, e) =>
            {
                var dialog = new Microsoft.Win32.OpenFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    textBox.Text = dialog.FileName;
                }
            };

            Grid.SetColumn(browseButton, 2);
            panel.Children.Add(browseButton);

            SetupControls(textBox, panel, null);
            
            return panel;
        }
    }
}