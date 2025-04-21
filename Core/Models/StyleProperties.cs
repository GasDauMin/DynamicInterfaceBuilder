using System.Windows;
using System.Windows.Media;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Interfaces;
using DynamicInterfaceBuilder.Core.Constants;

namespace DynamicInterfaceBuilder.Core.Models
{
    [ExtendedProperties]
    public class StyleProperties : IProperties
    {
        #region Form Style Properties
        
        public int Spacing { get; set; } = Default.Spacing;
        public Thickness Margin { get; set; } = Default.Margin;
        public Thickness Padding { get; set; } = Default.Padding;
        public HorizontalAlignment HorizontalAlignment { get; set; } = Default.HorizontalAlignment;
        public VerticalAlignment VerticalAlignment { get; set; } = Default.VerticalAlignment;
        public FontWeight FontWeight { get; set; } = Default.FontWeight;
        public FontFamily FontFamily { get; set; } = Default.FontFamily;
        public double FontSize { get; set; } = Default.FontSize;
        public Thickness BorderThickness { get; set; } = Default.BorderThickness;
        public SolidColorBrush? BorderColor { get; set; } = Default.BorderColor;
        
        #endregion
        
        #region Panel Control Style Properties
        
        public Thickness PanelControlMargin { get; set; } = Default.PanelControlMargin;
        public Thickness PanelControlPadding { get; set; } = Default.PanelControlPadding;
        public HorizontalAlignment PanelControlHorizontalAlignment { get; set; } = Default.PanelControlHorizontalAlignment;
        public VerticalAlignment PanelControlVerticalAlignment { get; set; } = Default.PanelControlVerticalAlignment;
        public Thickness PanelControlBorderThickness { get; set; } = Default.PanelControlBorderThickness;
        public SolidColorBrush? PanelControlBorderColor { get; set; } = Default.PanelControlBorderColor;
        
        #endregion
        
        #region Value Control Style Properties
        
        public Thickness ValueControlMargin { get; set; } = Default.ValueControlMargin;
        public Thickness ValueControlPadding { get; set; } = Default.ValueControlPadding;
        public FontWeight ValueControlFontWeight { get; set; } = Default.ValueControlFontWeight;
        public FontFamily ValueControlFontFamily { get; set; } = Default.ValueControlFontFamily;
        public double ValueControlFontSize { get; set; } = Default.ValueControlFontSize;
        public SolidColorBrush? ValueControlForeground { get; set; } = Default.ValueControlForeground;
        public SolidColorBrush? ValueControlBackground { get; set; } = Default.ValueControlBackground;
        public HorizontalAlignment ValueControlHorizontalAlignment { get; set; } = Default.ValueControlHorizontalAlignment;
        public VerticalAlignment ValueControlVerticalAlignment { get; set; } = Default.ValueControlVerticalAlignment;
        
        #endregion
        
        #region Label Control Style Properties
        
        public Thickness LabelControlMargin { get; set; } = Default.LabelControlMargin;
        public Thickness LabelControlPadding { get; set; } = Default.LabelControlPadding;
        public FontWeight LabelControlFontWeight { get; set; } = Default.LabelControlFontWeight;
        public FontFamily LabelControlFontFamily { get; set; } = Default.LabelControlFontFamily;
        public double LabelControlFontSize { get; set; } = Default.LabelControlFontSize;
        public SolidColorBrush? LabelControlForeground { get; set; } = Default.LabelControlForeground;
        public SolidColorBrush? LabelControlBackground { get; set; } = Default.LabelControlBackground;
        public HorizontalAlignment LabelControlHorizontalAlignment { get; set; } = Default.LabelControlHorizontalAlignment;
        public VerticalAlignment LabelControlVerticalAlignment { get; set; } = Default.LabelControlVerticalAlignment;
        
        #endregion
        
        #region Button Control Style Properties
        
