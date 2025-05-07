using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Documents;
using System.Globalization;

using DynamicInterfaceBuilder.Core.Models;
using DynamicInterfaceBuilder.Core.Constants;
using DynamicInterfaceBuilder.Core.Managers;

namespace DynamicInterfaceBuilder.Core.Forms
{
    public class FormBuilder : AppBase
    {
        #region Properties

        public Window? Form;   
        public Panel? MainPanel, MessagePanel, ContentPanel, ButtonPanel;
        public RichTextBox? MessageViewer;
        
        public int MaxLabelWidth{ get; set; } = -1;

        #endregion

        #region Functions

        public FormBuilder(App application) : base(application)
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
                // Removed validation call
                AdjustMessageViewer();
            }
        }

        private void HandleFormResize(object? sender, EventArgs e)
        {
        }

        #endregion

        #region Build form

        protected Window BuildForm()
        {
            Window form = new()
            {
                Title = App.Title,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = App.AdvancedProperties.AllowResize ? ResizeMode.CanResize : ResizeMode.NoResize,
                FontFamily = Default.FontFamily,
                FontSize = Default.FontSize,
                Style = (Style)Application.Current.Resources["CustomWindowStyle"]
            };

            if (App.Width != null && App.Width > 0)
            {
                form.Width = (double)App.Width;
            }

            if (App.Height != null && App.Height > 0)
            {
                form.Height = (double)App.Height;
            }

            // Set icon if provided
            if (!string.IsNullOrEmpty(App.Icon))
            {
                try
                {
                    var uri = new Uri(App.Icon, UriKind.RelativeOrAbsolute);
                    form.Icon = new System.Windows.Media.Imaging.BitmapImage(uri);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to load custom icon: {ex.Message}");
                }
            }
            
            // If no custom icon or loading failed, try resource icon
            if (form.Icon == null)
            {
                try 
                {
                    var uri = new Uri("pack://application:,,,/Resources/Icons/App1.ico", UriKind.Absolute);
                    form.Icon = new System.Windows.Media.Imaging.BitmapImage(uri);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to load resource icon: {ex.Message}");
                }
            }

            form.Content = MainPanel = BuildMainPanel();
            return form;
        }

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
                if (element.BuildElement() is UIElement control)
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
                Name = $"{Default.ID}_Content_Panel",
                Orientation = Orientation.Vertical,
                Width = double.NaN, // Auto width
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
                        control.Margin = new Thickness(Default.Spacing, Default.Spacing, Default.Spacing, 0);
                    }
                }
            };

            return panel;
        }
        
        protected Panel BuildMessagePanel()
        {
            Grid panel = new()
            {
                Name = $"{Default.ID}_Message_Panel",
                Height = 100,
                Margin = new Thickness(0),
            };

            RichTextBox viewer = new()
            {
                Name = $"{Default.ID}_Message_Viewer",
                IsReadOnly = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Padding = new Thickness(0, 3, 0, 0),
                BorderThickness = new Thickness(0),
                Background = ThemeManager.GetBrush("ABrush.AlertTone3"),
                Foreground = ThemeManager.GetBrush("ABrush.AlertTone5"),
                VerticalAlignment = VerticalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch,
                Width = double.NaN,
                Height = double.NaN,
                Document = new FlowDocument
                {
                    FontFamily = new FontFamily("Segoe UI Emoji"),
                    PagePadding = new Thickness(5),
                    LineHeight = 1,
                },
            };

            Label buttonX = new()
            {
                Name = $"{Default.ID}_X_Button",
                Content = "...",
                FontSize = 12,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Background = Brushes.Transparent,

                Margin = new Thickness(0, 0, 0, -3),
                BorderThickness = new Thickness(0),
                Visibility = Visibility.Collapsed, // Initially hidden
            };
            
            // Set button click handler to close the message panel
            buttonX.MouseLeftButtonDown += (s, e) => 
            {
                if (MessagePanel != null)
                {
                    MessagePanel.Visibility = Visibility.Collapsed;
                    App.MessageText = string.Empty;
                }
            };

            // Show button on hover
            panel.MouseEnter += (s, e) => buttonX.Visibility = Visibility.Visible;
            panel.MouseLeave += (s, e) =>
            {
                buttonX.Visibility = Visibility.Collapsed;
            };

            // Set Grid rows
            Grid.SetRow(viewer, 0);
            Grid.SetRow(buttonX, 1);

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
                Name = $"{Default.ID}_Button_Panel",
                Height = 40,
            };
            
            // Create OK button
            Button buttonOk = new()
            {
                Name = $"{Default.ID}_Ok_Button",
                Content = "OK",
                IsDefault = true,
                Width = 100,
                Height = 25,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };
                
            // Create Cancel button
            Button buttonCancel = new()
            {
                Name = $"{Default.ID}_Cancel_Button",
                Content = "Cancel",
                IsCancel = true,
                Width = 100,
                Height = 25,
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

                var buttonX = MessagePanel.Children.OfType<Label>().FirstOrDefault(x => x.Name == $"{Default.ID}_X_Button");
                var lineCount = App.MessageText.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).Length;
                var lineHeight = SampleLineHeight(MessageViewer);
                var padding = 5; // Additional padding


                if (lineCount <= App.AdvancedProperties.MaxMessageLines)
                {
                    MessagePanel.Height = lineHeight * lineCount + padding;
                    MessageViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                }
                else
                {
                    MessagePanel.Height = lineHeight * App.AdvancedProperties.MaxMessageLines + padding;
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
            // if (ContentPanel == null)
            //     return;

            // double availableWidth = ContentPanel.ActualWidth;;

            // if (double.IsNaN(availableWidth) || availableWidth <= 0)
            // {
            //     availableWidth = App.Width - ((Constants.Default.Spacing != 0 ? Constants.Default.Spacing : 1) * 2 + 20);
            // }

            // if (IsScrollbarVisible())
            // {
            //     availableWidth -= SystemParameters.VerticalScrollBarWidth + 2;
            // }

            // foreach (var element in App.FormElements.Values)
            // {
            //     if (element.PanelControl is FrameworkElement ctrl)
            //     {
            //         ctrl.Width = availableWidth;
            //         ctrl.MinWidth = availableWidth;
            //     }
            // }
        }

        #endregion
    }
}