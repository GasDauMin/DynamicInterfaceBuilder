using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DynamicInterfaceBuilder.Core.Managers;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Form.Helpers
{
    public static class StyleHelper
    {
        public static SolidColorBrush? GetColorBrush(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            
            // First check ThemeManager directly - this is likely more efficient than trying conversion
            var themeBrush = ThemeManager.GetBrush(value);
            if (themeBrush != null)
                return themeBrush;
            
            // If theme manager didn't find a brush, try to convert from string
            try
            {
                var color = (Color)ColorConverter.ConvertFromString(value);
                return new SolidColorBrush(color);
            }
            catch
            {
                return null;
            }
        }

        public static void ApplyValueControlAlertStyle(Control control, StyleProperties styleProperties)
        {
            control.Background = GetColorBrush(styleProperties.AlertBackground);
            control.Foreground = GetColorBrush(styleProperties.AlertForeground);
            control.BorderBrush = GetColorBrush(styleProperties.AlertBorderColor);
        }

        public static void ApplyValueControlStyles(Control control, StyleProperties styleProperties)
        {
            if (styleProperties?.ValueControl?.Margin != null)
                control.Margin = (Thickness)styleProperties.ValueControl.Margin;

            if (styleProperties?.ValueControl?.Padding != null)
                control.Padding = (Thickness)styleProperties.ValueControl.Padding;

            if (styleProperties?.ValueControl?.FontWeight != null)
                control.FontWeight = (FontWeight)styleProperties.ValueControl.FontWeight;

            if (styleProperties?.ValueControl?.FontFamily != null)
                control.FontFamily = styleProperties.ValueControl.FontFamily;

            if (styleProperties?.ValueControl?.FontSize != null)
                control.FontSize = (double)styleProperties.ValueControl.FontSize;

            if (styleProperties?.ValueControl?.Foreground != null)
                control.Foreground = GetColorBrush(styleProperties?.ValueControl?.Foreground);

            if (styleProperties?.ValueControl?.Background != null)
                control.Background = GetColorBrush(styleProperties?.ValueControl?.Background);

            if (styleProperties?.ValueControl?.HorizontalAlignment != null)
                control.HorizontalAlignment = (HorizontalAlignment)styleProperties.ValueControl.HorizontalAlignment;

            if (styleProperties?.ValueControl?.VerticalAlignment != null)
                control.VerticalAlignment = (VerticalAlignment)styleProperties.ValueControl.VerticalAlignment;
        }

        public static void ApplyPanelControlStyles(Control control, StyleProperties styleProperties)
        {
            if (styleProperties?.PanelControl?.Margin != null)
                control.Margin = (Thickness)styleProperties.PanelControl.Margin;

            if (styleProperties?.PanelControl?.Padding != null)
                control.Padding = (Thickness)styleProperties.PanelControl.Padding;

            if (styleProperties?.PanelControl?.HorizontalAlignment != null)
                control.HorizontalAlignment = (HorizontalAlignment)styleProperties.PanelControl.HorizontalAlignment;

            if (styleProperties?.PanelControl?.VerticalAlignment != null)
                control.VerticalAlignment = (VerticalAlignment)styleProperties.PanelControl.VerticalAlignment;
            
            if (styleProperties?.PanelControl?.BorderColor != null)
                control.BorderBrush = GetColorBrush(styleProperties?.PanelControl?.BorderColor);
                
            if (styleProperties?.PanelControl?.BorderThickness != null)
                control.BorderThickness = (Thickness)styleProperties.PanelControl.BorderThickness;
            
            if (styleProperties?.PanelControl?.Width != null)
                control.Width = (double)styleProperties.PanelControl.Width;
            
            if (styleProperties?.PanelControl?.MinWidth != null)
                control.MinWidth = (double)styleProperties.PanelControl.MinWidth;
            
            if (styleProperties?.PanelControl?.MaxWidth != null)
                control.MaxWidth = (double)styleProperties.PanelControl.MaxWidth;
            
            if (styleProperties?.PanelControl?.Height != null)
                control.Height = (double)styleProperties.PanelControl.Height;
            
            if (styleProperties?.PanelControl?.MinHeight != null)
                control.MinHeight = (double)styleProperties.PanelControl.MinHeight;
            
            if (styleProperties?.PanelControl?.MaxHeight != null)
                control.MaxHeight = (double)styleProperties.PanelControl.MaxHeight;
        }

        public static void ApplyLabelControlStyles(Control control, StyleProperties styleProperties)
        {
            if (styleProperties?.LabelControl?.Margin != null)
                control.Margin = (Thickness)styleProperties.LabelControl.Margin;

            if (styleProperties?.LabelControl?.Padding != null)
                control.Padding = (Thickness)styleProperties.LabelControl.Padding;

            if (styleProperties?.LabelControl?.FontWeight != null)
                control.FontWeight = (FontWeight)styleProperties.LabelControl.FontWeight;

            if (styleProperties?.LabelControl?.FontFamily != null)
                control.FontFamily = styleProperties.LabelControl.FontFamily;

            if (styleProperties?.LabelControl?.FontSize != null)
                control.FontSize = (double)styleProperties.LabelControl.FontSize;

            if (styleProperties?.LabelControl?.Foreground != null)
                control.Foreground = GetColorBrush(styleProperties?.LabelControl?.Foreground);

            if (styleProperties?.LabelControl?.Background != null)
                control.Background = GetColorBrush(styleProperties?.LabelControl?.Background);

            if (styleProperties?.LabelControl?.HorizontalAlignment != null)
                control.HorizontalAlignment = (HorizontalAlignment)styleProperties.LabelControl.HorizontalAlignment;

            if (styleProperties?.LabelControl?.VerticalAlignment != null)
                control.VerticalAlignment = (VerticalAlignment)styleProperties.LabelControl.VerticalAlignment;
            
            if (styleProperties?.LabelControl?.Width != null)
                control.Width = (double)styleProperties.LabelControl.Width;
            
            if (styleProperties?.LabelControl?.MinWidth != null)
                control.MinWidth = (double)styleProperties.LabelControl.MinWidth;
            
            if (styleProperties?.LabelControl?.MaxWidth != null)
                control.MaxWidth = (double)styleProperties.LabelControl.MaxWidth;
            
            if (styleProperties?.LabelControl?.Height != null)
                control.Height = (double)styleProperties.LabelControl.Height;
            
            if (styleProperties?.LabelControl?.MinHeight != null)
                control.MinHeight = (double)styleProperties.LabelControl.MinHeight;
            
            if (styleProperties?.LabelControl?.MaxHeight != null)
                control.MaxHeight = (double)styleProperties.LabelControl.MaxHeight;
        }

        public static void ResetControlStyle(Control control)
        {
            control.ClearValue(Control.BackgroundProperty);
            control.ClearValue(Control.ForegroundProperty);
            control.ClearValue(Control.BorderBrushProperty);
        }
    }
}