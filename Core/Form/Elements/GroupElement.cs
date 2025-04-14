using System.Windows;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Form.Enums;
using DynamicInterfaceBuilder.Core.Form.Interfaces;
using DynamicInterfaceBuilder.Core.Form.Models;

namespace DynamicInterfaceBuilder.Core.Form.Elements
{
    [FormElement]
    public class GroupElement(App application, string name) : FormElementBase(application, name, FormElementType.Group), IFormGroup
    {
        public List<FormElementBase> Elements { get; set; } = new();
        public IReadOnlyList<FormElementBase> Children => Elements;
        public Orientation Orientation { get; set; } = Orientation.Horizontal;
        public int Spacing { get; set; }
        public bool ShowBorder { get; set; }
        public Thickness Margin { get; set; }
        public Thickness Padding { get; set; }
        
        public override UIElement? BuildElement()
        {
            // Create the main container
            var panel = new Grid
            {
                Name = $"{Name}_Panel",
                VerticalAlignment = VerticalAlignment.Top,
                Margin = Margin
            };
            
            // Create container for elements
            var elementsPanel = new StackPanel
            {
                Name = $"{Name}_Elements",
                Orientation = Orientation,
                Margin = Padding
            };
            
            // Add elements to the panel with proper spacing
            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i].BuildElement() is UIElement control)
                {
                    // Apply spacing to all elements except the last one
                    if (i < Elements.Count - 1 && Spacing > 0 && control is FrameworkElement frameworkElement)
                    {
                        if (Orientation == Orientation.Vertical)
                            frameworkElement.Margin = new Thickness(0, 0, 0, Spacing);
                        else
                            frameworkElement.Margin = new Thickness(0, 0, Spacing, 0);
                    }
                    elementsPanel.Children.Add(control);
                }
            }
            
            // Determine if we need a border
            if (ShowBorder)
            {
                var border = new Border
                {
                    Name = $"{Name}_Border",
                    BorderThickness = new Thickness(1),
                    BorderBrush = SystemColors.ControlDarkBrush,
                    Padding = new Thickness(5),
                    Child = elementsPanel
                };
                
                // Add group header if label is provided
                if (!string.IsNullOrEmpty(Label))
                {
                    var headerPanel = new Grid();
                    var header = new TextBlock
                    {
                        Name = $"{Name}_Header",
                        Text = Label,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(5, 0, 5, 5)
                    };
                    headerPanel.Children.Add(header);
                    panel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    panel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    Grid.SetRow(headerPanel, 0);
                    Grid.SetRow(border, 1);
                    panel.Children.Add(headerPanel);
                    panel.Children.Add(border);
                }
                else
                {
                    panel.Children.Add(border);
                }
            }
            else
            {
                // Add group header if label is provided
                if (!string.IsNullOrEmpty(Label))
                {
                    var header = new TextBlock
                    {
                        Name = $"{Name}_Header",
                        Text = Label,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 0, 5)
                    };
                    panel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    panel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    Grid.SetRow(header, 0);
                    Grid.SetRow(elementsPanel, 1);
                    panel.Children.Add(header);
                    panel.Children.Add(elementsPanel);
                }
                else
                {
                    panel.Children.Add(elementsPanel);
                }
            }
            
            SetupPanelControl(panel);
            return panel;
        }
        
        public override bool ValidateElement()
        {
            bool isValid = true;
            foreach (var element in Elements)
            {
                if (!element.ValidateElement())
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        public override void SetupElement()
        {
            Valid = true;
            Tooltip = Description;
        }

        public override void ResetElement()
        {
            Valid = true;
            Tooltip = Description;
        }

        public override void SetupControls(object? panelControl, object? labelControl, object? valueControl)
        {
            if (panelControl != null)
            {
                SetupPanelControl(panelControl);
            }
        }

        public override bool SetupValueControl(object? control)
        {
            return false; // Groups don't have value controls
        }

        public override bool SetupPanelControl(object? control)
        {
            if (control != null)
            {
                PanelControl = control;
                return true;
            }
            return false;
        }

        public override bool SetupLabelControl(object? control)
        {
            return false; // Groups don't have label controls
        }

        public override void ResetControls(bool doResetValueControl, bool doResetPanelControl, bool doResetLabelControl)
        {
            if (doResetPanelControl && PanelControl != null)
            {
                ResetPanelControl();
            }
        }

        public override void ResetValueControl()
        {
            // Groups don't have value controls
        }

        public override void ResetPanelControl()
        {
            if (PanelControl is Control control)
            {
                control.ClearValue(Control.BackgroundProperty);
                control.ClearValue(Control.ForegroundProperty);
                control.ClearValue(Control.BorderBrushProperty);
            }
        }
        public override void ResetLabelControl()
        {
            // Groups don't have label controls
        }

        public void AddChild(FormElementBase element)
        {
            Elements.Add(element);
        }
    }
}
