using System.Windows;
using System.Windows.Controls;
using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Forms.Validations;
using DynamicInterfaceBuilder.Core.Helpers;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Forms
{
    public abstract class FormElement<T>(App application, string name, FormElementType type) : FormElementBase(application, name, type)
    {
        public virtual T? DefaultValue { get; set; }
        public virtual T? ControlValue
        {
            get
            {
                return ValueControl switch
                {
                    TextBox textBox => TryConvertValue<T>(textBox.Text),
                    ComboBox comboBox => TryConvertValue<T>(comboBox.SelectedItem),
                    CheckBox checkBox => TryConvertValue<T>(checkBox.IsChecked),
                    RadioButton radioButton => TryConvertValue<T>(radioButton.IsChecked),
                    ListBox listBox => TryConvertValue<T>(listBox.SelectedItem),
                    _ => default,
                };
            }
        }
        private static T? TryConvertValue<U>(object? value)
        {
            if (value == null)
                return default;

            try
            {
                // Convert to the actual target type T, not the method type parameter U
                return (T?)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Conversion error: {ex.Message}. Attempted to convert {value?.GetType().Name ?? "null"} to {typeof(T).Name}");
                return default;
            }
        }

        #region Controls

        public override void SetupElement()
        {
            Tooltip = Description;
        }

        public override void ResetElement()
        {
            Tooltip = Description;
        }

        public override void SetupControls(object? valueControl, object? panelControl, object? labelControl)
        {
            if (valueControl != null)
            {
                SetupValueControl(valueControl);
            }

            if (panelControl != null)
            {
                SetupPanelControl(panelControl);
            }

            if (labelControl != null)
            {
                SetupLabelControl(labelControl);
            }
        }

        public override bool SetupValueControl(object? controlObject)
        {
            if (controlObject != null)
            {
                ValueControl = controlObject;

                if (ValueControl is Control control)
                {
                    ApplyValueControlStyles(control);
                    ApplyValueControlValidations(control);
                }

                if (ValueControl is TextBox textBox)
                {
                    textBox.ToolTip = Tooltip;
                }

                return true;
            }

            return false;
        }

        public override bool SetupPanelControl(object? controlObject)
        {
            if (controlObject != null)
            {
                PanelControl = controlObject;

                if (PanelControl is Control control)
                {
                    ApplyPanelControlStyles(control);
                }

                return true;
            }

            return false;
        }

        public override bool SetupLabelControl(object? controlObject)
        {
            if (controlObject != null)
            {
                LabelControl = controlObject;

                if (LabelControl is Control control)
                {
                    ApplyLabelControlStyles(control);
                }

                return true;
            }

            return false;
        }

        public override void ResetControls(bool doResetValueControl, bool doResetPanelControl, bool doResetLabelControl)
        {
            if (doResetValueControl && ValueControl != null)
            {
                ResetValueControl();
            }

            if (doResetPanelControl && PanelControl != null)
            {
                ResetPanelControl();
            }

            if (doResetLabelControl && LabelControl != null)
            {
                ResetLabelControl();
            }
        }

        public override void ResetValueControl()
        {
            if (ValueControl is Control control)
            {
                StyleHelper.ResetControlStyle(control);
                ApplyValueControlStyles(control);
            }

            if (ValueControl is TextBox textBox)
            {
                textBox.ToolTip = Description;
            }
        }

        public override void ResetLabelControl()
        {
            if (LabelControl is Control control)
            {
                StyleHelper.ResetControlStyle(control);
                ApplyLabelControlStyles(control);
            }
        }

        public override void ResetPanelControl()
        {
            if (PanelControl is Control control)
            {
                StyleHelper.ResetControlStyle(control);
                ApplyPanelControlStyles(control);
            }
        }

        #endregion

        protected void ApplyValueControlStyles(Control control)
        {
            StyleHelper.ApplyValueControlStyles(control);
        }

        protected void ApplyPanelControlStyles(Control control)
        {
            StyleHelper.ApplyPanelControlStyles(control);
        }

        protected void ApplyLabelControlStyles(Control control)
        {
            StyleHelper.ApplyLabelControlStyles(control);
        }

        protected void ApplyButtonControlStyles(Control control)
        {
            StyleHelper.ApplyButtonControlStyles(control);
        }

        protected void ApplyValueControlValidations(Control control)
        {
            // For WPF validation, we need to create a proper binding to a data source
            // rather than binding a control to itself which causes circular references
            
            // Only apply validation rules if we have validations to apply
            if (!Validations.Any())
                return;

            // Apply validation through direct event handling rather than binding
            // This avoids the circular binding issue while still providing validation
            
            // Add runtime validation for immediate feedback
            foreach (FormValidationBase validation in Validations)
            {
                // Add appropriate event handlers based on control type for real-time validation
                if (control is TextBox textBox)
                {
                    textBox.TextChanged += (s, e) => ValidateControl(textBox, validation);
                    textBox.LostFocus += (s, e) => ValidateControl(textBox, validation);
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.SelectionChanged += (s, e) => ValidateControl(comboBox, validation);
                }
                else if (control is CheckBox checkBox)
                {
                    checkBox.Checked += (s, e) => ValidateControl(checkBox, validation);
                    checkBox.Unchecked += (s, e) => ValidateControl(checkBox, validation);
                }
                else if (control is RadioButton radioButton)
                {
                    radioButton.Checked += (s, e) => ValidateControl(radioButton, validation);
                    radioButton.Unchecked += (s, e) => ValidateControl(radioButton, validation);
                }
                else if (control is ListBox listBox)
                {
                    listBox.SelectionChanged += (s, e) => ValidateControl(listBox, validation);
                }
            }
        }
        
        private void ValidateControl(Control control, FormValidationBase validation)
        {
            object? value = null;
            
            // Extract value based on control type
            if (control is TextBox textBox)
                value = textBox.Text;
            else if (control is ComboBox comboBox)
                value = comboBox.SelectedItem;
            else if (control is CheckBox checkBox)
                value = checkBox.IsChecked;
            else if (control is RadioButton radioButton)
                value = radioButton.IsChecked;
            else if (control is ListBox listBox)
                value = listBox.SelectedItem;
            
            // Perform validation
            var result = validation.Validate(value, System.Globalization.CultureInfo.CurrentCulture);
            
            // Create a unique validation message key for this specific validation
            string validationKey = $"{Name}_{validation.Type}";
            
            if (!result.IsValid)
            {
                // Use message panel for validation errors
                if (!string.IsNullOrEmpty(result.ErrorContent?.ToString()))
                {
                    // Format and display the validation error using the MessageHelper
                    string errorMessage = $"{Label}: {result.ErrorContent}";
                    
                    // Store validation errors in a dictionary for better management
                    if (App.ValidationErrors == null)
                        App.ValidationErrors = new Dictionary<string, string>();
                    
                    App.ValidationErrors[validationKey] = errorMessage;
                    
                    // Update the message text with all current validation errors
                    UpdateValidationMessages();
                }
            }
            else
            {
                // Clear this specific validation error
                if (App.ValidationErrors?.ContainsKey(validationKey) == true)
                {
                    App.ValidationErrors.Remove(validationKey);
                    
                    // Update the message text with remaining validation errors
                    UpdateValidationMessages();
                }
            }
        }
        
        private void UpdateValidationMessages()
        {
            if (App.ValidationErrors == null || !App.ValidationErrors.Any())
            {
                // Clear validation messages if there are no errors
                if (App.MessageType == DynamicInterfaceBuilder.Core.Enums.MessageType.Error && 
                    (App.MessageText?.Contains("❌") == true))
                {
                    App.MessageText = string.Empty;
                    App.MessageType = DynamicInterfaceBuilder.Core.Enums.MessageType.None;
                }
            }
            else
            {
                // Combine all validation errors
                var errorMessages = App.ValidationErrors.Values
                    .Select(msg => DynamicInterfaceBuilder.Core.Helpers.MessageHelper.FormatMessage(msg, DynamicInterfaceBuilder.Core.Enums.MessageType.Error));
                
                App.MessageText = string.Join(Environment.NewLine, errorMessages);
                App.MessageType = DynamicInterfaceBuilder.Core.Enums.MessageType.Error;
            }
            
            // If the FormBuilder instance is available, adjust the message viewer immediately
            if (App.FormBuilder?.MessageViewer != null)
            {
                App.FormBuilder.AdjustMessageViewer();
            }
        }
    }
}