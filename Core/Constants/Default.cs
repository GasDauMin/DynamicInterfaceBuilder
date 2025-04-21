using DynamicInterfaceBuilder.Core.UI.Enums;
using System.Windows;
using System.Windows.Media;

namespace DynamicInterfaceBuilder.Core.Constants
{
    public static class Default
    {
        #region Default Application Constants

        public const ThemeType Theme = ThemeType.Default;
        public const string Title = "Dynamic Interface Builder";
        public const int MaxMessageLines = 4;
        public const bool AutoSaveConfig = true;
        public const bool AutoLoadConfig = true;
        public const bool AllowResize = true;
        public const bool AdjustLabels = true;
        public const bool ReverseButtons = false;
        public const bool AllowAlertControl = false;
        public const bool AllowValidationControl = true;
        public const string Icon = "";

        #endregion

        #region Default Style Constants

        // Layout style defaults

        public static readonly int Spacing = 5;

        // Form style defaults

        public const int Width = 700;
        public const int Height = 400;
        public const int MinWidth = 300;
        public const int MinHeight = 200;
        public static readonly Thickness Margin = new(0);
        public static readonly Thickness Padding = new(0);
        public static readonly FontWeight FontWeight = FontWeights.Normal;
        public static readonly FontFamily FontFamily = new("Segoe UI Emoji");
        public static readonly double FontSize = 12.0;
        public static readonly HorizontalAlignment HorizontalAlignment = HorizontalAlignment.Stretch;
        public static readonly VerticalAlignment VerticalAlignment = VerticalAlignment.Stretch;
        public static readonly Thickness BorderThickness = new(0);
        public static readonly SolidColorBrush? BorderColor = null;

        // Panel control style defaults

        public static readonly Thickness PanelControlMargin = new(0);
        public static readonly Thickness PanelControlPadding = new(0);
        public static readonly HorizontalAlignment PanelControlHorizontalAlignment = HorizontalAlignment.Stretch;
        public static readonly VerticalAlignment PanelControlVerticalAlignment = VerticalAlignment.Stretch;
        public static readonly Thickness PanelControlBorderThickness = new(1);
        public static readonly SolidColorBrush? PanelControlBorderColor = null;

        // Value control style defaults

        public static readonly Thickness ValueControlMargin = new(0);
        public static readonly Thickness ValueControlPadding = new(0);
        public static readonly HorizontalAlignment ValueControlHorizontalAlignment = HorizontalAlignment.Left;
        public static readonly VerticalAlignment ValueControlVerticalAlignment = VerticalAlignment.Center;
        public static readonly FontWeight ValueControlFontWeight = Default.FontWeight;
        public static readonly FontFamily ValueControlFontFamily = Default.FontFamily;
        public static readonly double ValueControlFontSize = Default.FontSize;
        public static readonly SolidColorBrush? ValueControlForeground = null;
        public static readonly SolidColorBrush? ValueControlBackground = null;

        // Label control style defaults

        public static readonly Thickness LabelControlMargin = new(0);
        public static readonly Thickness LabelControlPadding = new(0);
        public static readonly HorizontalAlignment LabelControlHorizontalAlignment = HorizontalAlignment.Left;
        public static readonly VerticalAlignment LabelControlVerticalAlignment = VerticalAlignment.Center;
        public static readonly FontWeight LabelControlFontWeight = Default.FontWeight;
        public static readonly FontFamily LabelControlFontFamily = Default.FontFamily;
        public static readonly double LabelControlFontSize = Default.FontSize;
        public static readonly SolidColorBrush? LabelControlForeground = null;
        public static readonly SolidColorBrush? LabelControlBackground = null;

        // Button control style defaults

        public static readonly Thickness ButtonControlMargin = new(0);
        public static readonly Thickness ButtonControlPadding = new(0);
        public static readonly HorizontalAlignment ButtonControlHorizontalAlignment = HorizontalAlignment.Right;
        public static readonly VerticalAlignment ButtonControlVerticalAlignment = VerticalAlignment.Center;
        public static readonly FontWeight ButtonControlFontWeight = Default.FontWeight;
        public static readonly FontFamily ButtonControlFontFamily = Default.FontFamily;
        public static readonly double ButtonControlFontSize = Default.FontSize;
        public static readonly Thickness? ButtonControlBorderThickness = null;
        public static readonly SolidColorBrush? ButtonControlBorderColor = null;
        public static readonly SolidColorBrush? ButtonControlForeground = null;
        public static readonly SolidColorBrush? ButtonControlBackground = null;

        #endregion
    }
}