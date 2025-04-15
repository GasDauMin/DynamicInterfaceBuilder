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
        public Dictionary<string, FormElementBase> Elements { get; set; } = new();
        public IReadOnlyList<FormElementBase> Children => [.. Elements.Values];
        public Orientation Orientation { get; set; } = Orientation.Vertical;
        public int Spacing { get; set; }
        public bool ShowBorder { get; set; }
        public Thickness Margin { get; set; }
        public Thickness Padding { get; set; }

        public void AddChild(FormElementBase child)
        {
            Elements[child.Name] = child;
        }
        
        public override UIElement? BuildElement()
        {
            var mainPanel = new DockPanel
            {
                Name = $"{Name}_MainPanel",
                VerticalAlignment = VerticalAlignment.Top
            };
            
            // Create container for elements
            var elementsPanel = new StackPanel
            {
                Name = $"{Name}_Elements",
                Orientation = Orientation,
                Margin = new Thickness(Margin.Left + Padding.Left, 
                                     Margin.Top + Padding.Top, 
                                     Margin.Right + Padding.Right, 
                                     Margin.Bottom + Padding.Bottom)
            };

            // Set spacing between elements
            if (Spacing > 0)
            {
                elementsPanel.Margin = new Thickness(
                    elementsPanel.Margin.Left,
                    elementsPanel.Margin.Top,
                    elementsPanel.Margin.Right,
                    elementsPanel.Margin.Bottom + Spacing
                );
            }
            
            // Add group header if label is provided
            if (!string.IsNullOrEmpty(Label))
            {
                var header = new TextBlock
                {
                    Name = $"{Name}_Header",
                    Text = Label,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, Spacing > 0 ? Spacing : 5)
                };
                DockPanel.SetDock(header, Dock.Top);
                mainPanel.Children.Add(header);
            }
            
            // Add elements to the panel
            foreach (var element in Elements.Values)
            {
                if (element.BuildElement() is UIElement control)
                {
                    if (Spacing > 0 && elementsPanel.Children.Count > 0)
                    {
                        if (control is FrameworkElement frameworkElement)
                        {
                            if (Orientation == Orientation.Horizontal)
                            {
                                frameworkElement.Margin = new Thickness(Spacing, 0, 0, 0);
                            }
                            else
                            {
                                frameworkElement.Margin = new Thickness(0, Spacing, 0, 0);
                            }
                        }
                    }
                    elementsPanel.Children.Add(control);
                }
            }
            
            // Wrap elements panel in a ScrollViewer
            var scrollViewer = new ScrollViewer
            {
                Name = $"{Name}_ScrollViewer",
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Content = elementsPanel
            };
            
            mainPanel.Children.Add(scrollViewer);
            SetupPanelControl(mainPanel);
            
            return mainPanel;
        }
        
        public override bool ValidateElement()
        {
            bool isValid = true;
            foreach (var element in Elements.Values)
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

        public override void SetupControls(object? valueControl, object? panelControl, object? labelControl)
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
    }
}
