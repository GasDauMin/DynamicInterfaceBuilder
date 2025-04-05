using System.Collections;
using System.ComponentModel;
using System.Reflection.Metadata;

namespace DynamicInterfaceBuilder
{
    public class FormBuilder
    {
        #region Managers

        private MessageManager MessageManager;
        private ConfigManager ConfigManager;
        private ThemeManager ThemeManager;
        private ParametersManager ParametersManager;

        #endregion


        #region Properties

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
        
        public Form? Form;
        public FlowLayoutPanel? ContentPanel;        
        public Panel? ButtonPanel, MessagePanel;
        public RichTextBox? MessageTextBox;
        public int LabelWidth { get; set; } = -1;
        public Dictionary<string, object> Parameters { get; set; } = new();
        public Dictionary<string, FormElementBase> FormElements { get; set; } = new();
        public Action<object, object>? OnMessageTextChanged { get; internal set; }

        #endregion

        #region Constructors

        public FormBuilder()
            : this(Constants.DefaultTitle, Constants.DefaultWidth, Constants.DefaultHeight, Constants.DefaultTheme)
        {
        }

        public FormBuilder(string title = Constants.DefaultTitle, int width = Constants.DefaultWidth, int height = Constants.DefaultHeight, string theme = Constants.DefaultTheme)
        {
            ConfigManager = new(this);
            ThemeManager = new(this);
            ParametersManager = new(this);
            MessageManager = new(this);

            Title = title;
            Width = width;
            Height = height;
            Theme = theme;          
           
            if (MessageManager is INotifyPropertyChanged notifyPropertyChanged)
            {
                ((INotifyPropertyChanged)MessageManager).PropertyChanged += HandleMessageTextChanged;
            }
        }

        #endregion

        #region Functions
    
        void ResetDefaults(Boolean save = false)
        {
            Parameters.Clear();

            Title = Constants.DefaultTitle;
            Width = Constants.DefaultWidth;
            Height = Constants.DefaultHeight;
            Spacing = Constants.DefaultSpacing;
            Theme = Constants.DefaultTheme;

            AdvancedProperties = new AdvancedProperties
            {
                ReverseButtons = Constants.DefaultReverseButtons,
                AllowResize = Constants.DefaultAllowResize,
            };
        }

        protected bool IsScrollbarVisible()
        {
            var DisplayHeight = ContentPanel?.DisplayRectangle.Height;
            var VisibleHeight = ContentPanel?.ClientRectangle.Height;

            return DisplayHeight > VisibleHeight;
        }
        
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

        #endregion

        #region Events

        private void HandleMessageTextChanged(object? sender, EventArgs e)
        {
            if (MessageTextBox != null && MessagePanel != null && MessageManager.MessageText != string.Empty)
            {
                MessagePanel.Visible = true;
                MessageTextBox.Text += MessageManager.MessageText;
            }
            else if (MessagePanel != null)
            {
                MessagePanel.Visible = false;
            }
        }

