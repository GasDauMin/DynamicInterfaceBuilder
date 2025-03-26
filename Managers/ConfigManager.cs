using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicInterfaceBuilder
{
    public class ConfigManager
    {
        public string ConfigFile { get; set; } 
        public string ConfigPath { get; set; }

        public ConfigManager()
        {   
            string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string assemblyDirectory = Path.GetDirectoryName(assemblyLocation) ?? AppDomain.CurrentDomain.BaseDirectory;

            ConfigFile = Constants.DefaultConfigFile; // Fixed :: to . for C# syntax
            ConfigPath = Path.Combine(assemblyDirectory, ConfigFile);
        }
        
        public void SaveConfiguration(object target)
        {
            SaveConfiguration(target, ConfigPath);
        }

        public void SaveConfiguration(object target, string path)
        {
            var configValues = new Dictionary<string, object>();
            SaveConfigProperties(target, configValues);
            
            string json = JsonConvert.SerializeObject(configValues, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(path, json);
        }
        
        private void SaveConfigProperties(object target, Dictionary<string, object> configValues)
        {
            var properties = target.GetType().GetProperties()
                .Where(p => p.GetCustomAttribute<ConfigPropertyAttribute>() != null);
            
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
            LoadConfiguration(target, ConfigPath);
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
            var properties = target.GetType().GetProperties()
                .Where(p => p.GetCustomAttribute<ConfigPropertyAttribute>() != null);
            
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
                    else
                    {
                        // Handle primitive types
                        var convertedValue = Convert.ChangeType(value, property.PropertyType);
                        property.SetValue(target, convertedValue);
                    }
                }
            }
        }

        private bool IsComplexType(Type type)
        {
            return type != typeof(string) && !type.IsPrimitive && !type.IsEnum 
                && type != typeof(decimal) && type != typeof(DateTime)
                && !type.IsArray && type != typeof(Guid);
        }
    }
}