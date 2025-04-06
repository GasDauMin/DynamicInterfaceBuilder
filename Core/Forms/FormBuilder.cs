using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Documents;
using System.Globalization;

namespace DynamicInterfaceBuilder
{
    public class FormBuilder : ApplicationBase
    {
        #region Properties

        public Window? Form;   
        public Panel? MainPanel, MessagePanel, ContentPanel, ButtonPanel;
        public RichTextBox? MessageViewer;
        
        public int MaxLabelWidth{ get; set; } = -1;

        #endregion

        #region Functions

        public FormBuilder(Application application) : base(application)
        {
        }

        public bool? Run()
        {
            Form = BuildForm();

            AdjustMessageViewer();
            // AdjustControls();
            // AdjustLabels();

            Form.Closing += HandleFormClosing;
            Form.SizeChanged += HandleFormResize;

            return Form.ShowDialog();
        }

        protected bool IsScrollbarVisible()
        {
            var DisplayHeight = ContentPanel?.ActualHeight ?? 0;;
            var VisibleHeight = ContentPanel?.Height ?? 0;;

            return DisplayHeight > VisibleHeight;
        }

        protected double SampleLineHeight(Control control, string sample = "Sample")
        {
            var formattedText = new FormattedText(
                sample,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(control.FontFamily, control.FontStyle, control.FontWeight, control.FontStretch),
                control.FontSize,
                Brushes.Black,
                new NumberSubstitution(),
                VisualTreeHelper.GetDpi(control).PixelsPerDip
            );

            return formattedText.Height;
        }

        #endregion

        #region Event handlers