        public Thickness ButtonControlMargin { get; set; } = Default.ButtonControlMargin;
        public Thickness ButtonControlPadding { get; set; } = Default.ButtonControlPadding;
        public FontWeight ButtonControlFontWeight { get; set; } = Default.ButtonControlFontWeight;
        public FontFamily ButtonControlFontFamily { get; set; } = Default.ButtonControlFontFamily;
        public double ButtonControlFontSize { get; set; } = Default.ButtonControlFontSize;
        public SolidColorBrush? ButtonControlForeground { get; set; } = Default.ButtonControlForeground;
        public SolidColorBrush? ButtonControlBackground { get; set; } = Default.ButtonControlBackground;
        public HorizontalAlignment ButtonControlHorizontalAlignment { get; set; } = Default.ButtonControlHorizontalAlignment;
        public VerticalAlignment ButtonControlVerticalAlignment { get; set; } = Default.ButtonControlVerticalAlignment;
        public Thickness? ButtonControlBorderThickness { get; set; } = Default.ButtonControlBorderThickness;
        public SolidColorBrush? ButtonControlBorderColor { get; set; } = Default.ButtonControlBorderColor;
        
        #endregion

        public void ResetDefaults()
        {
            // Form Styles
            Spacing = Default.Spacing;
            Margin = Default.Margin;
            Padding = Default.Padding;
            HorizontalAlignment = Default.HorizontalAlignment;
            VerticalAlignment = Default.VerticalAlignment;
            FontWeight = Default.FontWeight;
            FontFamily = Default.FontFamily;
            FontSize = Default.FontSize;
            BorderThickness = Default.BorderThickness;
            BorderColor = Default.BorderColor;
            
            // Panel Control Styles
            PanelControlMargin = Default.PanelControlMargin;
            PanelControlPadding = Default.PanelControlPadding;
            PanelControlHorizontalAlignment = Default.PanelControlHorizontalAlignment;
            PanelControlVerticalAlignment = Default.PanelControlVerticalAlignment;
            PanelControlBorderThickness = Default.PanelControlBorderThickness;
            PanelControlBorderColor = Default.PanelControlBorderColor;
            
            // Value Control Styles
            ValueControlMargin = Default.ValueControlMargin;
            ValueControlPadding = Default.ValueControlPadding;
            ValueControlFontWeight = Default.ValueControlFontWeight;
            ValueControlFontFamily = Default.ValueControlFontFamily;
            ValueControlFontSize = Default.ValueControlFontSize;
            ValueControlForeground = Default.ValueControlForeground;
            ValueControlBackground = Default.ValueControlBackground;
            ValueControlHorizontalAlignment = Default.ValueControlHorizontalAlignment;
            ValueControlVerticalAlignment = Default.ValueControlVerticalAlignment;
            
            // Label Control Styles
            LabelControlMargin = Default.LabelControlMargin;
            LabelControlPadding = Default.LabelControlPadding;
            LabelControlFontWeight = Default.LabelControlFontWeight;
            LabelControlFontFamily = Default.LabelControlFontFamily;
            LabelControlFontSize = Default.LabelControlFontSize;
            LabelControlForeground = Default.LabelControlForeground;
            LabelControlBackground = Default.LabelControlBackground;
            LabelControlHorizontalAlignment = Default.LabelControlHorizontalAlignment;
            LabelControlVerticalAlignment = Default.LabelControlVerticalAlignment;
            
            // Button Control Styles
            ButtonControlMargin = Default.ButtonControlMargin;
            ButtonControlPadding = Default.ButtonControlPadding;
            ButtonControlFontWeight = Default.ButtonControlFontWeight;
            ButtonControlFontFamily = Default.ButtonControlFontFamily;
            ButtonControlFontSize = Default.ButtonControlFontSize;
            ButtonControlForeground = Default.ButtonControlForeground;
            ButtonControlBackground = Default.ButtonControlBackground;
            ButtonControlHorizontalAlignment = Default.ButtonControlHorizontalAlignment;
            ButtonControlVerticalAlignment = Default.ButtonControlVerticalAlignment;
            ButtonControlBorderThickness = Default.ButtonControlBorderThickness;
            ButtonControlBorderColor = Default.ButtonControlBorderColor;
        }
    }
}