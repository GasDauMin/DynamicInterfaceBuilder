using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace DynamicInterfaceBuilder
{
    public class ThemeManager : ApplicationBase
    {   
        public Dictionary<string, Dictionary<string, string>> Themes => _themes;

        public string RootThemePath { get; set; }

        private Dictionary<string, Dictionary<string, string>> _themes = new();
        private Dictionary<string, System.Drawing.Color> _colors = new();

        public ThemeManager(Application application) : base(application)
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
                _colors = _themes[theme].ToDictionary(kvp => kvp.Key, kvp => System.Drawing.ColorTranslator.FromHtml(kvp.Value));
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
                { "Message.Background", "#606060" },
                { "Message.Foreground", "#FFFFFF" },
                { "Message.InfoBg", "#505050" },
                { "Message.InfoFg", "#FFFFFF" },
                { "Message.AlertBg", "#8B4513" },
                { "Message.AlertFg", "#FFFFFF" },
                { "Message.SuccessBg", "#006400" },
                { "Message.SuccessFg", "#FFFFFF" },
                { "Message.WarningBg", "#8B8000" },
                { "Message.WarningFg", "#FFFFFF" },
                { "Message.ErrorBg", "#8B0000" },
                { "Message.ErrorFg", "#FFFFFF" },
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
                        var themeObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(themeContent);
                        if (themeObject != null)
                        {
                            var flattenedTheme = new Dictionary<string, string>();
                            FlattenThemeDictionary(themeObject, "", flattenedTheme);
                            _themes[themeName] = flattenedTheme;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error loading theme {themeName}: {ex.Message}");
                    }
                }
            }
        }

        private void FlattenThemeDictionary(Dictionary<string, object> source, string prefix, Dictionary<string, string> result)
        {
            foreach (var kvp in source)
            {
                var key = string.IsNullOrEmpty(prefix) ? kvp.Key : $"{prefix}.{kvp.Key}";
                
                if (kvp.Value is Dictionary<string, object> nestedDict)
                {
                    // If the value is another dictionary, recursively flatten it
                    FlattenThemeDictionary(nestedDict, key, result);
                }
                else if (kvp.Value is Newtonsoft.Json.Linq.JObject jObject)
                {
                    // Handle JObject from JSON.NET
                    FlattenThemeDictionary(jObject.ToObject<Dictionary<string, object>>(), key, result);
                }
                else
                {
                    // For simple values, add them directly
                    result[key] = kvp.Value?.ToString() ?? "#000000";
                }
            }
        }

        public System.Drawing.Color GetColor(string name)
        {
            return _colors.TryGetValue(name, out var color) ? color : System.Drawing.Color.Black;
        }

        public System.Windows.Media.Color GetColorWpf(string name)
        {
            var color = GetColor(name);
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}