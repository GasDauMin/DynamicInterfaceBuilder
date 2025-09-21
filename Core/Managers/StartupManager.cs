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
            string workingDirectory = Directory.GetCurrentDirectory();
            
            StartupSettingsFile = Default.StartupSettingsFile;
            StartupSettingsPath = Path.Combine(workingDirectory, StartupSettingsFile);
            
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
                    var settings = JsonConvert.DeserializeObject<StartupProperties>(json);
                    
                    if (settings != null)
                    {
                        var app = (DynamicInterfaceBuilder.App)App;
                        
                        // Load auto settings
                        app.StartupProperties.AutoLoadConfig = settings.AutoLoadConfig;
                        app.StartupProperties.AutoSaveConfig = settings.AutoSaveConfig;
                        
                        // Load config path if provided
                        if (!string.IsNullOrEmpty(settings.ConfigPath))
                        {
                            app.StartupProperties.ConfigPath = settings.ConfigPath;
                        }
                        
                        // Load style path if provided
                        if (!string.IsNullOrEmpty(settings.StylePath))
                        {
                            app.StartupProperties.StylePath = settings.StylePath;
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
                
                var settings = new StartupProperties
                {
                    AutoLoadConfig = app.StartupProperties.AutoLoadConfig,
                    AutoSaveConfig = app.StartupProperties.AutoSaveConfig,
                    ConfigPath = app.StartupProperties.ConfigPath,
                    StylePath = app.StartupProperties.StylePath
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
        
        public void UpdateStartupSettings(bool? autoLoad = null, bool? autoSave = null, string? configPath = null, string? stylePath = null)
        {
            var app = (DynamicInterfaceBuilder.App)App;
            
            if (autoLoad.HasValue)
                app.StartupProperties.AutoLoadConfig = autoLoad.Value;
                
            if (autoSave.HasValue)
                app.StartupProperties.AutoSaveConfig = autoSave.Value;
                
            if (configPath != null)
                app.StartupProperties.ConfigPath = configPath;
                
            if (stylePath != null)
                app.StartupProperties.StylePath = stylePath;
                
            SaveStartupSettings();
        }
    }
} 