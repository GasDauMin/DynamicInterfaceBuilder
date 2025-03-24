using System.Collections;

namespace DynamicInterfaceBuilder
{
    public class FormBuilder
    {
        #region Properties

        [Managers.ConfigProperty]
        public required string Title { get; set; }
        
        [Managers.ConfigProperty]
        public required int Width { get; set; }
        
        [Managers.ConfigProperty]
        public required int Height { get; set; }

        [Managers.ConfigProperty]
        public int FontSize { get; set; }

        [Managers.ConfigProperty]
        public int Margin { get; set; }
        
        [Managers.ConfigProperty]
        public int Padding { get; set; }

        [Managers.ConfigProperty]
        public int Spacing { get; set; }

        [Managers.ConfigProperty]    
        public required string Theme { 
            get => _theme;
            set {
                _theme = value;
                _themeManager.SetTheme(_theme);
            }
        }

        public Dictionary<string, object> Parameters { get; set; } = new();
        public Dictionary<string, object> Results { get; set; } = new();
        public Dictionary<string, Dictionary<string, string>> Themes { get; set; } = new();

        private Managers.ConfigManager _configManager;
        private Managers.ThemeManager _themeManager;
        private string _theme;
        
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

            _configManager = new Managers.ConfigManager();

            // Initialize theme manager

            _theme = theme;
            _themeManager = new Managers.ThemeManager();
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

        public void TestHashtable(Hashtable parameters)
        {
            foreach (DictionaryEntry entry in parameters)
            {
                Console.WriteLine($"{entry.Key}: {entry.Value}");
            }
        }
        
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