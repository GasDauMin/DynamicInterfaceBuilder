using DynamicInterfaceBuilder.Core.UI.Enums;
using System;
using System.Windows;
using System.Windows.Media;

namespace DynamicInterfaceBuilder.Core.Constants
{
    public static class DefaultStyle
    {
        // Form style Defaults
        public const int FormWidth = 700;
        public const int FormHeight = 400;
        public const int FormMinWidth = 300;
        public const int FormMinHeight = 200;
        public static readonly Thickness FormMargin = new(0);
        public static readonly Thickness FormPadding = new(0);

        // Layout style Defaults
        public static readonly int Spacing = 5;
        public static readonly Thickness Margin = new(0);
        public static readonly Thickness Padding = new(0);
        public static readonly HorizontalAlignment HorizontalAlignment = HorizontalAlignment.Stretch;
        public static readonly VerticalAlignment VerticalAlignment = VerticalAlignment.Center;

        // Font style Defaults
        public static readonly FontWeight FontWeight = FontWeights.Normal;
        public static readonly FontFamily FontFamily = new FontFamily("Segoe UI Emoji");
        public static readonly double FontSize = 12.0;

        // Border style Defaults
        public static readonly Thickness BorderThickness = new(1);
        public static readonly SolidColorBrush BorderColor = new();
    }
}