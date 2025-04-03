using System.Collections;
using System.ComponentModel;

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
        public AdvancedProperties AdvancedProperties { get; set; } = new();

        [ConfigProperty]    
        public required string Theme { 
            get => _theme;
            set {
                _theme = value;
                ThemeManager.SetTheme(_theme);
            }
        }

        public Dictionary<string, object> Parameters { get; set; } = new();
        public Dictionary<string, FormElementBase> FormElements { get; set; } = new();
        public Dictionary<string, Dictionary<string, string>> Themes { get; set; } = new();
        public Action<object, object> OnMessageTextChanged { get; internal set; }

        private MessageManager MessageManager;
        private ConfigManager ConfigManager;
        private ThemeManager ThemeManager;
        private ParametersManager ParametersManager;

        private string _theme;
        
        public Form? Form;
        public FlowLayoutPanel? ContentPanel;        
        public Panel? ButtonPanel, MessagePanel;
        public TextBox? MessageTextBox;

        #endregion

        #region Constructors

        public FormBuilder()
            : this(Constants.DefaultTitle, Constants.DefaultWidth, Constants.DefaultHeight, Constants.DefaultTheme)
        {
        }

        public FormBuilder(string title = Constants.DefaultTitle, int width = Constants.DefaultWidth, int height = Constants.DefaultHeight, string theme = Constants.DefaultTheme)
        {
            this.InitDefaults();

            Title = title;
            Width = width;
            Height = height;

            // Initialize config manager

            ConfigManager = new ConfigManager(this);

            // Initialize theme manager

            _theme = theme;

            ThemeManager = new ThemeManager(this);
            ThemeManager.SetTheme(_theme);

            // Initialize parameters manager

            ParametersManager = new ParametersManager(this);

            // Initialize message manager

            MessageManager = new MessageManager(this);

            ((INotifyPropertyChanged)MessageManager).PropertyChanged += (sender, e) => {
                if (e.PropertyName == nameof(MessageManager.MessageText))
                {
                    if (MessageTextBox != null
                        && MessagePanel != null
                        && MessageManager.MessageText != string.Empty
                    )
                    {
                        MessagePanel.Visible = true;
                        MessageTextBox.Text += MessageManager.MessageText;
                    }
                    else if (MessagePanel != null)
                    {
                        MessagePanel.Visible = false;
                    }
                }
            };
        }

        #endregion

        #region Functions
        
        public void SaveConfiguration() => ConfigManager.SaveConfiguration(this);
        public void SaveConfiguration(string path) => ConfigManager.SaveConfiguration(this, path);
        public void LoadConfiguration() => ConfigManager.LoadConfiguration(this);
        public void LoadConfiguration(string path) => ConfigManager.LoadConfiguration(this, path); 
        public Color GetThemeColor(string name) => ThemeManager.GetColor(name);
        public void ClearMessages() => MessageManager.Clear();
        public void PrintError(string message) => MessageManager.Print(message, MessageType.Error);
        public void PrintWarning(string message) => MessageManager.Print(message, MessageType.Warning);
        public void PrintSuccess(string message) => MessageManager.Print(message, MessageType.Success);
        public void PrintInfo(string message) => MessageManager.Print(message, MessageType.Info);
        public void PrintAlert(string message) => MessageManager.Print(message, MessageType.Alert);
        public void PrintDebug(string message) => MessageManager.Print(message, MessageType.Debug);
        public void PrintMessage(string message) => MessageManager.Print(message, MessageType.None);

        void InitDefaults()
        {
            Parameters.Clear();

            Title = Constants.DefaultTitle;
            Width = Constants.DefaultWidth;
            Height = Constants.DefaultHeight;
            FontSize = Constants.DefaultFontSize;
            Margin = Constants.DefaultMargin;
            Padding = Constants.DefaultPadding;
            Spacing = Constants.DefaultSpacing;

            AdvancedProperties = new AdvancedProperties
            {
                ReverseButtons = Constants.DefaultReverseButtons
            };
        }

        protected bool IsScrollbarVisible()
        {
            var DisplayHeight = ContentPanel?.DisplayRectangle.Height;
            var VisibleHeight = ContentPanel?.ClientRectangle.Height - Spacing;

            return DisplayHeight > VisibleHeight;
        }

        public DialogResult RunForm()
        {
            Form = BuildForm();
            Form.FormClosing += (sender, e) =>
            {
                if (Form.DialogResult == DialogResult.OK)
                {
                    foreach (var element in FormElements.Values)
                    {
                        if (element.Control != null && !element.ValidateControl())
                        {
                            e.Cancel = true;
                        }
                    }
                }
            };

            return Form.ShowDialog();
        }
        
        protected Form BuildForm()
        {
            ParametersManager.ParseParameters(Parameters);

            Form form = new()
            {
                Name = Constants.ID,
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

            ContentPanel = BuildContentPanel();
            MessagePanel = BuildMessagePanel();
            ButtonPanel = BuildButtonPanel();

            ContentPanel.Controls.Add(MessagePanel);
            FormElements = ParametersManager.FormElements;

            foreach (var element in FormElements.Values)
            {
                ContentPanel.Controls.Add(element.BuildControl());
            }

            // Add both panels to the form            
            form.Controls.Add(ContentPanel);
            form.Controls.Add(ButtonPanel);

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

        protected FlowLayoutPanel BuildContentPanel()
        {
            int buttonPanelHeight = ButtonPanel?.Height ?? 0;
            
            FlowLayoutPanel panel = new()
            {
                Name = $"{Constants.ID}_Container",
                Dock = DockStyle.Fill,
                Width = Width,
                Height = Height - buttonPanelHeight,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoScroll = true,
                BackColor = GetThemeColor("Panel")
            };

            panel.SetFlowBreak(panel, true);
            panel.Padding = new Padding(
                panel.Padding.Left,
                panel.Padding.Top,
                panel.Padding.Right,
                panel.Padding.Bottom + Spacing
            );

            panel.ControlAdded += (sender, e) =>
            {
                if (e.Control is Panel control)
                {
                    control.Margin = new Padding(
                        control.Margin.Left + Spacing,
                        control.Margin.Top + Spacing,
                        control.Margin.Right + Spacing,
                        control.Margin.Bottom
                    );
                }
            };

            return panel;
        }
        
        protected Panel BuildMessagePanel()
        {
            // Create a panel for the info
            Panel panel = new()
            {
                Name = $"{Constants.ID}_Message_Panel",
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(0, 0, 0, 0),
                Margin = new Padding(0, 0, 0, 0)
            };

            TextBox textBox = new()
            {
                Name = $"{Constants.ID}_Message_TextBox",
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                ScrollBars = ScrollBars.Vertical,
                WordWrap = true,
                BackColor = GetThemeColor("MessageBack"),
                ForeColor = GetThemeColor("MessageFore"),
            };
            
            // Add label to the panel
            panel.Controls.Add(textBox);

            // Prepare panel for messages
            MessageTextBox = textBox;
            panel.Visible = false;

            //Event whent
            return panel;
        }

        protected Panel BuildButtonPanel()
        {
            // Create a panel for the buttons
            Panel panel = new()
            {
                Name = $"{Constants.ID}_Button_Panel",
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = GetThemeColor("Panel")
            };
            
            // Create OK button
            Button buttonOk = new()
            {
                Name = $"{Constants.ID}_Ok_Button",
                Text = "OK",
                DialogResult = DialogResult.OK,
                Width = 80,
                Height = 30,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            
            // Create Cancel button
            Button buttonCancel = new()
            {
                Name = $"{Constants.ID}_Cancel_Button",
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Width = 80,
                Height = 30,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            
            // Add buttons to the panel
            if (AdvancedProperties.ReverseButtons)
            {
                buttonCancel.Location = new Point(panel.Width - buttonOk.Width - 10 - 90, (panel.Height - buttonOk.Height) / 2);
                buttonOk.Location = new Point(panel.Width - buttonCancel.Width - 10, (panel.Height - buttonCancel.Height) / 2);
            }
            else
            {
                buttonOk.Location = new Point(panel.Width - buttonOk.Width - 10 - 90, (panel.Height - buttonOk.Height) / 2);
                buttonCancel.Location = new Point(panel.Width - buttonCancel.Width - 10, (panel.Height - buttonCancel.Height) / 2);
            }
            
            panel.Controls.Add(buttonOk);
            panel.Controls.Add(buttonCancel);

            return panel;
        }

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

            for(int i = 0; i < 5; i++)
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
    }
}