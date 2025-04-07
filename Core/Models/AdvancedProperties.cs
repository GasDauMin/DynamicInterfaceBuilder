using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Constants;

namespace DynamicInterfaceBuilder.Core.Models
{
    public class AdvancedProperties
    {
        [ConfigProperty]
        public bool AutoLoadConfig { get; set; } = Default.AutoLoadConfig;

        [ConfigProperty]
        public bool AutoSaveConfig { get; set; } = Default.AutoSaveConfig;

        [ConfigProperty]
        public bool AdjustLabels { get; set; } = Default.AdjustLabels;

        [ConfigProperty]
        public bool ReverseButtons { get; set; } = Default.ReverseButtons;

        [ConfigProperty]
        public bool AllowResize { get; set; } = Default.AllowResize;

        [ConfigProperty]
        public int MaxMessageLines { get; set; } = Default.MaxMessageLines;
    }
}