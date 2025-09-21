using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Constants;
using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Helpers;
using DynamicInterfaceBuilder.Core.Managers;
using DynamicInterfaceBuilder.Core.Models;
using Microsoft.VisualBasic;
using DynamicInterfaceBuilder.Styles.Enums;
using DynamicInterfaceBuilder.Core.Forms;

namespace DynamicInterfaceBuilder
{
    public class App
    {
        #region Managers

        public ConfigManager ConfigManager;
        public ParametersManager ParametersManager;
        public StartupManager StartupManager;

        #endregion

        #region Helpers
    
        public GeneralHelper WpfHelper;
        public WindowsHelper WindowsHelper;
        public MessageHelper MessageHelper;
        public ThemeManager ThemesManager;

        #endregion

        #region Properties

        public Application Application;
        public FormBuilder FormBuilder;
        public string? MessageText{get; set;} = string.Empty;
        public MessageType MessageType { get; set; } = MessageType.None;
        public Dictionary<string, FormElementBase> FormElements { get; set; } = new();
        public List<string> FormElementIds { get; set; } = new();
        public Dictionary<string, object> Parameters { get; set; } = new();
        public Dictionary<string, string>? ValidationErrors { get; set; } = new();
        public StartupProperties StartupProperties { get; set; } = new();

        #endregion

        #region Properties [CP]

        [ConfigProperty]
        public string? Title { get; set; } = Default.Title;

        [ConfigProperty]
        public int? Width { get; set; } = Default.Width;
        
        [ConfigProperty]
        public int? Height { get; set; } = Default.Height;

        [ConfigProperty]
        public Thickness? Margin { get; set; } = Default.Margin;

        [ConfigProperty]
        public Thickness? Padding { get; set; } = Default.Padding;

        [ConfigProperty]
        public ThemeType Theme { get; set; } = Default.Theme;

        [ConfigProperty]
        public AdvancedProperties AdvancedProperties { get; set; }

        [ConfigProperty]
        public string? Icon { get; set; } = Default.Icon;

        #endregion

        #region Constructors

        public App()
            : this(Default.Title)
        {
        }

        public App(string? title = null, int? width = null, int? height = null)
        {
            // Reuse existing Application instance or create new one if none exists
            Application = System.Windows.Application.Current ?? new System.Windows.Application();
        
            StartupManager = new(this);
            ConfigManager = new(this);
            ParametersManager = new(this);
            ThemesManager = new(this);

            AdvancedProperties = new AdvancedProperties();

            MessageHelper = new(this);
            WindowsHelper = new(this);
            WpfHelper = new(this);

            FormBuilder = new(this);

            Title ??= Default.Title;
            Width ??= Default.Width;
            Height ??= Default.Height;
        }

        #endregion

        #region Functions

        public void Run()
        {
            if (StartupProperties.AutoLoadConfig)
            {
                LoadConfiguration();
            }  

            ParametersManager.ParseParameters(Parameters);
            
            ThemeManager.SetTheme(Theme);

            FormBuilder.Run();

            if (StartupProperties.AutoSaveConfig)
            {
                SaveConfiguration();
            }

            StartupManager.UpdateStartupSettings();
        }

        public string SetElementId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Element ID cannot be null or empty.", nameof(id));
            }
            if (FormElementIds.Exists(existingId => existingId.Equals(id, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"Element ID '{id}' already exists.", nameof(id));
            }

            FormElementIds.Add(id);
            return id;
        }

        public string GenerateElementId(params string[] data)
        {
            List<String> baseList = new List<string>();
            if (data != null && data.Length > 0)
            {
                baseList.AddRange(data.Where(p => !string.IsNullOrEmpty(p)));
            }
            else
            {
                baseList.Add(Default.FieldDefaultPrefix);
            }

            string id, guid;
            int sequence = 0;

            do
            {
                var list = new List<string>(baseList);
                sequence++;
                if (sequence > 1)
                {
                    if (Default.FieldGuidEnabled)
                    {
                        guid = Default.FieldGuidSize > 0
                            ? Guid.NewGuid().ToString("N").Substring(0, Default.FieldGuidSize)
                            : Guid.NewGuid().ToString("N");
                        list.Add(guid);
                    }
                    else
                    {
                        if (Default.FieldSequenceSeparate)
                        {
                            list.Add(sequence.ToString());
                        }
                        else
                        {
                            list[^1] = $"{list[^1]}{sequence}"; //..append sequence to the last element
                        }
                    }
                }
                id = string.Join(Default.FieldPrefixSeparator, list);
            } while (FormElementIds.Exists(existingId => existingId.Equals(id, StringComparison.OrdinalIgnoreCase)));

            return id;
        }

        public void ResetDefaults()
        {
            Parameters.Clear();

            Title = Default.Title;
            Width = Default.Width;
            Height = Default.Height;
            Theme = Default.Theme;
            Icon = Default.Icon;

            StartupProperties.Init();
            AdvancedProperties.Init();
        }

        public void SaveConfiguration() => ConfigManager.SaveConfiguration(this);
        public void SaveConfiguration(string path) => ConfigManager.SaveConfiguration(this, path);
        public void LoadConfiguration() => ConfigManager.LoadConfiguration(this);
        public void LoadConfiguration(string path) => ConfigManager.LoadConfiguration(this, path);
        
        #endregion
    }
}