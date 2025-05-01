using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Interfaces;
using DynamicInterfaceBuilder.Core.Constants;

namespace DynamicInterfaceBuilder.Core.Models
{
    [ExtendedProperties]
    public class StartupProperties : IProperties
    {
        public string ConfigPath { get; set; }
        
        public bool AutoLoadConfig { get; set; }
        public bool AutoSaveConfig { get; set; }

        public StartupProperties()
        {
            Init();

            ConfigPath = General.ConfigPropertiesFile;
        }

        public void Init()
        {
            ConfigPath = General.ConfigPropertiesFile;
            AutoLoadConfig = Default.AutoLoadConfig;
            AutoSaveConfig = Default.AutoSaveConfig;
        }
    }
}