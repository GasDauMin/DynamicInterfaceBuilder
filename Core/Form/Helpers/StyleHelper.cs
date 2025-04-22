using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DynamicInterfaceBuilder.Core.Managers;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Form.Helpers
{
    public static class StyleHelper
    {
        public static void ApplyValueControlAlertStyle(Control control, StyleProperties styleProperties)
        {
            control.Background = styleProperties.AlertBackground;
            control.Foreground = styleProperties.AlertForeground;
            control.BorderBrush = styleProperties.AlertBorderBrush;
        }

        public static void ApplyValueControlStyles(Control control, StyleProperties styleProperties)
        {
            if (styleProperties.ValueControlMargin != null)
                control.Margin = (Thickness)styleProperties.ValueControlMargin;

            if (styleProperties.ValueControlPadding != null)
                control.Padding = (Thickness)styleProperties.ValueControlPadding;

            if (styleProperties.ValueControlFontWeight != null)
                control.FontWeight = (FontWeight)styleProperties.ValueControlFontWeight;

            if (styleProperties.ValueControlFontFamily != null)
                control.FontFamily = styleProperties.ValueControlFontFamily;

            if (styleProperties.ValueControlFontSize != null)
                control.FontSize = (double)styleProperties.ValueControlFontSize;

            if (styleProperties.ValueControlForeground != null)
                control.Foreground = styleProperties.ValueControlForeground;

            if (styleProperties.ValueControlBackground != null)
                control.Background = styleProperties.ValueControlBackground;

            if (styleProperties.ValueControlHorizontalAlignment != null)
                control.HorizontalAlignment = (HorizontalAlignment)styleProperties.ValueControlHorizontalAlignment;

            if (styleProperties.ValueControlVerticalAlignment != null)
                control.VerticalAlignment = (VerticalAlignment)styleProperties.ValueControlVerticalAlignment;
        }

        public static void ApplyPanelControlStyles(Control control, StyleProperties styleProperties)
        {
            if (styleProperties.PanelControlMargin != null)
                control.Margin = (Thickness)styleProperties.PanelControlMargin;

            if (styleProperties.PanelControlPadding != null)
                control.Padding = (Thickness)styleProperties.PanelControlPadding;

            if (styleProperties.PanelControlHorizontalAlignment != null)
                control.HorizontalAlignment = (HorizontalAlignment)styleProperties.PanelControlHorizontalAlignment;

            if (styleProperties.PanelControlVerticalAlignment != null)
                control.VerticalAlignment = (VerticalAlignment)styleProperties.PanelControlVerticalAlignment;
            
            if (styleProperties.PanelControlWidth != null)
                control.Width = (double)styleProperties.PanelControlWidth;
            
            if (styleProperties.PanelControlMinWidth != null)
                control.MinWidth = (double)styleProperties.PanelControlMinWidth;
            
            if (styleProperties.PanelControlMaxWidth != null)
                control.MaxWidth = (double)styleProperties.PanelControlMaxWidth;
            
            if (styleProperties.PanelControlHeight != null)
                control.Height = (double)styleProperties.PanelControlHeight;
            
            if (styleProperties.PanelControlMinHeight != null)
                control.MinHeight = (double)styleProperties.PanelControlMinHeight;
            
            if (styleProperties.PanelControlMaxHeight != null)
                control.MaxHeight = (double)styleProperties.PanelControlMaxHeight;
        }

        public static void ApplyLabelControlStyles(Control control, StyleProperties styleProperties)
        {
            if (styleProperties.LabelControlMargin != null)
                control.Margin = (Thickness)styleProperties.LabelControlMargin;

            if (styleProperties.LabelControlPadding != null)
                control.Padding = (Thickness)styleProperties.LabelControlPadding;

            if (styleProperties.LabelControlFontWeight != null)
                control.FontWeight = (FontWeight)styleProperties.LabelControlFontWeight;

            if (styleProperties.LabelControlFontFamily != null)
                control.FontFamily = styleProperties.LabelControlFontFamily;

            if (styleProperties.LabelControlFontSize != null)
                control.FontSize = (double)styleProperties.LabelControlFontSize;

            if (styleProperties.LabelControlForeground != null)
                control.Foreground = styleProperties.LabelControlForeground;

            if (styleProperties.LabelControlBackground != null)
                control.Background = styleProperties.LabelControlBackground;

            if (styleProperties.LabelControlHorizontalAlignment != null)
                control.HorizontalAlignment = (HorizontalAlignment)styleProperties.LabelControlHorizontalAlignment;

            if (styleProperties.LabelControlVerticalAlignment != null)
                control.VerticalAlignment = (VerticalAlignment)styleProperties.LabelControlVerticalAlignment;
            
            if (styleProperties.LabelControlWidth != null)
                control.Width = (double)styleProperties.LabelControlWidth;
            
            if (styleProperties.LabelControlMinWidth != null)
                control.MinWidth = (double)styleProperties.LabelControlMinWidth;
            
            if (styleProperties.LabelControlMaxWidth != null)
                control.MaxWidth = (double)styleProperties.LabelControlMaxWidth;
            
            if (styleProperties.LabelControlHeight != null)
                control.Height = (double)styleProperties.LabelControlHeight;
            
            if (styleProperties.LabelControlMinHeight != null)
                control.MinHeight = (double)styleProperties.LabelControlMinHeight;
            
            if (styleProperties.LabelControlMaxHeight != null)
                control.MaxHeight = (double)styleProperties.LabelControlMaxHeight;
        }

        public static void ResetControlStyle(Control control)
        {
            control.ClearValue(Control.BackgroundProperty);
            control.ClearValue(Control.ForegroundProperty);
            control.ClearValue(Control.BorderBrushProperty);
        }
    }
} 