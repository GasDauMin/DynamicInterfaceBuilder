using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DynamicInterfaceBuilder.Core.Attributes;
using DynamicInterfaceBuilder.Core.Constants;
using DynamicInterfaceBuilder.Core.Form.Elements;
using DynamicInterfaceBuilder.Core.Form.Enums;
using DynamicInterfaceBuilder.Core.Form.Helpers;
using DynamicInterfaceBuilder.Core.Form.Models;
using DynamicInterfaceBuilder.Core.Managers;

namespace DynamicInterfaceBuilder.Core.Form
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
            try
            {
                return (T?)Convert.ChangeType(value, typeof(U));
            }
            catch
            {
                return default;
            }
        }

        #region Controls

        public override void SetupElement()
        {
            Valid = true;   
            Tooltip = Description;
        }

        public override void ResetElement()
        {
            Valid = true;
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
                    StyleHelper.ApplyValueControlStyles(control, StyleProperties);
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
                    StyleHelper.ApplyPanelControlStyles(control, StyleProperties);
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
                    StyleHelper.ApplyLabelControlStyles(control, StyleProperties);
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
                StyleHelper.ApplyValueControlStyles(control, StyleProperties);
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
                StyleHelper.ApplyLabelControlStyles(control, StyleProperties);
            }
        }

        public override void ResetPanelControl()
        {
            if (PanelControl is Control control)
            {
                StyleHelper.ResetControlStyle(control);
                StyleHelper.ApplyPanelControlStyles(control, StyleProperties);
            }
        }

        #endregion

        #region Validation

        public override bool ValidateElement()
        {
            Valid = true;

            // Initialize validation

            var attribute = GetType().GetCustomAttributes(typeof(FormElementAttribute), true).FirstOrDefault() as FormElementAttribute;

            bool allowValidationControl = attribute?.AllowValidationControl ?? Default.AllowValidationControl;
            if (allowValidationControl)
            {
                string tooltipValue = String.Empty;  

                Control? alertControl = ValueControl as Control;
                bool allowAlertControl = attribute?.AllowAlertControl ?? Default.AllowAlertControl;
                
                if (allowAlertControl)
                {
                    ResetElement();
                    ResetValueControl();
                }

                // Validate rules

                foreach (var rule in ValidationRules)
                {
                    if (!ValidateRule(rule))
                    {
                        Valid = false;
                        if (rule.Message != null)
                        {
                            App.MessageHelper.AddMessage(rule.Message, MessageType.Error);
                            tooltipValue += (tooltipValue == String.Empty ? "" : General.EndLine) + MessageHelper.FormatMessage(rule.Message, MessageType.Error);             
                        }
                        
                    }
                }

                if (allowAlertControl && !Valid)
                {   
                    alertControl!.ToolTip += (alertControl!.ToolTip.ToString() == String.Empty ? "" : General.EndLine) + tooltipValue;         
                    StyleHelper.ApplyValueControlAlertStyle(alertControl, StyleProperties);
                }
            }

            return Valid;
        }

        public virtual bool ValidateRule(FormElementValidationRule rule)
        {
            // if (typeof(T) == typeof(string))
            // {
            //     //return rule.ValidateText(Value.ToString());
            // }

            return true;
        }

        #endregion
    }
}