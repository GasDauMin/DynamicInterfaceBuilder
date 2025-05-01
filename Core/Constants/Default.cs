using DynamicInterfaceBuilder.Core.Managers;
using DynamicInterfaceBuilder.Core.Models;
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
        public static readonly FontWeight FontWeight = FontWeights.Normal;
        public static readonly FontFamily FontFamily = new("Segoe UI Emoji");
        public static readonly double FontSize = 12.0;
        public static readonly Thickness? Margin = new(0);
        public static readonly Thickness? Padding = new(0);
        public static readonly HorizontalAlignment HorizontalAlignment = HorizontalAlignment.Stretch;
        public static readonly VerticalAlignment VerticalAlignment = VerticalAlignment.Stretch;
        public static readonly Thickness BorderThickness = new(0);
        public static readonly string? BorderColor = null;
        public static readonly int? MinWidth = null;
        public static readonly int? MaxWidth = null;
        public static readonly int? MinHeight = null;
        public static readonly int? MaxHeight = null;

        // Panel control style defaults

        public static readonly Thickness? PanelControlMargin = new(0);
        public static readonly Thickness? PanelControlPadding = new(0);
        public static readonly HorizontalAlignment? PanelControlHorizontalAlignment = null;
        public static readonly VerticalAlignment? PanelControlVerticalAlignment = null;
        public static readonly Thickness? PanelControlBorderThickness = null;
        public static readonly string? PanelControlBorderColor = null;
        public static readonly string? PanelControlForeground = null;
        public static readonly string? PanelControlBackground = null;

        // Value control style defaults

        public static readonly Thickness? ValueControlMargin = new(0);
        public static readonly Thickness? ValueControlPadding = new(0);
        public static readonly FontWeight? ValueControlFontWeight = FontWeight;
        public static readonly FontFamily? ValueControlFontFamily = FontFamily;
        public static readonly double? ValueControlFontSize = FontSize;
        public static readonly HorizontalAlignment? ValueControlHorizontalAlignment = null;
        public static readonly VerticalAlignment? ValueControlVerticalAlignment = null;
        public static readonly string? ValueControlForeground = null;
        public static readonly string? ValueControlBackground = null;

        // Label control style defaults

        public static readonly Thickness? LabelControlMargin = new(0);
        public static readonly Thickness? LabelControlPadding = new(0);
        public static readonly FontWeight? LabelControlFontWeight = FontWeight;
        public static readonly FontFamily? LabelControlFontFamily = FontFamily;
        public static readonly double? LabelControlFontSize = FontSize;
        public static readonly HorizontalAlignment? LabelControlHorizontalAlignment = null;
        public static readonly VerticalAlignment? LabelControlVerticalAlignment = null;
        public static readonly string? LabelControlForeground = null;
        public static readonly string? LabelControlBackground = null;

        // Button control style defaults

        public static readonly Thickness? ButtonControlMargin = new(0);
        public static readonly Thickness? ButtonControlPadding = new(0);
        public static readonly HorizontalAlignment? ButtonControlHorizontalAlignment = HorizontalAlignment.Right;
        public static readonly VerticalAlignment? ButtonControlVerticalAlignment = VerticalAlignment.Center;
        public static readonly FontWeight? ButtonControlFontWeight = FontWeight;
        public static readonly FontFamily? ButtonControlFontFamily = FontFamily;
        public static readonly double? ButtonControlFontSize = FontSize;
        public static readonly Thickness? ButtonControlBorderThickness = null;
        public static readonly string? ButtonControlBorderColor = null;
        public static readonly string? ButtonControlForeground = null;
        public static readonly string? ButtonControlBackground = null;

        // Alert control style defaults

        public static readonly string? AlertBackground = "ABrush.AlertTone2";
        public static readonly string? AlertForeground = "ABrush.AlertTone5";
        public static readonly string? AlertBorderColor = "ABrush.AlertTone3";

        // Group element style defaults
        
        public static readonly double? GroupSpacing = Spacing;
        public static readonly Thickness? GroupMargin = null;
        public static readonly Thickness? GroupPadding = null;
        public static readonly bool? GroupShowBorder = true;
        public static readonly double? GroupBorderThickness = 1;
        public static readonly CornerRadius? GroupCornerRadius = new(3);
        public static readonly string? GroupBorderColor = null;
        public static readonly string? GroupBackground = null;
        public static readonly string? GroupHeaderBackground = null;
        public static readonly string? GroupHeaderForeground = Colors.White.ToString();

        #endregion
    }
}