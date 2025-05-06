using System.Windows;
using System.Windows.Media;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Interfaces;
using DynamicInterfaceBuilder.Core.Constants;
using DynamicInterfaceBuilder.Core.Managers;
using System.Reflection.Emit;
using System.Windows.Controls;

namespace DynamicInterfaceBuilder.Core.Models
{
    [ExtendedProperties]
    public class StyleProperties : IProperties
    {
        #region General Style Properties
        
        public int Spacing { get; set; }
        public FontWeight FontWeight { get; set; }
        public FontFamily FontFamily { get; set; }
        public double FontSize { get; set; }
        public HorizontalAlignment? HorizontalAlignment { get; set; }
        public VerticalAlignment? VerticalAlignment { get; set; }
        public string? BorderColor { get; set; }
        public Thickness? BorderThickness { get; set; }
        public Thickness? Margin { get; set; }
        public Thickness? Padding { get; set; }
        public double? Width { get; set; }
        public double? MinWidth { get; set; }
        public double? MaxWidth { get; set; }
        public double? Height { get; set; }
        public double? MinHeight { get; set; }
        public double? MaxHeight { get; set; }
        
        #endregion

        #region Group Style Properties
        
        public Orientation GroupOrientation { get; set; }
        public bool GroupShowHeader { get; set; }
        public bool GroupShowBorder { get; set; }
        public bool GroupEnableVerticalScroll { get; set; }
        public bool GroupEnableHorizontalScroll { get; set; }
        public double GroupBorderThickness { get; set; }
        public CornerRadius GroupCornerRadius { get; set; }
        public double GroupSpacing { get; set; }
        public FontWeight GroupFontWeight { get; set; }
        public FontFamily GroupFontFamily { get; set; }
        public double GroupFontSize { get; set; }
        public Thickness? GroupMargin { get; set; }
        public Thickness? GroupPadding { get; set; }
        public double GroupMaxWidth { get; set; }
        public double GroupMinWidth { get; set; }
        public double GroupMaxHeight { get; set; }
        public double GroupMinHeight { get; set; }
        public string? GroupBorderColor { get; set; }
        public string? GroupHeaderBackground { get; set; }
        public string? GroupHeaderForeground { get; set; }
        public string? GroupBackground { get; set; }
        
        #endregion

        #region Alert Style Properties

        public string? AlertBackground { get; set; }
        public string? AlertForeground { get; set; }
        public string? AlertBorderColor { get; set; }

        #endregion
        
        #region Controls Style Properties

        public StyleControlProperties? PanelControl { get; set; }
        public StyleControlProperties? ValueControl { get; set; }
        public StyleControlProperties? LabelControl { get; set; }
        public StyleControlProperties? ButtonControl { get; set; }
        
        #endregion

        #region Constructor
        
        public StyleProperties()
        {
            Init();

            FontFamily = Default.FontFamily;
            GroupFontFamily = Default.FontFamily;
        }
        
        #endregion

        #region Initialize

        public void Init()
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
            
            // Group Element Styles
            GroupOrientation = Default.GroupOrientation;
            GroupShowHeader = Default.GroupShowHeader;
            GroupShowBorder = Default.GroupShowBorder;
            GroupEnableVerticalScroll = Default.GroupEnableVerticalScroll;
            GroupEnableHorizontalScroll = Default.GroupEnableHorizontalScroll;
            GroupBorderThickness = Default.GroupBorderThickness;
            GroupCornerRadius = Default.GroupCornerRadius;
            GroupSpacing = Default.GroupSpacing;
            GroupFontWeight = Default.GroupFontWeight;
            GroupFontFamily = Default.GroupFontFamily;
            GroupFontSize = Default.GroupFontSize;

            GroupMargin = Default.GroupMargin;
            GroupPadding = Default.GroupPadding;
            GroupBorderColor = Default.GroupBorderColor;
            GroupHeaderBackground = Default.GroupHeaderBackground;
            GroupHeaderForeground = Default.GroupHeaderForeground;
            GroupBackground = Default.GroupBackground;
            GroupMaxWidth = Default.GroupMaxWidth;
            GroupMaxHeight = Default.GroupMaxHeight;
            GroupMinWidth = Default.GroupMinWidth;
            GroupMinHeight = Default.GroupMinHeight;
            
            // Alert Styles
            AlertBackground = Default.AlertBackground;
            AlertForeground = Default.AlertForeground;
            AlertBorderColor = Default.AlertBorderColor;

            // Controls Styles
            PanelControl = new StyleControlProperties
            {
                Margin = Default.PanelControlMargin,
                Padding = Default.PanelControlPadding,
                Foreground = Default.PanelControlForeground,
                Background = Default.PanelControlBackground,
                HorizontalAlignment = Default.PanelControlHorizontalAlignment,
                VerticalAlignment = Default.PanelControlVerticalAlignment,
                BorderThickness = Default.PanelControlBorderThickness,
                BorderColor = Default.PanelControlBorderColor,
                Width = null,
                MinWidth = null,
                MaxWidth = null,
                Height = null,
                MinHeight = null,
                MaxHeight = null
            };
            
            ValueControl = new StyleControlProperties
            {
                Margin = Default.ValueControlMargin,
                Padding = Default.ValueControlPadding,
                FontWeight = Default.ValueControlFontWeight,
                FontFamily = Default.ValueControlFontFamily,
                FontSize = Default.ValueControlFontSize,
                Foreground = Default.ValueControlForeground,
                Background = Default.ValueControlBackground,
                HorizontalAlignment = Default.ValueControlHorizontalAlignment,
                VerticalAlignment = Default.ValueControlVerticalAlignment,
                Width = null,
                MinWidth = null,
                MaxWidth = null,
                Height = null,
                MinHeight = null,
                MaxHeight = null
            };
            
            LabelControl = new StyleControlProperties
            {
                Margin = Default.LabelControlMargin,
                Padding = Default.LabelControlPadding,
                FontWeight = Default.LabelControlFontWeight,
                FontFamily = Default.LabelControlFontFamily,
                FontSize = Default.LabelControlFontSize,
                Foreground = Default.LabelControlForeground,
                Background = Default.LabelControlBackground,
                HorizontalAlignment = Default.LabelControlHorizontalAlignment,
                VerticalAlignment = Default.LabelControlVerticalAlignment,
                Width = null,
                MinWidth = null,
                MaxWidth = null,
                Height = null,
                MinHeight = null,
                MaxHeight = null
            };

            ButtonControl = new StyleControlProperties
            {
                Margin = Default.ButtonControlMargin,
                Padding = Default.ButtonControlPadding,
                FontWeight = Default.ButtonControlFontWeight,
                FontFamily = Default.ButtonControlFontFamily,
                FontSize = Default.ButtonControlFontSize,
                Foreground = Default.ButtonControlForeground,
                Background = Default.ButtonControlBackground,
                HorizontalAlignment = Default.ButtonControlHorizontalAlignment,
                VerticalAlignment = Default.ButtonControlVerticalAlignment,
                BorderThickness = Default.ButtonControlBorderThickness,
                BorderColor = Default.ButtonControlBorderColor,
                Width = null,
                MinWidth = null,
                MaxWidth = null,
                Height = null,
                MinHeight = null,
                MaxHeight = null
            };
        }

        #endregion
    }
}