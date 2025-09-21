using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Interfaces;
using DynamicInterfaceBuilder.Core.Constants;

namespace DynamicInterfaceBuilder.Core.Models
{
    [ExtendedProperties]
    public class StartupProperties : IProperties
    {
        public string ConfigPath { get; set; }
        public string StylePath { get; set; }
        
        public bool AutoLoadConfig { get; set; }
        public bool AutoSaveConfig { get; set; }

        public StartupProperties()
        {
            Init();

            ConfigPath = Default.ConfigPropertiesFile;
            StylePath = Default.StyleFile;
        }

        public void Init()
        {
            ConfigPath = Default.ConfigPropertiesFile;
            StylePath = Default.StyleFile;
            AutoLoadConfig = Default.AutoLoadConfig;
            AutoSaveConfig = Default.AutoSaveConfig;
        }
    }
}