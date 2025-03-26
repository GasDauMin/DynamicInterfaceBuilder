using System.Collections;

namespace DynamicInterfaceBuilder
{
    public class FormBuilder
    {
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

        public Dictionary<string, object> Parameters { get; set; } = new();
        public Dictionary<string, FormElement> FormElements { get; set; } = new();
        public Dictionary<string, Dictionary<string, string>> Themes { get; set; } = new();

        private ConfigManager _configManager;
        private ThemeManager _themeManager;
        private ParametersManager _parametersManager;
        private string _theme;
        
        #endregion

        #region Main 

        static void Main()
        {
            FormBuilder formBuilder = new()
            {
                Title = Constants.DefaultTitle,
                Width = Constants.DefaultWidth, 
                Height = Constants.DefaultHeight,
                Theme = Constants.DefaultTheme
            };
            
            formBuilder.Parameters["InputFile"] = new Hashtable
            {
                { "Type", FormElementType.TextBox },
                { "Label", "Input File" },
                { "Description", "The input file to process" },
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

            formBuilder.RunForm();
        }

        #endregion

        #region Constructors

        public FormBuilder()
            : this(Constants.DefaultTitle, Constants.DefaultWidth, Constants.DefaultHeight, Constants.DefaultTheme)
        {
        }

        public FormBuilder(string title = Constants.DefaultTitle, int width = Constants.DefaultWidth, int height = Constants.DefaultHeight, string theme = Constants.DefaultTheme)
        {
            Title = title;
            Width = width;
            Height = height;

            Margin = Constants.DefaultMargin;
            Padding = Constants.DefaultPadding;
            Spacing = Constants.DefaultSpacing;
            FontSize = Constants.DefaultFontSize;

            // Initialize config manager

            _configManager = new ConfigManager();

            // Initialize theme manager

            _theme = theme;
            _themeManager = new ThemeManager();
            _themeManager.SetTheme(_theme);

            // Initialize parameters manager

            _parametersManager = new ParametersManager();
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
            _parametersManager.ParseParameters(Parameters);

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
            
            FormElements = _parametersManager.FormElements;
            foreach (var element in FormElements.Values)
            {
                
            }
            
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
            Title = Constants.DefaultTitle;
            Width = Constants.DefaultWidth;
            Height = Constants.DefaultHeight;
            
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