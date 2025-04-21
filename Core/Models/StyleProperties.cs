using System.Windows;
using System.Windows.Media;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Constants;

namespace DynamicInterfaceBuilder.Core.Models
{
    [ExtendedProperties]
    public class StyleProperties
    {
        public int Spacing { get; set; } = DefaultStyle.Spacing;
        public Thickness Margin { get; set; } = DefaultStyle.Margin;
        public Thickness Padding { get; set; } = DefaultStyle.Padding;
        public HorizontalAlignment HorizontalAlignment { get; set; } = DefaultStyle.HorizontalAlignment;
        public VerticalAlignment VerticalAlignment { get; set; } = DefaultStyle.VerticalAlignment;
        public FontWeight FontWeight { get; set; } = DefaultStyle.FontWeight;
        public FontFamily FontFamily { get; set; } = DefaultStyle.FontFamily;
        public double FontSize { get; set; } = DefaultStyle.FontSize;
        public Thickness BorderThickness { get; set; } = DefaultStyle.BorderThickness;
        public SolidColorBrush BorderColor { get; set; } = DefaultStyle.BorderColor;

        public void ResetDefaults()
        {
            Spacing = DefaultStyle.Spacing;
            Margin = DefaultStyle.Margin;
            Padding = DefaultStyle.Padding;
            HorizontalAlignment = DefaultStyle.HorizontalAlignment;
            VerticalAlignment = DefaultStyle.VerticalAlignment;

            FontWeight = DefaultStyle.FontWeight;
            FontFamily = DefaultStyle.FontFamily;
            FontSize = DefaultStyle.FontSize;

            BorderThickness = DefaultStyle.BorderThickness;
            BorderColor = DefaultStyle.BorderColor;
        }
    }
}