using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DynamicInterfaceBuilder.Core.Models;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Constants;
using DynamicInterfaceBuilder.Core.Helpers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace DynamicInterfaceBuilder.Core.Managers
{
    public class ConfigManager : AppBase
    {        
        public ConfigManager(App application) : base(application)
        {
        }
        
        public void SaveConfiguration(object target)
        {
            var app = (DynamicInterfaceBuilder.App)App;
            var configPath = GetFullConfigPath(app);
            SaveConfiguration(target, configPath);
        }

        public void SaveConfiguration(object target, string path)
        {
            var configValues = new Dictionary<string, object>();
            SaveConfigProperties(target, configValues);
            
            string json = JsonConvert.SerializeObject(configValues, Formatting.Indented);
            
            // Make sure directory exists
            string? directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            File.WriteAllText(path, json);
        }
        
        private void SaveConfigProperties(object target, Dictionary<string, object> configValues)
        {
            var allProperties = target.GetType().GetProperties();
            
            bool hasConfigPropertyAttributes = allProperties.Any(p => 
                p.GetCustomAttribute<ConfigPropertyAttribute>() != null);
            
            var properties = hasConfigPropertyAttributes 
                ? allProperties.Where(p => p.GetCustomAttribute<ConfigPropertyAttribute>() != null)
                : allProperties;
            
            foreach (var property in properties)
            {
                object? value = property.GetValue(target);
                if (value != null)
                {
                    if (IsComplexType(property.PropertyType))
                    {
                        // Handle complex types recursively
                        var nestedValues = new Dictionary<string, object>();
                        SaveConfigProperties(value, nestedValues);
                        configValues[property.Name] = nestedValues;
                    }
                    else
                    {
                        configValues[property.Name] = value;
                    }
                }
            }
        }

        public void LoadConfiguration(object target)
        {
            var app = (DynamicInterfaceBuilder.App)App;
            var configPath = GetFullConfigPath(app);
            LoadConfiguration(target, configPath);
        }

        public void LoadConfiguration(object target, string path)
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                var config = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                
                if (config != null)
                {
                    LoadConfigProperties(target, config);
                }
            }
        }
        
        private void LoadConfigProperties(object target, Dictionary<string, object> config)
        {
            var allProperties = target.GetType().GetProperties();
            
            bool hasConfigPropertyAttributes = allProperties.Any(p => 
                p.GetCustomAttribute<ConfigPropertyAttribute>() != null);
            
            var properties = hasConfigPropertyAttributes 
                ? allProperties.Where(p => p.GetCustomAttribute<ConfigPropertyAttribute>() != null)
                : allProperties;
            
            foreach (var property in properties)
            {
                if (config.TryGetValue(property.Name, out object? value) && value != null)
                {
                    if (value is JObject jObject)
                    {
                        // Handle complex types
                        var nestedObj = property.GetValue(target);
                        if (nestedObj == null)
                        {
                            nestedObj = Activator.CreateInstance(property.PropertyType);
                            property.SetValue(target, nestedObj);
                        }
                        
                        // Convert JObject to Dictionary for recursive processing
                        var nestedConfig = jObject.ToObject<Dictionary<string, object>>();
                        if (nestedConfig != null && nestedObj != null)
                        {
                            LoadConfigProperties(nestedObj, nestedConfig);
                        }
                    }
                    else if (property.PropertyType.IsEnum)
                    {
                        var enumValue = Enum.ToObject(property.PropertyType, value);
                        property.SetValue(target, enumValue);
                    }
                    else
                    {
                        // Use the helper to convert and set the property value
                        object? convertedValue = TypeConversionHelper.ConvertValueToType(value, property.PropertyType);
                        if (convertedValue != null)
                        {
                            property.SetValue(target, convertedValue);
                        }
                    }
                }
            }
        }

        private bool IsComplexType(Type type)
        {
            return type.GetCustomAttribute<ExtendedPropertiesAttribute>() != null;
        }
        
        // Helper to get the full path from AdvancedProperties.ConfigPath
        private string GetFullConfigPath(DynamicInterfaceBuilder.App app)
        {
            string configPath = app.StartupProperties.ConfigPath;
            
            // If the path is not absolute, make it relative to the assembly directory
            if (!Path.IsPathRooted(configPath))
            {
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                string assemblyDirectory = Path.GetDirectoryName(assemblyLocation) ?? AppDomain.CurrentDomain.BaseDirectory;
                configPath = Path.Combine(assemblyDirectory, configPath);
            }
            
            return configPath;
        }
    }
}