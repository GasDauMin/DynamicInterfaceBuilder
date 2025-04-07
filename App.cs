using System.Collections;
using System.Windows;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Constants;
using DynamicInterfaceBuilder.Core.Form.Enums;
using DynamicInterfaceBuilder.Core.Form.Helpers;
using DynamicInterfaceBuilder.Core.Form.Structure;
using DynamicInterfaceBuilder.Core.Helpers;
using DynamicInterfaceBuilder.Core.Managers;
using DynamicInterfaceBuilder.Core.Models;
using DynamicInterfaceBuilder.Core.UI.Enums;

namespace DynamicInterfaceBuilder
{
    public class App
    {
        #region Managers

        public ConfigManager ConfigManager;
        public ParametersManager ParametersManager;

        #endregion

        #region Helpers
    
        public WpfHelper WpfHelper;
        public WindowsHelper WindowsHelper;
        public MessageHelper MessageHelper;
        public ThemesManager ThemesManager;
        public ValidationHelper ValidationHelper;

        #endregion

        #region Properties

        public Application Application;
        public FormBuilder FormBuilder;
        public string? MessageText{get; set;} = string.Empty;
        public MessageType MessageType { get; set; } = MessageType.None;
        public Dictionary<string, FormElementBase> FormElements { get; set; } = new();
        public Dictionary<string, object> Parameters { get; set; } = new();

        #endregion

        #region Properties [CP]

        [ConfigProperty]
        public required string Title { get; set; } = Default.Title;

        [ConfigProperty]
        public required int Width { get; set; } = Default.Width;
        
        [ConfigProperty]
        public required int Height { get; set; } = Default.Height;

        [ConfigProperty]
        public int Spacing { get; set; } = Default.Spacing;

        [ConfigProperty]
        public string FontName { get; set; } = Default.FontName;

        [ConfigProperty]
        public int FontSize { get; set; } = Default.FontSize;

        [ConfigProperty]
        public ThemeType Theme { get; set; } = Default.Theme;

        [ConfigProperty]
        public AdvancedProperties AdvancedProperties { get; set; } = new();

        #endregion

        #region Constructors

        public App()
            : this(Default.Title, Default.Width, Default.Height)
        {
        }

        public App(string title = Default.Title, int width = Default.Width, int height = Default.Height)
        {
            Application = new System.Windows.Application();

            ConfigManager       = new(this);
            ParametersManager   = new(this);
            ThemesManager       = new(this);

            MessageHelper       = new(this);
            ValidationHelper    = new(this);
            WindowsHelper       = new(this);
            WpfHelper           = new(this);

            FormBuilder         = new(this);

            Title = title;
            Width = width;
            Height = height;

            ThemesManager.SetTheme(Theme);
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
            MessageHelper.ResetMessage();

            bool ok = true;

            foreach (var element in FormElements.Values)
            {
                if (element.ValueControl != null && !element.ValidateControl())
                {
                    ok = false;
                }
            }

            return ok;
        }

        public void ResetDefaults(bool save = false)
        {
            Parameters.Clear();

            Title = Default.Title;
            Width = Default.Width;
            Height = Default.Height;
            Spacing = Default.Spacing;
            FontName = Default.FontName;
            FontSize = Default.FontSize;

            AdvancedProperties = new AdvancedProperties
            {
                ReverseButtons = Default.ReverseButtons,
                AllowResize = Default.AllowResize,
                AdjustLabels = Default.AdjustLabels,
                AutoLoadConfig = Default.AutoLoadConfig,
                AutoSaveConfig = Default.AutoSaveConfig,
                MaxMessageLines = Default.MaxMessageLines,
            };
        }
        
        public void SaveConfiguration() => ConfigManager.SaveConfiguration(this);
        public void SaveConfiguration(string path) => ConfigManager.SaveConfiguration(this, path);
        public void LoadConfiguration() => ConfigManager.LoadConfiguration(this);
        public void LoadConfiguration(string path) => ConfigManager.LoadConfiguration(this, path); 
        
        #endregion

        #region Main 

        [STAThread]
        static void Main()
        {       
            var application = new App()
            {
                Title = Default.Title,
                Width = Default.Width,
                Height = Default.Height
            };
            
            application.Parameters["InputFile"] = new Hashtable
            {
                { "Type", FormElementType.FileBox },
                { "Label", "Input file text" },
                { "Description", "The input file to process" },
                { "DefaultValue", "C:\\Users\\GasDauMin\\Downloads\\OllamaSetup.exe" },
                { "Required", true },
                { "Validation", new[]
                    {
                        new Hashtable { 
                            { "Type", FormElementValidationType.Required },
                            { "Value", true },
                            { "Message", "Input file is required." }
                        },
                        new Hashtable {
                            { "Type", FormElementValidationType.Regex },
                            { "Value", @"^(.*)\.exe$" },
                            { "Message", "Only .exe files are allowed." }
                        },
                        new Hashtable {
                            { "Type", FormElementValidationType.FileExists },
                            { "Value", false },
                            { "Message", "File already exists."} 
                        }
                    }
                }
            };

            for(int i = 0; i < 5; i++)
            {
                var item = new Hashtable
                {
                    { "Type", FormElementType.TextBox },
                    { "Label", $"Testas #{i}" },
                    { "Description", "The input file to process" },
                    { "Required", false }
                };

                if (i % 2 == 0)
                {
                    item["Validation"] = new[] { 
                        new Hashtable {
                            { "Type", "Required" },
                            { "Value", true },
                            { "Message", $" \"Testas #{i}\" is required." }
                        }
                    };
                }

                application.Parameters[$"Test{i}"] = item;
            }

            application.Run();
            
            // Exit the application when the form is closed
            // application.Shutdown();
        }

        #endregion
    }
}