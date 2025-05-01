using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Interfaces;
using DynamicInterfaceBuilder.Core.Constants;

namespace DynamicInterfaceBuilder.Core.Models
{
    [ExtendedProperties]
    public class AdvancedProperties : IProperties
    {
        public bool AdjustLabels { get; set; }
        public bool ReverseButtons { get; set; }
        public bool AllowResize { get; set; }
        public int MaxMessageLines { get; set; }

        public AdvancedProperties()
        {
            Init();
        }

        public void Init()
        {
            AdjustLabels = Default.AdjustLabels;
            ReverseButtons = Default.ReverseButtons;
            AllowResize = Default.AllowResize;
            MaxMessageLines = Default.MaxMessageLines;
        }
    }
}