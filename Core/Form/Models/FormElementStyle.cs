namespace DynamicInterfaceBuilder.Core.Form.Models
{
    public class FormElementStyle
    {
        public string? BackgroundColor { get; set; }
        public string? TextColor { get; set; }
        public string? BorderThickness { get; set; }
        public string? BorderColor { get; set; }
        public string? FontFamily { get; set; }
        public double? FontSize { get; set; }
        public double? CornerRadius { get; set; }
        public double? Padding { get; set; }
        public double? Margin { get; set; }
        public bool? IsVisible { get; set; }
        public bool? IsEnabled { get; set;}
    }
}