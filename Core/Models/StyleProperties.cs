using System.Windows;
using System.Windows.Media;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Interfaces;
using DynamicInterfaceBuilder.Core.Constants;
using DynamicInterfaceBuilder.Core.Managers;

namespace DynamicInterfaceBuilder.Core.Models
{
    [ExtendedProperties]
    public class StyleProperties : IProperties
    {
        #region Form Style Properties
        
        public int Spacing { get; set; } = Default.Spacing;
        public FontWeight FontWeight { get; set; } = Default.FontWeight;
        public FontFamily FontFamily { get; set; } = Default.FontFamily;
        public double FontSize { get; set; } = Default.FontSize;
        public HorizontalAlignment? HorizontalAlignment { get; set; } = Default.HorizontalAlignment;
        public VerticalAlignment? VerticalAlignment { get; set; } = Default.VerticalAlignment;
        public SolidColorBrush? BorderColor { get; set; } = Default.BorderColor;
        public Thickness? BorderThickness { get; set; } = Default.BorderThickness;
        public Thickness? Margin { get; set; } = Default.Margin;
        public Thickness? Padding { get; set; } = Default.Padding;
        public double? Width { get; set; } = Default.Width;
        public double? MinWidth { get; set; } = Default.MinWidth;
        public double? MaxWidth { get; set; } = Default.MaxWidth;
        public double? Height { get; set; } = Default.Height;
        public double? MinHeight { get; set; } = Default.MinHeight;
        public double? MaxHeight { get; set; } = Default.MaxHeight;
        
        #endregion
        
        #region Panel Control Style Properties
        
        public Thickness? PanelControlMargin { get; set; } = Default.PanelControlMargin;
        public Thickness? PanelControlPadding { get; set; } = Default.PanelControlPadding;
        public HorizontalAlignment? PanelControlHorizontalAlignment { get; set; } = Default.PanelControlHorizontalAlignment;
        public VerticalAlignment? PanelControlVerticalAlignment { get; set; } = Default.PanelControlVerticalAlignment;
        public Thickness? PanelControlBorderThickness { get; set; } = Default.PanelControlBorderThickness;
        public SolidColorBrush? PanelControlBorderColor { get; set; } = Default.PanelControlBorderColor;
        public double? PanelControlWidth { get; set; } = null;
        public double? PanelControlMinWidth { get; set; } = null;
        public double? PanelControlMaxWidth { get; set; } = null;
        public double? PanelControlHeight { get; set; } = null;
        public double? PanelControlMinHeight { get; set; } = null;
        public double? PanelControlMaxHeight { get; set; } = null;
        
        #endregion
        
        #region Value Control Style Properties
        
        public Thickness? ValueControlMargin { get; set; } = Default.ValueControlMargin;
        public Thickness? ValueControlPadding { get; set; } = Default.ValueControlPadding;
        public FontWeight? ValueControlFontWeight { get; set; } = Default.ValueControlFontWeight;
        public FontFamily? ValueControlFontFamily { get; set; } = Default.ValueControlFontFamily;
        public double? ValueControlFontSize { get; set; } = Default.ValueControlFontSize;
        public SolidColorBrush? ValueControlForeground { get; set; } = Default.ValueControlForeground;
        public SolidColorBrush? ValueControlBackground { get; set; } = Default.ValueControlBackground;
        public HorizontalAlignment? ValueControlHorizontalAlignment { get; set; } = Default.ValueControlHorizontalAlignment;
        public VerticalAlignment? ValueControlVerticalAlignment { get; set; } = Default.ValueControlVerticalAlignment;
        public double? ValueControlWidth { get; set; } = null;
        public double? ValueControlMinWidth { get; set; } = null;
        public double? ValueControlMaxWidth { get; set; } = null;
        public double? ValueControlHeight { get; set; } = null;
        public double? ValueControlMinHeight { get; set; } = null;
        public double? ValueControlMaxHeight { get; set; } = null;
        
        #endregion
        
        #region Label Control Style Properties
        
