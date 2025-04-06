using System.Collections;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

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
        
        public Window? Form;
        public StackPanel? ContentGrid;        
        public Grid? ButtonGrid, MessageGrid;
        public FlowDocumentScrollViewer? MessageViewer;
        private FlowDocument? MessageDocument;
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
            var DisplayHeight = ContentGrid?.ActualHeight ?? 0;
            var VisibleHeight = ContentGrid?.Height ?? 0;

            return DisplayHeight > VisibleHeight;
        }
        
        public void SaveConfiguration() => ConfigManager.SaveConfiguration(this);
        public void SaveConfiguration(string path) => ConfigManager.SaveConfiguration(this, path);
        public void LoadConfiguration() => ConfigManager.LoadConfiguration(this);
        public void LoadConfiguration(string path) => ConfigManager.LoadConfiguration(this, path); 
        public System.Windows.Media.Color GetThemeColor(string name) => ThemeManager.GetWpfColor(name);
        public void ClearMessages() => MessageManager.Clear();
        public void PrintError(string message) => MessageManager.Print(message, MessageType.Error);
        public void PrintWarning(string message) => MessageManager.Print(message, MessageType.Warning);
        public void PrintSuccess(string message) => MessageManager.Print(message, MessageType.Success);
        public void PrintInfo(string message) => MessageManager.Print(message, MessageType.Info);
        public void PrintAlert(string message) => MessageManager.Print(message, MessageType.Alert);
        public void PrintDebug(string message) => MessageManager.Print(message, MessageType.Debug);
        public void PrintMessage(string message) => MessageManager.Print(message, MessageType.None);

        #endregion

        #region Event handlers

        private void HandleMessageTextChanged(object? sender, EventArgs e)
        {
            if (MessageViewer != null && MessageGrid != null && !string.IsNullOrEmpty(MessageManager.MessageText))
            {
                MessageGrid.Visibility = Visibility.Visible;

                if (MessageDocument == null)
                {
                    MessageDocument = new FlowDocument
                    {
                        FontFamily = new FontFamily("Segoe UI Emoji"),
                        PagePadding = new Thickness(5)
                    };
                    MessageViewer.Document = MessageDocument;
                }

                MessageDocument.Blocks.Clear();
                foreach (var line in MessageManager.MessageText.Split('\n'))
                {
                    MessageDocument.Blocks.Add(new Paragraph(new Run(line.TrimEnd())));
                }
            }
            else if (MessageGrid != null)
            {
                MessageGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void HandleFormClosing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Form != null && Form.DialogResult == true)
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
            if (Form != null && ContentGrid != null && ButtonGrid != null)
            {
                Width = (int)Form.Width;
                Height = (int)Form.Height;

                AdjustControls();
            }
        }

        #endregion

        #region Form builder

        public bool? RunForm()
        {
            if (AdvancedProperties.AutoLoadConfig)
            {
                LoadConfiguration();
            }  

            Form = BuildForm();
            Form.Closing += HandleFormClosing;
            Form.SizeChanged += HandleFormResize;

            bool? result = Form.ShowDialog();

            if (AdvancedProperties.AutoSaveConfig)
            {
                SaveConfiguration();
            }  

            return result;
        }

        protected Window BuildForm()
        {
            ParametersManager.ParseParameters(Parameters);

            Window form = new()
            {
                Title = Title,
                Width = Width,
                Height = Height,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = (AdvancedProperties.AllowResize ? ResizeMode.CanResize : ResizeMode.NoResize),
                FontFamily = new FontFamily(FontName),
                FontSize = FontSize,
                Background = new SolidColorBrush(GetThemeColor("Background")),
                Foreground = new SolidColorBrush(GetThemeColor("Foreground")),
            };

            // Create main content container
            Grid mainGrid = new Grid();
            
            // Define rows in the main grid
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });  // Message panel
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });  // Content panel
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });  // Button panel

            ContentGrid = BuildContentPanel();
            MessageGrid = BuildMessagePanel();
            ButtonGrid = BuildButtonPanel();

            // Subscribe to SizeChanged event to dynamically adjust controls
            ContentGrid.SizeChanged += (s, e) => AdjustControls();
            
            // Load all form elements
            FormElements = ParametersManager.FormElements;

            // Add form elements to content panel
            foreach (var element in FormElements.Values)
            {
                UIElement? control = element.BuildControl() as UIElement;
                if (control != null)
                {
                    ContentGrid.Children.Add(control);
                }
            }

            // Create a ScrollViewer to wrap the ContentPanel for scrolling
            ScrollViewer scrollViewer = new()
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Content = ContentGrid
            };

            // Add panels to the main grid
            Grid.SetRow(MessageGrid, 0);
            Grid.SetRow(scrollViewer, 1);
            Grid.SetRow(ButtonGrid, 2);
            mainGrid.Children.Add(MessageGrid);
            mainGrid.Children.Add(scrollViewer);
            mainGrid.Children.Add(ButtonGrid);

            // Set the main grid as the form's content
            form.Content = mainGrid;

            AdjustControls();
            AdjustLabels();

            return form;
        }

        protected StackPanel BuildContentPanel()
        {
            StackPanel panel = new()
            {
                Name = $"{Constants.ID}_Container",
                Orientation = Orientation.Vertical,
                Width = double.NaN, // Auto width
                Background = new SolidColorBrush(GetThemeColor("Panel")),
                Margin = new Thickness(0, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            return panel;
        }
        
        protected Grid BuildMessagePanel()
        {
            Grid panel = new()
            {
                Name = $"{Constants.ID}_Message_Panel",
                Height = 90,
                Margin = new Thickness(0)
            };

            FlowDocumentScrollViewer viewer = new()
            {
                Name = $"{Constants.ID}_Message_Viewer",
                IsToolBarVisible = false,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                
                Background = new SolidColorBrush(GetThemeColor("MessageBack")),
                Foreground = new SolidColorBrush(GetThemeColor("MessageFore")),
                Padding = new Thickness(0),
                BorderThickness = new Thickness(0)
            };

            panel.Children.Add(viewer);

            MessageViewer = viewer;
            MessageDocument = null;

            panel.Visibility = Visibility.Visible;

            return panel;
        }

        protected Grid BuildButtonPanel()
        {
            // Create a panel for the buttons
            Grid panel = new()
            {
                Name = $"{Constants.ID}_Button_Panel",
                Height = 60,
                Background = new SolidColorBrush(GetThemeColor("Panel"))
            };
            
            // Create OK button
            Button buttonOk = new()
            {
                Name = $"{Constants.ID}_Ok_Button",
                Content = "OK",
                IsDefault = true,
                Width = 100,
                Height = 45,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };
            
            // Create Cancel button
            Button buttonCancel = new()
            {
                Name = $"{Constants.ID}_Cancel_Button",
                Content = "Cancel",
                IsCancel = true,
                Width = 100,
                Height = 45,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };
            
            // Set button click handlers
            buttonOk.Click += (s, e) => { if (Form != null) Form.DialogResult = true; };
            buttonCancel.Click += (s, e) => { if (Form != null) Form.DialogResult = false; };
            
            // Position buttons
            if (AdvancedProperties.ReverseButtons)
            {
                buttonCancel.Margin = new Thickness(0, 0, 120, 0);
                buttonOk.Margin = new Thickness(0, 0, 10, 0);
            }
            else
            {
                buttonOk.Margin = new Thickness(0, 0, 120, 0);
                buttonCancel.Margin = new Thickness(0, 0, 10, 0);
            }
            
            panel.Children.Add(buttonOk);
            panel.Children.Add(buttonCancel);

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
                    if (element.Control != null && element.Control is Grid panel)
                    {
                        foreach (UIElement control in panel.Children)
                        {
                            if (control is TextBlock textBlock)
                            {
                                LabelWidth = Math.Max(LabelWidth, (int)textBlock.ActualWidth);
                            }
                        }
                    }      
                }
            }

            if (LabelWidth > 0)
            {
                foreach (var element in FormElements.Values)
                {
                    if (element.Control != null && element.Control is Grid panel)
                    {
                        foreach (UIElement control in panel.Children)
                        {
                            if (control is TextBlock textBlock)
                            {
                                textBlock.TextAlignment = TextAlignment.Right;
                                textBlock.Width = LabelWidth;
                                textBlock.MinWidth = LabelWidth;
                            }
                        }
                    }
                }
            }
        }

        public void AdjustControls(bool initial = false)
        {
            if (ContentGrid == null)
                return;

            double availableWidth = ContentGrid.ActualWidth;

            if (double.IsNaN(availableWidth) || availableWidth <= 0)
            {
                availableWidth = Width - ((Spacing != 0 ? Spacing : 1) * 2 + 20);
            }

            if (IsScrollbarVisible())
            {
                availableWidth -= SystemParameters.VerticalScrollBarWidth + 2;
            }

            foreach (var element in FormElements.Values)
            {
                if (element.Control is FrameworkElement ctrl)
                {
                    ctrl.Width = availableWidth;
                    ctrl.MinWidth = availableWidth;
                }
            }
        }

        #endregion

        #region Main 

        [STAThread]
        static void Main()
        {
            // Standard WPF application initialization
            var application = new Application();
            
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
            
            // Exit the application when the form is closed
            application.Shutdown();
        }

        #endregion
    }
}