        private void HandleFormClosing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Form != null && Form.DialogResult == true)
            {                                
                if (!App.Validate())
                {                    
                    e.Cancel = true;
                }

                AdjustMessageViewer();
            }
        }

        private void HandleFormResize(object? sender, EventArgs e)
        {
            if (Form != null && ContentPanel != null && ButtonPanel != null)
            {
                App.Width = (int)Form.Width;
                App.Height = (int)Form.Height;
            }
        }

        #endregion

        #region Build form

        protected Window BuildForm()
        {
            Window form = new()
            {
                Title = App.Title,
                Width = App.Width,
                Height = App.Height,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = App.AdvancedProperties.AllowResize ? ResizeMode.CanResize : ResizeMode.NoResize,
                FontFamily = new FontFamily(App.FontName),
                FontSize = App.FontSize,
                Background = App.GetBrush("Background"),
                Foreground = App.GetBrush("Foreground"),
            };

            form.Content = MainPanel = BuildMainPanel();
            return form;
        }

        #endregion

        #region Build panels

        protected Grid BuildMainPanel()
        {
            // Create a main grid panel
            Grid panel = new();
            
            // Define the grid rows
            panel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });  // Message panel
            panel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });  // Content panel
            panel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });  // Button panel

            // Create and initialize the panels
            ContentPanel = BuildContentPanel();;
            MessagePanel = BuildMessagePanel();
            ButtonPanel = BuildButtonPanel();
            
            // Load all form elements
            App.FormElements = App.ParametersManager.FormElements;

            // Add form elements to content panel
            foreach (var element in App.FormElements.Values)
            {
                if (element.BuildControl() is UIElement control)
                {
                    ContentPanel.Children.Add(control); ;
                }
            }

            // Create a ScrollViewer to wrap the ContentPanel for scrolling
            ScrollViewer scrollViewer = new()
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Content = ContentPanel,
            };

            // Add panels to the main grid
            Grid.SetRow(MessagePanel, 0);
            Grid.SetRow(scrollViewer, 1);
            Grid.SetRow(ButtonPanel, 2);

            panel.Children.Add(MessagePanel);
            panel.Children.Add(scrollViewer);
            panel.Children.Add(ButtonPanel);

            return panel;
        }

        protected StackPanel BuildContentPanel()
        {
            // Create a panel for the content area
            StackPanel panel = new()
            {
                Name = $"{Constants.ID}_Content_Panel",
                Orientation = Orientation.Vertical,
                Width = double.NaN, // Auto width
                Background = App.GetBrush("Panel"),
                Margin = new Thickness(0, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            panel.Loaded += (s, e) =>
            {
                foreach (UIElement element in panel.Children)
                {
                    if (element is FrameworkElement control)
                    {
                        control.Margin = new Thickness(App.Spacing, App.Spacing, App.Spacing, 0);
                    }
                }
            };




            return panel;
        }
        
        protected Panel BuildMessagePanel()
        {
            // Create a panel for the message viewer
            Grid panel = new()
            {
                Name = $"{Constants.ID}_Message_Panel",
                Height = 90,
                Margin = new Thickness(0)
            };

            // Create a RichTextBox for displaying messages
            RichTextBox viewer = new()
            {
                Name = $"{Constants.ID}_Message_Viewer",
                IsReadOnly = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Background = App.GetBrush("Message.Background"),
                Foreground = App.GetBrush("Message.Foreground"),
                Padding = new Thickness(0),
                BorderThickness = new Thickness(0),
                Document = new FlowDocument
                {
                    FontFamily = new FontFamily("Segoe UI Emoji"),
                    PagePadding = new Thickness(5)
                }
            };

            Label buttonX = new()
            {
                Name = $"{Constants.ID}_X_Button",
                Content = "❌",
                FontSize = 9,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 0, 0, 0),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Foreground = App.GetBrush("Foreground"),
            };
            
            // Add click handler to hide the message panel
            buttonX.MouseLeftButtonDown += (s, e) => 
            {
                if (MessagePanel != null)
                {
                    MessagePanel.Visibility = Visibility.Collapsed;
                    App.MessageText = string.Empty;
                }
            };
            
            // Add viewer to the panel
            panel.Children.Add(viewer);
            panel.Children.Add(buttonX);

            // Global reference to the message viewer
            MessageViewer = viewer;

            return panel;
        }

        protected Grid BuildButtonPanel()
        {
            // Create a panel for the buttons
            Grid panel = new()
            {
                Name = $"{Constants.ID}_Button_Panel",
                Height = 40,
                Background = App.GetBrush("Panel")
            };
            
            // Create OK button
            Button buttonOk = new()
            {
                Name = $"{Constants.ID}_Ok_Button",
                Content = "OK",
                IsDefault = true,
                Width = 100,
                Height = 25,
                Background = App.GetBrush("Button.Background"),
                Foreground = App.GetBrush("Button.Foreground"),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };
            
            // Add hover effect
            buttonOk.MouseEnter += (s, e) => {
                buttonOk.Background = App.GetBrush("Button.Hover");
            };
            
            buttonOk.MouseLeave += (s, e) => {
                buttonOk.Background = App.GetBrush("Button.Background");
            };
            
            // Create Cancel button
            Button buttonCancel = new()
            {
                Name = $"{Constants.ID}_Cancel_Button",
                Content = "Cancel",
                IsCancel = true,
                Width = 100,
                Height = 25,
                Background = App.GetBrush("Button.Background"),
                Foreground = App.GetBrush("Button.Foreground"),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };
            
            // Set button click handlers
            buttonOk.Click += (s, e) => { if (Form != null) Form.DialogResult = true; };
            buttonCancel.Click += (s, e) => { if (Form != null) Form.DialogResult = false; };
            
            // Position buttons
            if (App.AdvancedProperties.ReverseButtons)
            {
                buttonCancel.Margin = new Thickness(0, 0, 120, 0);
                buttonOk.Margin = new Thickness(0, 0, 7, 0);
            }
            else
            {
                buttonOk.Margin = new Thickness(0, 0, 120, 0);
                buttonCancel.Margin = new Thickness(0, 0, 7, 0);
            }
            
            // Add buttons to the panel
            panel.Children.Add(buttonOk);
            panel.Children.Add(buttonCancel);

            return panel;
        }

        #endregion

        #region Adjustments

        public void AdjustMessageViewer()
        {
            if (MessagePanel == null || MessageViewer == null)
                return;

            if (!string.IsNullOrEmpty(App.MessageText))
            {
                MessagePanel.Visibility = Visibility.Visible;
                
                MessageViewer.Document.Blocks.Clear();
                MessageViewer.Document.Blocks.Add(new Paragraph(new Run(App.MessageText)));
                MessageViewer.Background = App.GetBrush(App.MessageManager.ColorKey(ColorType.Background));
                MessageViewer.Foreground = App.GetBrush(App.MessageManager.ColorKey(ColorType.Foreground));

                int lineCount = App.MessageText.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).Length;
                double lineHeight = SampleLineHeight(MessageViewer);

                double padding = 2; // Additional padding

                if (lineCount <= App.AdvancedProperties.MaxMessageLines)
                {
                    MessagePanel.Height = (lineHeight * lineCount) + padding;
                    MessageViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                }
                else
                {
                    MessagePanel.Height = (lineHeight * App.AdvancedProperties.MaxMessageLines) + padding;
                    MessageViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                }
            }
            else
            {
                MessagePanel.Visibility = Visibility.Collapsed;
                MessageViewer.Document.Blocks.Clear();
            }
        }

        public void AdjustLabels()
        {
            if (!App.AdvancedProperties.AdjustLabels)
                return;

            if (MaxLabelWidth == -1)
            {
                foreach (var element in App.FormElements.Values)
                {
                    if (element.PanelControl != null && element.PanelControl is Grid panel)
                    {
                        foreach (UIElement control in panel.Children)
                        {
                            if (control is TextBlock textBlock)
                            {
                                MaxLabelWidth = Math.Max(MaxLabelWidth, (int)textBlock.ActualWidth);
                            }
                        }
                    }      
                }
            }

            if (MaxLabelWidth > 0)
            {
                foreach (var element in App.FormElements.Values)
                {
                    if (element.PanelControl != null && element.PanelControl is Grid panel)
                    {
                        foreach (UIElement control in panel.Children)
                        {
                            if (control is TextBlock textBlock)
                            {
                                textBlock.TextAlignment = TextAlignment.Right;
                                textBlock.Width = MaxLabelWidth;
                                textBlock.MinWidth = MaxLabelWidth;
                            }
                        }
                    }
                }
            }
        }

        public void AdjustControls(bool initial = false)
        {
            if (ContentPanel == null)
                return;

            double availableWidth = ContentPanel.ActualWidth;;

            if (double.IsNaN(availableWidth) || availableWidth <= 0)
            {
                availableWidth = App.Width - ((App.Spacing != 0 ? App.Spacing : 1) * 2 + 20);
            }

            if (IsScrollbarVisible())
            {
                availableWidth -= SystemParameters.VerticalScrollBarWidth + 2;
            }

            foreach (var element in App.FormElements.Values)
            {
                if (element.PanelControl is FrameworkElement ctrl)
                {
                    ctrl.Width = availableWidth;
                    ctrl.MinWidth = availableWidth;
                }
            }
        }

        #endregion
    }
}