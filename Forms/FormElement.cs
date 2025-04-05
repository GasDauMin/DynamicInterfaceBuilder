using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace DynamicInterfaceBuilder
{   
    #region FormElement
    public abstract class FormElement<T>(FormBuilder formBuilder, string name, FormElementType type) : FormElementBase(formBuilder, name, type)
    {
        public virtual T? DefaultValue { get; set; }
        public virtual T? Value { get; set; }

        public override bool ValidateControl()
        {
            bool ok = true;

            foreach (var rule in ValidationRules)
            {
                FB.ClearMessages();

                if (!ValidateRule(rule))
                {
                    if (rule.Message != null)
                    {
                        FB.PrintError(rule.Message);
                    }
                    
                    ok = false;   
                }
            }

            return ok;
        }

        public virtual bool ValidateRule(ValidationRule rule)
        {
            return false;
        }
    }
    #endregion

    #region TextBoxElement
    public class TextBoxElement(FormBuilder formBuilder, string name) : FormElement<string>(formBuilder, name, FormElementType.TextBox)
    {
        public override UIElement? BuildControl()
        {
            var panel = new Grid
            {
                Name = $"{Name}_Panel",
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 0, 0, 0)
            };
            
            // Set up columns for label and textbox
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        
            bool isLabelVisible = Label != null && Label.Length > 0;
            double spacing = isLabelVisible ? FB.Spacing : 0;

            if (isLabelVisible)
            {
                var label = new TextBlock
                {
                    Name = $"{Name}_Label",
                    Text = Label + ": ",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(0, 0, 0, 0),
                    TextAlignment = TextAlignment.Left,
                    TextWrapping = TextWrapping.NoWrap,
                    TextTrimming = TextTrimming.CharacterEllipsis
                };

                Grid.SetColumn(label, 0);
                panel.Children.Add(label);
            }
            
            var textBox = new TextBox
            {
                Name = $"{Name}_TextBox",
                Text = DefaultValue,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(spacing, 0, 0, 0),
                Background = new SolidColorBrush(FB.GetThemeColor("ControlBack")),
                Foreground = new SolidColorBrush(FB.GetThemeColor("ControlFore")),
                BorderThickness = new Thickness(0)
            };

            Grid.SetColumn(textBox, 1);
            panel.Children.Add(textBox);

            Control = panel;
            return panel;
        }
    }
    #endregion

    #region NumericElement
    public class NumericElement(FormBuilder formBuilder, string name) : FormElement<int>(formBuilder, name, FormElementType.Numeric)
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
    #endregion

    #region CheckBoxElement
    public class CheckBoxElement(FormBuilder formBuilder, string name) : FormElement<bool>(formBuilder, name, FormElementType.CheckBox)
    {
        public override UIElement? BuildControl()
        {
            var checkBox = new CheckBox
            {
                Name = $"{Name}_CheckBox",
                Content = Label,
                IsChecked = DefaultValue,
                VerticalAlignment = VerticalAlignment.Top
            };

            Control = checkBox;
            return checkBox;
        }
    }
    #endregion

    #region FileBoxElement
    public class FileBoxElement(FormBuilder formBuilder, string name) : FormElement<string>(formBuilder, name, FormElementType.FileBox)
    {
        public override UIElement? BuildControl()
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

            var textBox = new TextBox
            {
                Name = $"{Name}_FilePath",
                Text = DefaultValue,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch
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

            Control = panel;
            return panel;
        }
    }
    #endregion

    #region FolderBoxElement
    public class FolderBoxElement(FormBuilder formBuilder, string name) : FormElement<string>(formBuilder, name, FormElementType.FolderBox)
    {
        public override UIElement? BuildControl()
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

            var textBox = new TextBox
            {
                Name = $"{Name}_FolderPath",
                Text = DefaultValue,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch
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
                string? selectedFolder = WinAPI.BrowseForFolder("Select Folder", DefaultValue);
                if (!string.IsNullOrEmpty(selectedFolder))
                {
                    textBox.Text = selectedFolder;
                }
            };

            Grid.SetColumn(browseButton, 2);
            panel.Children.Add(browseButton);

            Control = panel;
            return panel;
        }
    }
    #endregion

    #region ListBoxElement
    public class ListBoxElement(FormBuilder formBuilder, string name) : FormElement<string>(formBuilder, name, FormElementType.ListBox), ISelectableList<string>
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
    #endregion

    #region ComboBoxElement
    public class ComboBoxElement(FormBuilder formBuilder, string name) : FormElement<string>(formBuilder, name, FormElementType.ComboBox), ISelectableList<string>
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
    #endregion

    #region RadioButtonElement
    public class RadioButtonElement(FormBuilder formBuilder, string name) : FormElement<string>(formBuilder, name, FormElementType.RadioButton), ISelectableList<string>
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
    #endregion
}