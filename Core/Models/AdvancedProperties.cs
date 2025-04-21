using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Interfaces;
using DynamicInterfaceBuilder.Core.Constants;

namespace DynamicInterfaceBuilder.Core.Models
{
    [ExtendedProperties]
    public class AdvancedProperties : IProperties
    {
        public bool AdjustLabels { get; set; } = Default.AdjustLabels;
        public bool ReverseButtons { get; set; } = Default.ReverseButtons;
        public bool AllowResize { get; set; } = Default.AllowResize;
        public int MaxMessageLines { get; set; } = Default.MaxMessageLines;

        public void ResetDefaults()
        {
            AdjustLabels = Default.AdjustLabels;
            ReverseButtons = Default.ReverseButtons;
            AllowResize = Default.AllowResize;
            MaxMessageLines = Default.MaxMessageLines;
        }
    }
}