        private void HandleFormClosing(object? sender, FormClosingEventArgs e)
        {
            if (Form?.DialogResult == DialogResult.OK)
            {
                foreach (var element in FormElements.Values)
                {
                    if (element.Control != null && !element.ValidateControl())
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        private void HandleFormResize(object? sender, EventArgs e)
        {
            if (Form != null && ContentPanel != null && ButtonPanel != null)
            {
                Width = Form.Width;
                Height = Form.Height;

                AdjustControls();
            }
        }

        #endregion

        #region Form builder

        public DialogResult RunForm()
        {
            if (Environment.OSVersion.Version.Major >= 6)
                WinAPI.SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            if (AdvancedProperties.AutoLoadConfig)
            {
                LoadConfiguration();
            }  

            Form = BuildForm();
            Form.FormClosing += HandleFormClosing;
            Form.Resize += HandleFormResize;

            DialogResult result = Form.ShowDialog();

            Form.Dispose();
            Form = null;

            if (AdvancedProperties.AutoSaveConfig)
            {
                SaveConfiguration();
            }  

            return result;
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
                FormBorderStyle = (AdvancedProperties.AllowResize ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle),
                MaximizeBox = false,
                MinimizeBox = true,
                AutoScroll = false,
                AutoSize = false,
                Font = new Font(FontName, FontSize),
                BackColor = GetThemeColor("Background"),
                ForeColor = GetThemeColor("Foreground"),
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

            AdjustControls();
            AdjustLabels();

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
                BackColor = GetThemeColor("Panel"),
                Margin = new Padding(0, 0, 0, 0),
                Padding = new Padding(0, 0, 0, 0),
            };

            panel.SetFlowBreak(panel, true);
            panel.ControlAdded += (sender, e) =>
            {
                if (e.Control is Panel control)
                {
                    control.Margin = new Padding(
                        control.Margin.Left + Spacing,
                        control.Margin.Top + Spacing,
                        control.Margin.Right,
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
                Height = 90,
                Padding = new Padding(0, 0, 0, 0),
                Margin = new Padding(0, 0, 0, 0)
            };

            RichTextBox richTextBox = new()
            {
                Name = $"{Constants.ID}_Message_TextBox",
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                WordWrap = true,
                Padding = new Padding(0, 0, 0, 0),
                Margin = new Padding(0, 0, 0, 0),
                BackColor = GetThemeColor("MessageBack"),
                ForeColor = GetThemeColor("MessageFore"),
            };
            
            // Add label to the panel
            panel.Controls.Add(richTextBox);

            // Prepare panel for messages
            MessageTextBox = richTextBox;
            panel.Visible = true;
            MessageTextBox.Text = "ℹ️ Info line\n✅ Success line\n❌ Error line\n⚠️ Warning line\n❗ Alert line\n🛠️ Debug line\n";

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
                Height = 60,
                BackColor = GetThemeColor("Panel")
            };
            
            // Create OK button
            Button buttonOk = new()
            {
                Name = $"{Constants.ID}_Ok_Button",
                Text = "OK",
                DialogResult = DialogResult.OK,
                Width = 100,
                Height = 45,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            
            // Create Cancel button
            Button buttonCancel = new()
            {
                Name = $"{Constants.ID}_Cancel_Button",
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Width = 100,
                Height = 45,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            
            // Add buttons to the panel
            if (AdvancedProperties.ReverseButtons)
            {
                buttonCancel.Location = new Point(panel.Width - buttonOk.Width - 120, (panel.Height - buttonOk.Height) / 2);
                buttonOk.Location = new Point(panel.Width - buttonCancel.Width - 10, (panel.Height - buttonCancel.Height) / 2);
            }
            else
            {
                buttonOk.Location = new Point(panel.Width - buttonOk.Width - 120, (panel.Height - buttonOk.Height) / 2);
                buttonCancel.Location = new Point(panel.Width - buttonCancel.Width - 10, (panel.Height - buttonCancel.Height) / 2);
            }
            
            panel.Controls.Add(buttonOk);
            panel.Controls.Add(buttonCancel);

            return panel;
        }

        public void AdjustLabels()
        {
            if (!AdvancedProperties.AdjustLabels)
                return;

            if (LabelWidth == -1)
            {
                foreach (var element in FormElements.Values)
                {
                    if (element.Control != null && element.Control is TableLayoutPanel panel)
                    {
                        foreach (Control control in panel.Controls)
                        {
                            LabelWidth = Math.Max(LabelWidth, panel.GetColumnWidths()[0]);
                        }
                    }      
                }
            }

            if (LabelWidth > 0)
            {
                foreach (var element in FormElements.Values)
                {
                    if (element.Control != null && element.Control is TableLayoutPanel panel)
                    {
                        foreach (Control control in panel.Controls)
                        {
                            if (control is Label label)
                            {
                                label.AutoSize = false;
                                label.Width = LabelWidth;
                                label.TextAlign = ContentAlignment.MiddleRight;
                                label.MinimumSize = new Size(LabelWidth, label.Height);
                            }
                        }
                    }
                }
            }
        }

        public void AdjustControls(bool initial = false)
        {
            foreach (var element in FormElements.Values)
            {
                // Adjust the control size and position
                if (element.Control != null && element.Control is Control ctrl)
                {
                    int offset = (Spacing !=0 ? Spacing : 1) * 2 + 20;
                    
                    if (IsScrollbarVisible())
                    {
                        offset += SystemInformation.VerticalScrollBarWidth + 2;
                    }

                    int w = Width - offset;
                    int h = ctrl.MinimumSize.Height;

                    ctrl.Width = w;
                    ctrl.MinimumSize = new Size(w, h);
                }
            }
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
            
            formBuilder.Parameters["InputFile"] = new Hashtable
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