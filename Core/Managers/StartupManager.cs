using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using DynamicInterfaceBuilder.Core.Constants;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Managers
{
    public class StartupManager : AppBase
    {
        public string StartupSettingsFile { get; set; }
        public string StartupSettingsPath { get; set; }

        public StartupManager(DynamicInterfaceBuilder.App application) : base(application)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string assemblyDirectory = Path.GetDirectoryName(assemblyLocation) ?? AppDomain.CurrentDomain.BaseDirectory;
            
            StartupSettingsFile = General.StartupSettingsFile;
            StartupSettingsPath = Path.Combine(assemblyDirectory, StartupSettingsFile);
            
            // Load settings at initialization
            LoadStartupSettings();
        }
        
        public void LoadStartupSettings()
        {
            LoadStartupSettings(StartupSettingsPath);
        }
        
        public void LoadStartupSettings(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    string json = File.ReadAllText(path);
                    var settings = JsonConvert.DeserializeObject<StartupSettingsData>(json);
                    
                    if (settings != null)
                    {
                        var app = (DynamicInterfaceBuilder.App)App;
                        
                        // Load auto settings
                        app.AdvancedProperties.AutoLoadConfig = settings.AutoLoadConfig;
                        app.AdvancedProperties.AutoSaveConfig = settings.AutoSaveConfig;
                        
                        // Load config path if provided
                        if (!string.IsNullOrEmpty(settings.ConfigPath))
                        {
                            app.ConfigPath = settings.ConfigPath;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading startup settings: {ex.Message}");
                }
            }
            else
            {
                SaveStartupSettings(path);
            }
        }
        
        public void SaveStartupSettings()
        {
            SaveStartupSettings(StartupSettingsPath);
        }
        
        public void SaveStartupSettings(string path)
        {
            try
            {
                var app = (DynamicInterfaceBuilder.App)App;
                
                var settings = new StartupSettingsData
                {
                    AutoLoadConfig = app.AdvancedProperties.AutoLoadConfig,
                    AutoSaveConfig = app.AdvancedProperties.AutoSaveConfig,
                    ConfigPath = app.ConfigPath
                };
                
                string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                
                // Make sure directory exists
                string? directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving startup settings: {ex.Message}");
            }
        }
        
        public void UpdateStartupSettings(bool? autoLoad = null, bool? autoSave = null, string? configPath = null)
        {
            var app = (DynamicInterfaceBuilder.App)App;
            
            if (autoLoad.HasValue)
                app.AdvancedProperties.AutoLoadConfig = autoLoad.Value;
                
            if (autoSave.HasValue)
                app.AdvancedProperties.AutoSaveConfig = autoSave.Value;
                
            if (configPath != null)
                app.ConfigPath = configPath;
                
            SaveStartupSettings();
        }
        
        private class StartupSettingsData
        {
            public bool AutoLoadConfig { get; set; } = Default.AutoLoadConfig;
            public bool AutoSaveConfig { get; set; } = Default.AutoSaveConfig;
            public string ConfigPath { get; set; } = General.ConfigPropertiesFile;
        }
    }
} 