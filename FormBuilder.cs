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
        public Dictionary<string, FormElementBase> FormElements { get; set; } = new();
        public Dictionary<string, Dictionary<string, string>> Themes { get; set; } = new();

        private ConfigManager _configManager;
        private ThemeManager _themeManager;
        private ParametersManager _parametersManager;
        private string _theme;
        
        public FlowLayoutPanel Container;        
        
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
            
            formBuilder.SaveConfiguration();
            formBuilder.LoadConfiguration();
            formBuilder.Parameters["InputFile"] = new Hashtable
            {
                { "Type", FormElementType.TextBox },
                { "Label", "Input File" },
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

            for(int i = 0; i < 25; i++)
            {
                formBuilder.Parameters[$"Test{i}"] = new Hashtable
                {
                    { "Type", FormElementType.TextBox },
                    { "Label", $"Testas #{i}" },
                    { "Description", "The input file to process" },
                    { "Required", false }
                };
            }

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

            _configManager = new ConfigManager(this);

            // Initialize theme manager

            _theme = theme;
            _themeManager = new ThemeManager(this);
            _themeManager.SetTheme(_theme);

            // Initialize parameters manager

            _parametersManager = new ParametersManager(this);
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
                AutoScroll = false,
                AutoSize = false,
                BackColor = GetThemeColor("Background"),
                ForeColor = GetThemeColor("Foreground")
            };

            Container = BuildContainer();
            var idx = 0;

            FormElements = _parametersManager.FormElements;
            foreach (var element in FormElements.Values)
            {
                var control = element.BuildControl();
                if (control != null)
                {
                    idx++;
                    control.Margin = new Padding(
                        control.Margin.Left + Spacing,
                        control.Margin.Top + (idx==1 ? Spacing : 0),
                        control.Margin.Right + Spacing,
                        control.Margin.Bottom + Spacing
                    );

                    Container.Controls.Add(control);
                }
            }

            form.Controls.Add(Container);

            if (IsScrollbarVisible())
            {
                foreach (var element in FormElements.Values)
                {
                    if (element.Control != null && element.Control is TableLayoutPanel panel)
                    {
                        int offset = Spacing * 2 + 16 + 20;
                        int minWidth = Width - offset;
                        int minHeight = panel.MinimumSize.Height;

                        panel.MinimumSize = new Size(minWidth, minHeight);
                    }
                }
            }

            return form;
        }

        protected FlowLayoutPanel BuildContainer()
        {
            FlowLayoutPanel container = new()
            {
                Dock = DockStyle.Fill,
                Width = Width,
                Height = Height,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoScroll = true,
                // Padding = new Padding(Padding), // Add padding inside flow layout
                // Margin = new Padding(Margin), // Add margin around flow layout
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

        protected bool IsScrollbarVisible()
        {
            var DisplayHeight = Container.DisplayRectangle.Height;
            var VisibleHeight = Container.ClientRectangle.Height - Spacing;

            return DisplayHeight > VisibleHeight;
        }

        public Color GetThemeColor(string name) => _themeManager.GetColor(name);

        public void SaveConfiguration() => _configManager.SaveConfiguration(this);
        public void SaveConfiguration(string path) => _configManager.SaveConfiguration(this, path);
        public void LoadConfiguration() => _configManager.LoadConfiguration(this);
        public void LoadConfiguration(string path) => _configManager.LoadConfiguration(this, path); 

        #endregion
    }  
}