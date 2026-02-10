using System.Windows;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Forms;
using System.Management.Automation;

namespace DynamicInterfaceBuilder.Core.Forms.Elements
{
    [FormElement]
    public class ButtonElement(App application, string name, FormElementType type) : FormElementBase(application, name, type)
    {
        /// <summary>
        /// Action to execute when button is clicked (C# delegate).
        /// </summary>
        public Action<App>? Action { get; set; }

        /// <summary>
        /// PowerShell ScriptBlock to execute when button is clicked.
        /// </summary>
        public ScriptBlock? ScriptBlock { get; set; }

        /// <summary>
        /// Generic value that can hold Action or ScriptBlock.
        /// </summary>
        public object? Value { get; set; }

        /// <summary>
        /// Button text override. If not set, uses Label property.
        /// </summary>
        public string? ButtonText { get; set; }

        /// <summary>
        /// Button width. If null, uses default sizing.
        /// </summary>
        public double? ButtonWidth { get; set; }

        /// <summary>
        /// Button height. If null, uses default sizing.
        /// </summary>
        public double? ButtonHeight { get; set; }

        public override UIElement? BuildElement()
        {
            var panel = new Grid
            {
                Name = $"{Name}_Panel",
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 0, 0, 0)
            };

            var button = new Button
            {
                Name = $"{Name}_Button",
                Content = ButtonText ?? Label ?? "Button",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 0, 0, 0),
                Padding = new Thickness(20, 5, 20, 5),
            };

            // Set button dimensions if specified
            if (ButtonWidth.HasValue)
            {
                button.Width = ButtonWidth.Value;
            }

            if (ButtonHeight.HasValue)
            {
                button.Height = ButtonHeight.Value;
            }

            // Set up the click event handler
            button.Click += (sender, e) => ExecuteAction();

            panel.Children.Add(button);

            SetupControls(button, panel, null);

            return panel;
        }

        /// <summary>
        /// Executes the button action if one is defined.
        /// </summary>
        private void ExecuteAction()
        {
            try
            {
                // Try Action property first
                if (Action != null)
                {
                    Action.Invoke(App);
                    return;
                }

                // Try ScriptBlock property
                if (ScriptBlock != null)
                {
                    ScriptBlock.Invoke(App);
                    return;
                }

                // Try Value property - check for various types
                if (Value != null)
                {
                    if (Value is Action<App> actionValue)
                    {
                        actionValue.Invoke(App);
                        return;
                    }
                    else if (Value is ScriptBlock scriptBlockValue)
                    {
                        scriptBlockValue.Invoke(App);
                        return;
                    }
                }

                // No action defined
                System.Diagnostics.Debug.WriteLine($"Button '{Name}' clicked but no action is defined.");
            }
            catch (Exception ex)
            {
                // Display error in message panel if available
                if (App.FormBuilder != null)
                {
                    App.MessageText = $"Button '{Label}' error: {ex.Message}";
                    App.MessageType = MessageType.Error;
                    App.FormBuilder.AdjustMessageViewer();
                }
                
                System.Diagnostics.Debug.WriteLine($"Error executing button action: {ex.Message}");
            }
        }

        public override void SetupElement()
        {
            Tooltip = Description;
            
            // Try to populate Action or ScriptBlock from Value
            if (Value != null)
            {
                if (Value is Action<App> actionValue)
                {
                    Action = actionValue;
                }
                else if (Value is ScriptBlock scriptBlockValue)
                {
                    ScriptBlock = scriptBlockValue;
                }
            }
        }

        public override void ResetElement()
        {
            Tooltip = Description;
        }

        public override void SetupControls(object? valueControl, object? panelControl, object? labelControl)
        {
            if (panelControl != null)
            {
                SetupPanelControl(panelControl);
            }
        }

        public override bool SetupValueControl(object? control)
        {
            return false; // Buttons don't have value controls
        }

        public override bool SetupPanelControl(object? control)
        {
            if (control != null)
            {
                PanelControl = control;
                return true;
            }
            return false;
        }

        public override bool SetupLabelControl(object? control)
        {
            return false; // Buttons don't have label controls
        }

        public override void ResetControls(bool doResetValueControl, bool doResetPanelControl, bool doResetLabelControl)
        {
            if (doResetPanelControl && PanelControl != null)
            {
                ResetPanelControl();
            }
        }

        public override void ResetValueControl()
        {
            // Buttons don't have value controls
        }

        public override void ResetPanelControl()
        {
            PanelControl = null;
        }

        public override void ResetLabelControl()
        {
            // Buttons don't have label controls
        }
    }
}
