using System.Diagnostics;
using Newtonsoft.Json;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace TheToolkit
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigPropertyAttribute : Attribute
    {
    }

    public class ConfigManager
    {
        public string ConfigFile { get; set; } 
        public string ConfigPath { get; set; }

        public ConfigManager()
        {   
            string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string assemblyDirectory = Path.GetDirectoryName(assemblyLocation) ?? AppDomain.CurrentDomain.BaseDirectory;

            ConfigFile = DynamicInterfaceBuilder.DefaultConfigFile;
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

    public class ThemeManager
    {       
        public Dictionary<string, Dictionary<string, string>> Themes => _themes;
        public string RootThemePath { get; set; }

        private Dictionary<string, Dictionary<string, string>> _themes = new();
        private Dictionary<string, Color> _colors = new();

        public ThemeManager()
        {
            string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string assemblyDirectory = Path.GetDirectoryName(assemblyLocation) ?? AppDomain.CurrentDomain.BaseDirectory;

            RootThemePath = Path.Combine(assemblyDirectory, "Themes");

            LoadThemes();
        }

        public void SetTheme(string theme)
        {
            if (_themes.ContainsKey(theme))
            {
                _colors = _themes[theme].ToDictionary(kvp => kvp.Key, kvp => ColorTranslator.FromHtml(kvp.Value));
            }
        } 

        public void LoadThemes()
        {
            _themes.Clear();
            _themes["Default"] = new Dictionary<string, string>() {
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
            };

            Debug.WriteLine($"Root theme path: {RootThemePath}");
            if (Directory.Exists(RootThemePath))
            {
                string[] themeFiles = Directory.GetFiles(RootThemePath, "*.json");
                foreach (string themeFile in themeFiles)
                {
                    string themeName = Path.GetFileNameWithoutExtension(themeFile);
                    string themeContent = File.ReadAllText(themeFile);
                    try 
                    {
                        var theme = JsonConvert.DeserializeObject<Dictionary<string, string>>(themeContent);
                        if (theme != null)
                        {
                            _themes[themeName] = theme;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error loading theme {themeName}: {ex.Message}");
                    }
                }
            }
        }

        public System.Drawing.Color GetColor(string name)
        {      
            if (_colors.ContainsKey(name))
            {
                return _colors[name];
            }

            return Color.Black;
        }  
    }

    public class DynamicInterfaceBuilder
    {
        #region Constants

        public const string DefaultTheme = "Default";
        public const string DefaultTitle = "Dynamic Interface Builder";
        public const int DefaultWidth = 800;
        public const int DefaultHeight = 600;
        public const int DefaultFontSize = 12;
        public const int DefaultSpacing = 5;
        public const int DefaultMargin = 5;
        public const int DefaultPadding = 5;
        public const string DefaultConfigFile = "DynamicInterfaceBuilder.properties.json";
        
        #endregion

        #region Properties

        [ConfigProperty]
        public required string Title { get; set; }
        
        [ConfigProperty]
        public required int Width { get; set; }
        
        [ConfigProperty]
        public required int Height { get; set; }

        [ConfigProperty]
        public int FontSize { get; set; }

        [ConfigProperty]
        public int Margin { get; set; }
        
        [ConfigProperty]
        public int Padding { get; set; }

        [ConfigProperty]
        public int Spacing { get; set; }

        [ConfigProperty]    
        public required string Theme { 
            get => _theme;
            set {
                _theme = value;
                _themeManager.SetTheme(_theme);
            }
        }

        public Dictionary<string, string> Parameters { get; set; } = new();
        public Dictionary<string, object> Results { get; set; } = new();
        public Dictionary<string, Dictionary<string, string>> Themes { get; set; } = new();

        private ConfigManager _configManager;
        private ThemeManager _themeManager;
        private string _theme;
        
        #endregion

        #region Constructors
        public DynamicInterfaceBuilder()
            : this(DefaultTitle, DefaultWidth, DefaultHeight, DynamicInterfaceBuilder.DefaultTheme)
        {
        }

        public DynamicInterfaceBuilder(string title = DefaultTitle, int width = DefaultWidth, int height = DefaultHeight, string theme = DynamicInterfaceBuilder.DefaultTheme)
        {
            Title = title;
            Width = width;
            Height = height;

            Margin = DefaultMargin;
            Padding = DefaultPadding;
            Spacing = DefaultSpacing;
            FontSize = DefaultFontSize;

            // Initialize config manager

            _configManager = new ConfigManager();

            // Initialize theme manager

            _theme = theme;
            _themeManager = new ThemeManager();
            _themeManager.SetTheme(_theme);
        }

        #endregion

        #region Forms

        public DialogResult RunForm()
        {
            var form = BuildForm();
            return form.ShowDialog();
        }
        
        protected Form BuildForm()
        {
            Form form = new()
            {
                Text = Title,
                Width = Width,
                Height = Height,
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedSingle,
                MaximizeBox = false,
                MinimizeBox = true,
                Padding = new Padding(Margin), // Add form-level padding
                BackColor = GetThemeColor("Background"),
                ForeColor = GetThemeColor("Foreground")
            };

            var container = BuildContainer();

            
            
            form.Controls.Add(container);

            return form;
        }

        protected FlowLayoutPanel BuildContainer()
        {
            FlowLayoutPanel container = new()
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoScroll = true,
                Padding = new Padding(Padding), // Add padding inside flow layout
                Margin = new Padding(Margin), // Add margin around flow layout
                BackColor = GetThemeColor("Panel")
            };

            container.SetFlowBreak(container, true);
            container.Padding = new Padding(
                container.Padding.Left,
                container.Padding.Top,
                container.Padding.Right,
                container.Padding.Bottom + Spacing
            );

            return container;
        }

        #endregion

        #region Functions

        public void ResetDefaults()
        {
            Title = DefaultTitle;
            Width = DefaultWidth;
            Height = DefaultHeight;
            
            Parameters.Clear();
        }

        public Color GetThemeColor(string name) => _themeManager.GetColor(name);

        public void SaveConfiguration() => _configManager.SaveConfiguration(this);
        public void SaveConfiguration(string path) => _configManager.SaveConfiguration(this, path);
        public void LoadConfiguration() => _configManager.LoadConfiguration(this);
        public void LoadConfiguration(string path) => _configManager.LoadConfiguration(this, path); 

        #endregion
    }   
}