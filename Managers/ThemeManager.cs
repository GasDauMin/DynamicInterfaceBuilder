using System.Diagnostics;
using Newtonsoft.Json;

namespace DynamicInterfaceBuilder
{
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
}