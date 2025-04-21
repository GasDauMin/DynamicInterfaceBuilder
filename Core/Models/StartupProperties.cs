using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Interfaces;
using DynamicInterfaceBuilder.Core.Constants;

namespace DynamicInterfaceBuilder.Core.Models
{
    [ExtendedProperties]
    public class StartupProperties : IProperties
    {
        public bool AutoLoadConfig { get; set; } = Default.AutoLoadConfig;
        public bool AutoSaveConfig { get; set; } = Default.AutoSaveConfig;
        public string ConfigPath { get; set; } = General.ConfigPropertiesFile;

        public void ResetDefaults()
        {
            AutoLoadConfig = Default.AutoLoadConfig;
            AutoSaveConfig = Default.AutoSaveConfig;
            ConfigPath = General.ConfigPropertiesFile;
        }
    }
}