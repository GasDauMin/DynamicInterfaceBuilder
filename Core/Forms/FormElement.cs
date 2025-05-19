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
        }
    }
}