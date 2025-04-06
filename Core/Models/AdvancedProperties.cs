namespace DynamicInterfaceBuilder
{
    public class AdvancedProperties
    {
        [ConfigProperty]
        public FormBaseType FormBaseType { get; set; } = Constants.DefaultFormBaseType;

        [ConfigProperty]
        public bool AutoLoadConfig { get; set; } = Constants.DefaultAutoLoadConfig;

        [ConfigProperty]
        public bool AutoSaveConfig { get; set; } = Constants.DefaultAutoSaveConfig;

        [ConfigProperty]
        public bool AdjustLabels { get; set; } = Constants.DefaultAdjustLabels;

        [ConfigProperty]
        public bool ReverseButtons { get; set; } = Constants.DefaultReverseButtons;

        [ConfigProperty]
        public bool AllowResize { get; set; } = Constants.DefaultAllowResize;

        [ConfigProperty]
        public int MaxMessageLines { get; set; } = Constants.DefaultMaxMessageLines;
    }
}