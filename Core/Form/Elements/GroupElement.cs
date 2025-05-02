using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Form.Enums;
using DynamicInterfaceBuilder.Core.Form.Helpers;
using DynamicInterfaceBuilder.Core.Form.Interfaces;
using DynamicInterfaceBuilder.Core.Form.Models;

namespace DynamicInterfaceBuilder.Core.Form.Elements
{
    [FormElement]
    public class GroupElement(App application, string name) : FormElementBase(application, name, FormElementType.Group), IFormGroup
    {
        public Dictionary<string, FormElementBase> Elements { get; set; } = new();
        public IReadOnlyList<FormElementBase> Children => [.. Elements.Values];

        public void AddChild(FormElementBase child)
        {
            Elements[child.Name] = child;
        }
        
        public override UIElement? BuildElement()
        {
            var panel = new DockPanel
            {
                Name = $"{Name}_Panel",
                VerticalAlignment = VerticalAlignment.Top
            };

            // Create inner container for header and elements
            var innerContainer = new StackPanel
            {
                Name = $"{Name}_InnerContainer",
            };
            
            // Add group header if label is provided and ShowHeader is true
            if (StyleProperties.GroupShowHeader && !string.IsNullOrEmpty(Label))
            {
                var headerBorder = new Border
                {
                    Name = $"{Name}_HeaderBorder",
                    Background = StyleHelper.GetColorBrush(StyleProperties.GroupHeaderBackground),
                    CornerRadius = new CornerRadius(StyleProperties.GroupCornerRadius.TopLeft, StyleProperties.GroupCornerRadius.TopRight, 0, 0),
                };

                var headerContent = new TextBlock
                {
                    Name = $"{Name}_HeaderContent",
                    Text = Label,
                    FontWeight = StyleProperties.GroupFontWeight,
                    FontSize = StyleProperties.GroupFontSize,
                    Foreground = StyleHelper.GetColorBrush(StyleProperties.GroupHeaderForeground),
                    Margin = new Thickness(StyleProperties.GroupSpacing)
                };

                headerBorder.Child = headerContent;
                innerContainer.Children.Add(headerBorder);
                
                // Add separator line with same color and thickness as border
                var headerSeparator = new Border
                {
                    Name = $"{Name}_HeaderSeparator",
                    BorderBrush = StyleHelper.GetColorBrush(StyleProperties.BorderColor),
                    BorderThickness = new Thickness(0, StyleProperties.GroupBorderThickness, 0, 0),
                    Height = StyleProperties.GroupBorderThickness
                };
                
                innerContainer.Children.Add(headerSeparator);
            }

            // Create container for elements
            var elementsPanel = new StackPanel
            {
                Name = $"{Name}_ElementsPanel",
                Orientation = StyleProperties.GroupOrientation,
                Margin = new Thickness(StyleProperties.GroupSpacing)
            };

            // Add elements to the panel
            int index = 0;
            foreach (var element in Elements.Values)
            {
                if (element.BuildElement() is UIElement control)
                {
                    if (StyleProperties.GroupSpacing > 0 && index > 0) // Apply margin only to non-first elements
                    {
                        if (control is FrameworkElement frameworkElement)
                        {
                            frameworkElement.Margin = StyleProperties.GroupOrientation == Orientation.Horizontal
                                ? new Thickness(StyleProperties.GroupSpacing, 0, 0, 0)
                                : new Thickness(0, StyleProperties.GroupSpacing, 0, 0);
                        }
                    }
                    elementsPanel.Children.Add(control);
                    index++;
                }
            }

            // Create elements border
            var elementsBorder = new Border
            {
                Name = $"{Name}_ElementsBorder",
                CornerRadius = StyleProperties.GroupShowHeader 
                    ? new CornerRadius(0, 0, StyleProperties.GroupCornerRadius.BottomLeft, StyleProperties.GroupCornerRadius.BottomRight)
                    : StyleProperties.GroupCornerRadius,
                Child = elementsPanel,
            };

            innerContainer.Children.Add(elementsBorder);

            // Create single border for the group
            var innerBorder = new Border
            {
                Name = $"{Name}_StackBorder",
                BorderBrush = StyleProperties.GroupShowBorder ? StyleHelper.GetColorBrush(StyleProperties.GroupBorderColor) : null,
                BorderThickness = StyleProperties.GroupShowBorder ? new Thickness(StyleProperties.GroupBorderThickness) : new Thickness(0),
                CornerRadius = StyleProperties.GroupCornerRadius,
                Child = innerContainer,
                ClipToBounds = true,
            };

            // Configure ScrollViewer
            var scrollViewer = new ScrollViewer
            {
                Name = $"{Name}_ScrollViewer",
                VerticalScrollBarVisibility = StyleProperties.GroupEnableVerticalScroll ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled,
                HorizontalScrollBarVisibility = StyleProperties.GroupEnableHorizontalScroll ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled,
                Content = innerBorder,
                MaxHeight = StyleProperties.GroupMaxHeight > 0 ? StyleProperties.GroupMaxHeight : double.PositiveInfinity,
                MaxWidth = StyleProperties.GroupMaxWidth > 0 ? StyleProperties.GroupMaxWidth : double.PositiveInfinity
            };
            
            panel.Children.Add(scrollViewer);
            SetupPanelControl(panel);
            
            return panel;
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
