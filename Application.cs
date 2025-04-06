using System.Collections;
namespace DynamicInterfaceBuilder
{
    public class Application
    {
        #region Managers

        public MessageManager MessageManager;
        public ConfigManager ConfigManager;
        public ThemeManager ThemeManager;
        public ParametersManager ParametersManager;

        #endregion

        #region Properties

        public FormBuilder FormBuilder;
        public string? MessageText{get; set;} = string.Empty;
        public Dictionary<string, FormElementBase> FormElements { get; set; } = new();
        public Dictionary<string, object> Parameters { get; set; } = new();

        #endregion

        #region Properties [CP]

        [ConfigProperty]
        public required string Title { get; set; } = Constants.DefaultTitle;

        [ConfigProperty]
        public required int Width { get; set; } = Constants.DefaultWidth;
        
        [ConfigProperty]
        public required int Height { get; set; } = Constants.DefaultHeight;

        [ConfigProperty]
        public int Spacing { get; set; } = Constants.DefaultSpacing;

        [ConfigProperty]
        public string FontName { get; set; } = Constants.DefaultFontName;

        [ConfigProperty]
        public int FontSize { get; set; } = Constants.DefaultFontSize;

        [ConfigProperty]
        public AdvancedProperties AdvancedProperties { get; set; } = new();

        [ConfigProperty]    
        public required string Theme { 
            get => _theme;
            set {
                _theme = value;
                ThemeManager.SetTheme(_theme);
            }
        }

        private string _theme = Constants.DefaultTheme;

        #endregion

        #region Constructors

        public Application()
            : this(Constants.DefaultTitle, Constants.DefaultWidth, Constants.DefaultHeight, Constants.DefaultTheme)
        {
        }

        public Application(string title = Constants.DefaultTitle, int width = Constants.DefaultWidth, int height = Constants.DefaultHeight, string theme = Constants.DefaultTheme)
        {
            ConfigManager = new(this);
            ThemeManager = new(this);
            ParametersManager = new(this);
            MessageManager = new(this);

            FormBuilder = new(this);

            Title = title;
            Width = width;
            Height = height;
            Theme = theme;
        }

        #endregion

        #region Functions
    
        public void Run()
        {
            if (AdvancedProperties.AutoLoadConfig)
            {
                LoadConfiguration();
            }  

            ParametersManager.ParseParameters(Parameters);

            FormBuilder.Run();

            if (AdvancedProperties.AutoSaveConfig)
            {
                SaveConfiguration();
            }  
        }

        public bool Validate()
        {
            MessageText = string.Empty;
            return FormElements.Values.All(element => element.Control == null || element.ValidateControl());
        }

        public void ResetDefaults(bool save = false)
        {
            Parameters.Clear();

            Title = Constants.DefaultTitle;
            Width = Constants.DefaultWidth;
            Height = Constants.DefaultHeight;
            Spacing = Constants.DefaultSpacing;
            Theme = Constants.DefaultTheme;
            FontName = Constants.DefaultFontName;
            FontSize = Constants.DefaultFontSize;

            AdvancedProperties = new AdvancedProperties
            {
                FormBaseType = Constants.DefaultFormBaseType,
                ReverseButtons = Constants.DefaultReverseButtons,
                AllowResize = Constants.DefaultAllowResize,
                AdjustLabels = Constants.DefaultAdjustLabels,
                AutoLoadConfig = Constants.DefaultAutoLoadConfig,
                AutoSaveConfig = Constants.DefaultAutoSaveConfig,
                MaxMessageLines = Constants.DefaultMaxMessageLines,
            };
        }
        
        public void SaveConfiguration() => ConfigManager.SaveConfiguration(this);
        public void SaveConfiguration(string path) => ConfigManager.SaveConfiguration(this, path);
        public void LoadConfiguration() => ConfigManager.LoadConfiguration(this);
        public void LoadConfiguration(string path) => ConfigManager.LoadConfiguration(this, path); 
        public string FormatMessage(string message, MessageType type = MessageType.None) => MessageManager.FormatMessage(message, type);
        
        public System.Drawing.Color GetColor(string name) => ThemeManager.GetColor(name);
        public System.Windows.Media.Brush GetBrush(string name) => new System.Windows.Media.SolidColorBrush(ThemeManager.GetColorWpf(name));


        #endregion

        #region Main 

        [STAThread]
        static void Main()
        {
            // Standard WPF application initialization            
            var application = new Application()
            {
                Title = Constants.DefaultTitle,
                Width = Constants.DefaultWidth,
                Height = Constants.DefaultHeight,
                Theme = Constants.DefaultTheme
            };
            
            application.Parameters["InputFile"] = new Hashtable
            {
                { "Type", FormElementType.TextBox },
                { "Label", "Input file text" },
                { "Description", "The input file to process" },
                { "DefaultValue", "C:\\Users\\GasDauMin\\Downloads\\OllamaSetup.exe" },
                { "Required", true },
                { "Validation", new[]
                    {
                        new Hashtable { 
                            { "Type", "Required" },
                            { "Value", true },
                            { "Message", "Input file is required." }
                        },
                        new Hashtable {
                            { "Type", "Regex" },
                            { "Value", @"^[a-zA-Z0-9_\-]+\.txt$" },
                            { "Message", "Only .txt files are allowed." }
                        },
                        new Hashtable {
                            { "Type", "FileExists" },
                            { "Value", true },
                            { "Message", "File must exist."} 
                        }
                    }
                }
            };

            for(int i = 0; i < 15; i++)
            {
                application.Parameters[$"Test{i}"] = new Hashtable
                {
                    { "Type", FormElementType.TextBox },
                    { "Label", $"Testas #{i}" },
                    { "Description", "The input file to process" },
                    { "Required", false }
                };
            }

            application.Run();
            
            // Exit the application when the form is closed
            // application.Shutdown();
        }

        #endregion
    }
}