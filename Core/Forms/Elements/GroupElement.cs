using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Models;
using DynamicInterfaceBuilder.Core.Helpers;
using DynamicInterfaceBuilder.Core.Interfaces;
using DynamicInterfaceBuilder.Core.Forms;

namespace DynamicInterfaceBuilder.Core.Forms.Elements
{
    [FormElement]
    public class GroupElement(App application, string name, FormElementType type) : FormElementBase(application, name, type), IFormGroup
    {
        public Dictionary<string, FormElementBase> Elements { get; set; } = new();
        public IReadOnlyList<FormElementBase> Children => [.. Elements.Values];

        /// <summary>
        /// Optional group name for styling and identification purposes
        /// </summary>
        public string? GroupName { get; set; }

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
            if (Constants.Default.GroupShowHeader && !string.IsNullOrEmpty(Label))
            {
                var headerBorder = new Border
                {
                    Name = $"{Name}_HeaderBorder",
                    Background = StyleHelper.GetColorBrush(Constants.Default.GroupHeaderBackground),
                    CornerRadius = new CornerRadius(Constants.Default.GroupCornerRadius.TopLeft, Constants.Default.GroupCornerRadius.TopRight, 0, 0),
                };

                var headerContent = new TextBlock
                {
                    Name = $"{Name}_HeaderContent",
                    Text = Label,
                    FontWeight = Constants.Default.GroupFontWeight,
                    FontSize = Constants.Default.GroupFontSize,
                    Foreground = StyleHelper.GetColorBrush(Constants.Default.GroupHeaderForeground),
                    Margin = new Thickness(Constants.Default.GroupSpacing)
                };

                headerBorder.Child = headerContent;
                innerContainer.Children.Add(headerBorder);
                
                // Add separator line with same color and thickness as border
                var headerSeparator = new Border
                {
                    Name = $"{Name}_HeaderSeparator",
                    BorderBrush = StyleHelper.GetColorBrush(Constants.Default.BorderColor),
                    BorderThickness = new Thickness(0, Constants.Default.GroupBorderThickness, 0, 0),
                    Height = Constants.Default.GroupBorderThickness
                };
                
                innerContainer.Children.Add(headerSeparator);
            }

            // Create container for elements
            var elementsPanel = new StackPanel
            {
                Name = $"{Name}_ElementsPanel",
                Orientation = Constants.Default.GroupOrientation,
                Margin = new Thickness(Constants.Default.GroupSpacing)
            };

            // Add elements to the panel
            int index = 0;
            foreach (var element in Elements.Values)
            {
                if (element.BuildElement() is UIElement control)
                {
                    if (Constants.Default.GroupSpacing > 0 && index > 0) // Apply margin only to non-first elements
                    {
                        if (control is FrameworkElement frameworkElement)
                        {
                            frameworkElement.Margin = Constants.Default.GroupOrientation == Orientation.Horizontal
                                ? new Thickness(Constants.Default.GroupSpacing, 0, 0, 0)
                                : new Thickness(0, Constants.Default.GroupSpacing, 0, 0);
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
                CornerRadius = Constants.Default.GroupShowHeader 
                    ? new CornerRadius(0, 0, Constants.Default.GroupCornerRadius.BottomLeft, Constants.Default.GroupCornerRadius.BottomRight)
                    : Constants.Default.GroupCornerRadius,
                Child = elementsPanel,
            };

            innerContainer.Children.Add(elementsBorder);

            // Create single border for the group
            var innerBorder = new Border
            {
                Name = $"{Name}_StackBorder",
                BorderBrush = Constants.Default.GroupShowBorder ? StyleHelper.GetColorBrush(Constants.Default.GroupBorderColor) : null,
                BorderThickness = Constants.Default.GroupShowBorder ? new Thickness(Constants.Default.GroupBorderThickness) : new Thickness(0),
                CornerRadius = Constants.Default.GroupCornerRadius,
                Child = innerContainer,
                ClipToBounds = true,
            };

            // Configure ScrollViewer
            var scrollViewer = new ScrollViewer
            {
                Name = $"{Name}_ScrollViewer",
                VerticalScrollBarVisibility = Constants.Default.GroupEnableVerticalScroll ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled,
                HorizontalScrollBarVisibility = Constants.Default.GroupEnableHorizontalScroll ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled,
                Content = innerBorder,
                MaxHeight = Constants.Default.GroupMaxHeight > 0 ? Constants.Default.GroupMaxHeight : double.PositiveInfinity,
                MaxWidth = Constants.Default.GroupMaxWidth > 0 ? Constants.Default.GroupMaxWidth : double.PositiveInfinity
            };
            
            panel.Children.Add(scrollViewer);
            SetupPanelControl(panel);
            
            return panel;
        }

        public override void SetupElement()
        {
            Tooltip = Description;
        }

        public override void ResetElement()
        {
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