        public Thickness? LabelControlMargin { get; set; } = Default.LabelControlMargin;
        public Thickness? LabelControlPadding { get; set; } = Default.LabelControlPadding;
        public FontWeight? LabelControlFontWeight { get; set; } = Default.LabelControlFontWeight;
        public FontFamily? LabelControlFontFamily { get; set; } = Default.LabelControlFontFamily;
        public double? LabelControlFontSize { get; set; } = Default.LabelControlFontSize;
        public SolidColorBrush? LabelControlForeground { get; set; } = Default.LabelControlForeground;
        public SolidColorBrush? LabelControlBackground { get; set; } = Default.LabelControlBackground;
        public HorizontalAlignment? LabelControlHorizontalAlignment { get; set; } = Default.LabelControlHorizontalAlignment;
        public VerticalAlignment? LabelControlVerticalAlignment { get; set; } = Default.LabelControlVerticalAlignment;
        public double? LabelControlWidth { get; set; } = null;
        public double? LabelControlMinWidth { get; set; } = null;
        public double? LabelControlMaxWidth { get; set; } = null;
        public double? LabelControlHeight { get; set; } = null;
        public double? LabelControlMinHeight { get; set; } = null;
        public double? LabelControlMaxHeight { get; set; } = null;
        
        #endregion
        
        #region Button Control Style Properties
        
        public Thickness? ButtonControlMargin { get; set; } = Default.ButtonControlMargin;
        public Thickness? ButtonControlPadding { get; set; } = Default.ButtonControlPadding;
        public FontWeight? ButtonControlFontWeight { get; set; } = Default.ButtonControlFontWeight;
        public FontFamily? ButtonControlFontFamily { get; set; } = Default.ButtonControlFontFamily;
        public double? ButtonControlFontSize { get; set; } = Default.ButtonControlFontSize;
        public SolidColorBrush? ButtonControlForeground { get; set; } = Default.ButtonControlForeground;
        public SolidColorBrush? ButtonControlBackground { get; set; } = Default.ButtonControlBackground;
        public HorizontalAlignment? ButtonControlHorizontalAlignment { get; set; } = Default.ButtonControlHorizontalAlignment;
        public VerticalAlignment? ButtonControlVerticalAlignment { get; set; } = Default.ButtonControlVerticalAlignment;
        public Thickness? ButtonControlBorderThickness { get; set; } = Default.ButtonControlBorderThickness;
        public SolidColorBrush? ButtonControlBorderColor { get; set; } = Default.ButtonControlBorderColor;
        public double? ButtonControlWidth { get; set; } = null;
        public double? ButtonControlMinWidth { get; set; } = null;
        public double? ButtonControlMaxWidth { get; set; } = null;
        public double? ButtonControlHeight { get; set; } = null;
        public double? ButtonControlMinHeight { get; set; } = null;
        public double? ButtonControlMaxHeight { get; set; } = null;
        
        #endregion

        #region Alert Style Properties

        public SolidColorBrush? AlertBackground { get; set; } = ThemeManager.GetBrush("ABrush.AlertTone2");
        public SolidColorBrush? AlertForeground { get; set; } = ThemeManager.GetBrush("ABrush.AlertTone5");
        public SolidColorBrush? AlertBorderBrush { get; set; } = ThemeManager.GetBrush("ABrush.AlertTone3");

        #endregion

        #region Reset Defaults

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
            Width = null;
            MinWidth = null;
            MaxWidth = null;
            Height = null;
            MinHeight = null;
            MaxHeight = null;
            
            // Panel Control Styles
            PanelControlMargin = Default.PanelControlMargin;
            PanelControlPadding = Default.PanelControlPadding;
            PanelControlHorizontalAlignment = Default.PanelControlHorizontalAlignment;
            PanelControlVerticalAlignment = Default.PanelControlVerticalAlignment;
            PanelControlBorderThickness = Default.PanelControlBorderThickness;
            PanelControlBorderColor = Default.PanelControlBorderColor;
            PanelControlWidth = null;
            PanelControlMinWidth = null;
            PanelControlMaxWidth = null;
            PanelControlHeight = null;
            PanelControlMinHeight = null;
            PanelControlMaxHeight = null;
            
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
            ValueControlWidth = null;
            ValueControlMinWidth = null;
            ValueControlMaxWidth = null;
            ValueControlHeight = null;
            ValueControlMinHeight = null;
            ValueControlMaxHeight = null;
            
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
            LabelControlWidth = null;
            LabelControlMinWidth = null;
            LabelControlMaxWidth = null;
            LabelControlHeight = null;
            LabelControlMinHeight = null;
            LabelControlMaxHeight = null;
            
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
            ButtonControlWidth = null;
            ButtonControlMinWidth = null;
            ButtonControlMaxWidth = null;
            ButtonControlHeight = null;
            ButtonControlMinHeight = null;
            ButtonControlMaxHeight = null;
        }

        #endregion
    }
}