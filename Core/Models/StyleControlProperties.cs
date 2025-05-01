using System.Windows;
using System.Windows.Media;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Interfaces;
using DynamicInterfaceBuilder.Core.Constants;
using DynamicInterfaceBuilder.Core.Managers;

namespace DynamicInterfaceBuilder.Core.Models
{
    [ExtendedProperties]
    public class StyleControlProperties : IProperties
    {
        public Thickness? Margin { get; set; }
        public Thickness? Padding { get; set; }
        public FontWeight? FontWeight { get; set; }
        public FontFamily? FontFamily { get; set; }
        public double? FontSize { get; set; }
        public string? Foreground { get; set; }
        public string? Background { get; set; }
        public HorizontalAlignment? HorizontalAlignment { get; set; }
        public VerticalAlignment? VerticalAlignment { get; set; }
        public double? Width { get; set; }
        public double? MinWidth { get; set; }
        public double? MaxWidth { get; set; }
        public double? Height { get; set; }
        public double? MinHeight { get; set; }
        public double? MaxHeight { get; set; }
        public Thickness? BorderThickness { get; set; }
        public string? BorderColor { get; set; }
        
        #region Constructor
        
        public StyleControlProperties()
        {
            Init();
        }
        
        #endregion

        #region Initialize

        public void Init()
        {      
        }

        #endregion
    }
}