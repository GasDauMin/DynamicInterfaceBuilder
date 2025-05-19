using System.IO;
using System.Windows;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Forms;
using Microsoft.Win32;

namespace DynamicInterfaceBuilder.Core.Forms.Elements
{
    [FormElement]
    public class FolderBoxElement(App application, string name, FormElementType type) : FormElement<string>(application, name, type)
    {
        public override UIElement? BuildElement()
        {
            // Implement folder box control similar to FileBoxElement
            var panel = new Grid
            {
                Name = $"{Name}_Panel",
                VerticalAlignment = VerticalAlignment.Top
            };

            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
           
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
                Name = $"{Name}_FolderPath",
                Text = DefaultValue,
                Margin = new Thickness(spacing, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                BorderThickness = new Thickness(0),
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
                // Use our WPF-only folder browser implementation
                string? selectedFolder = BrowseForFolder("Select Folder", DefaultValue);
                if (!string.IsNullOrEmpty(selectedFolder))
                {
                    textBox.Text = selectedFolder;
                }
            };

            Grid.SetColumn(browseButton, 2);
            panel.Children.Add(browseButton);

            SetupControls(textBox, panel, null);

            return panel;
        }

        public string? BrowseForFolder(string title = "Select Folder", string? initialDirectory = null)
        {
            // WPF doesn't have a built-in folder browser, so we use OpenFileDialog with tricks
            var dialog = new OpenFileDialog
            {
                CheckFileExists = false,
                FileName = "Folder Selection",
                Title = title,
                ValidateNames = false,
                CheckPathExists = true
            };

            if (!string.IsNullOrEmpty(initialDirectory) && Directory.Exists(initialDirectory))
            {
                dialog.InitialDirectory = initialDirectory;
            }

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string? folderPath = Path.GetDirectoryName(dialog.FileName);
                return folderPath;
            }

            return null;
        }
    }
}