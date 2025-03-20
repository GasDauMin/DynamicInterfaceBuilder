using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Reflection;
using System.Linq;

namespace TheToolkit
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigPropertyAttribute : Attribute
    {
    }

    public class DynamicInterfaceBuilder
    {
        #region Constants

        const string DefaultTitle = "Dynamic Interface Builder";
        const int DefaultWidth = 800;
        const int DefaultHeight = 600;
        const string DefaultTheme = "Default";
        const string DefaultConfigFile = "DynamicInterfaceBuilder.properties.json";
        
        #endregion

        #region Properties

        [ConfigProperty]
        public required string Title { get; set; }
        
        [ConfigProperty]
        public required int Width { get; set; }
        
        [ConfigProperty]
        public required int Height { get; set; }
        
        [ConfigProperty]
        public required string Theme { get; set; }

        [ConfigProperty]
        public string ConfigFile { get; set; } 

        [ConfigProperty]
        public string ConfigPath { get; set; }

        protected string assemblyLocation;
        protected string assemblyDirectory;

        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, object> Results { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> Themes { get; set; } = new Dictionary<string, object>();

        #endregion

        #region Constructors
        public DynamicInterfaceBuilder()
            : this(DefaultTitle, DefaultWidth, DefaultHeight, DefaultTheme)
        {
        }

        public DynamicInterfaceBuilder(string title = DefaultTitle, int width = DefaultWidth, int height = DefaultHeight, string theme = DefaultTheme)
        {
            Title = title;
            Width = width;
            Height = height;
            Theme = theme;
            
            assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            assemblyDirectory = Path.GetDirectoryName(assemblyLocation) ?? AppDomain.CurrentDomain.BaseDirectory;

            ConfigFile = DefaultConfigFile;
            ConfigPath = Path.Combine(assemblyDirectory, ConfigFile);

            LoadThemes();
        }

        #endregion

        #region Configurations

        public void ResetToDefaults()
        {
            Title = DefaultTitle;
            Width = DefaultWidth;
            Height = DefaultHeight;
            Theme = DefaultTheme;
            
            Parameters.Clear();
        }

        public void LoadConfiguration(string configPath = DefaultConfigFile)
        {
            if (File.Exists(ConfigPath))
            {
                string json = File.ReadAllText(ConfigPath);
                Dictionary<string, object>? config = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                
                if (config != null)
                {
                    var properties = GetType().GetProperties()
                        .Where(p => p.GetCustomAttribute<ConfigPropertyAttribute>() != null);
                    
                    foreach (var property in properties)
                    {
                        if (config.TryGetValue(property.Name, out object? value) && value != null)
                        {
                            // Convert JToken or primitive type to property type
                            var convertedValue = Convert.ChangeType(value, property.PropertyType);
                            property.SetValue(this, convertedValue);
                        }
                    }
                }
            }
        }
        
        public void SaveConfiguration(string configPath = DefaultConfigFile)
        {
            var configValues = new Dictionary<string, object>();
            
            var properties = GetType().GetProperties()
                .Where(p => p.GetCustomAttribute<ConfigPropertyAttribute>() != null);
            
            foreach (var property in properties)
            {
                object? value = property.GetValue(this);
                if (value != null)
                {
                    configValues[property.Name] = value;
                }
            }

            string json = JsonConvert.SerializeObject(configValues, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(ConfigPath, json);
        }

        #endregion

        #region Theme management

        public void LoadThemes()
        {
            Themes = new Dictionary<string, object>
            {
                ["Default"] = new Dictionary<string, object>() {
                    { "Background", "#303030" },
                    { "Foreground", "#FFFFFF" },
                    { "Panel", "#454545" },
                    { "ControlBack", "#606060" },
                    { "ControlFore", "#FFFFFF" },
                    { "ButtonBack", "#606060" },
                    { "ButtonFore", "#FFFFFF" },
                    { "ButtonHover", "#808080" },
                    { "Description", "#D3D3D3" },
                    { "Error", "#E81123" },
                    { "Success", "#107C10" },
                    { "Warning", "#FF8C00" }
                }
            };

            string rootThemePath = Path.Combine(assemblyDirectory, "Themes");
            Debug.WriteLine($"Root theme path: {rootThemePath}");
            if (Directory.Exists(rootThemePath))
            {
                string[] themeFiles = Directory.GetFiles(rootThemePath, "*.json");
                foreach (string themeFile in themeFiles)
                {
                    string themeName = Path.GetFileNameWithoutExtension(themeFile);
                    string themeContent = File.ReadAllText(themeFile);
                    try 
                    {
                        var theme = JsonConvert.DeserializeObject<Dictionary<string, object>>(themeContent);
                        if (theme != null)
                        {
                            Themes[themeName] = theme;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error loading theme {themeName}: {ex.Message}");
                    }
                }
            }
        }

        private bool IsDarkMode()
        {
            Debug.WriteLine("Checking system dark mode setting...");
            bool ret = true;

            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    Debug.WriteLine($"Registry key opened: {key != null}");
                    if (key != null)
                    {
                        object? value = key.GetValue("AppsUseLightTheme");
                        Debug.WriteLine($"Theme value: {value}");
                        if (value != null && value is int intValue && intValue == 1)
                        {
                            ret = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking dark mode: {ex.Message}");
            }

            Debug.WriteLine($"Dark mode result: {ret}");
            return ret;
        }

        #endregion
    }   
}