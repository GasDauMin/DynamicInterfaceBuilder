using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DynamicInterfaceBuilder.Core.Constants;
using DynamicInterfaceBuilder.Core.Managers;

namespace DynamicInterfaceBuilder.Core.Helpers
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

        public static void ApplyValueControlAlertStyle(Control control)
        {
            control.Background = GetColorBrush(Default.AlertBackground);
            control.Foreground = GetColorBrush(Default.AlertForeground);
            control.BorderBrush = GetColorBrush(Default.AlertBorderColor);
        }

        public static void ApplyValueControlStyles(Control control)
        {
            if (TryFindResource("DefaultValueControlStyle", out Style? style) && style != null)
                control.Style = style;
        }

        public static void ApplyPanelControlStyles(Control control)
        {
            if (TryFindResource("DefaultPanelControlStyle", out Style? style) && style != null)
                control.Style = style;
        }

        public static void ApplyLabelControlStyles(Control control)
        {
            if (TryFindResource("DefaultLabelControlStyle", out Style? style) && style != null)
                control.Style = style;
        }

        public static void ApplyButtonControlStyles(Control control)
        {
            if (TryFindResource("DefaultButtonControlStyle", out Style? style) && style != null)
                control.Style = style;
        }

        public static void ResetControlStyle(Control control)
        {
            control.Style = null;
        }
        
        private static bool TryGetResource(string key, out object? resource)
        {
            resource = null;
            try
            {
                if (Application.Current.Resources.Contains(key))
                {
                    resource = Application.Current.Resources[key];
                    return resource != null;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        
        private static bool TryFindResource<T>(string key, out T? resource) where T : class
        {
            resource = null;
            try
            {
                if (Application.Current.Resources.Contains(key))
                {
                    resource = Application.Current.Resources[key] as T;
                    return resource != null;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}