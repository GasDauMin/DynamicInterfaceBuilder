using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Constants;

namespace DynamicInterfaceBuilder.Core.Models
{
    [ExtendedProperties]
    public class AdvancedProperties
    {
        public bool AutoLoadConfig { get; set; } = Default.AutoLoadConfig;
        public bool AutoSaveConfig { get; set; } = Default.AutoSaveConfig;
        public bool AdjustLabels { get; set; } = Default.AdjustLabels;
        public bool ReverseButtons { get; set; } = Default.ReverseButtons;
        public bool AllowResize { get; set; } = Default.AllowResize;
        public int MaxMessageLines { get; set; } = Default.MaxMessageLines;

        public void ResetDefaults()
        {
            AutoLoadConfig = Default.AutoLoadConfig;
            AutoSaveConfig = Default.AutoSaveConfig;
            AdjustLabels = Default.AdjustLabels;
            ReverseButtons = Default.ReverseButtons;
            AllowResize = Default.AllowResize;
            MaxMessageLines = Default.MaxMessageLines;
        }
    }
}