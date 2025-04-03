namespace DynamicInterfaceBuilder
{   
    #region FormElement
    public abstract class FormElement<T>(FormBuilder formBuilder, string name, FormElementType type) : FormElementBase(formBuilder, name, type)
    {
        public virtual T? DefaultValue { get; set; }
        public virtual T? Value { get; set; }

        public override bool ValidateControl()
        {
            bool ok = true;

            foreach (var rule in ValidationRules)
            {
                FB.ClearMessages();

                if (!ValidateRule(rule))
                {
                    if (rule.Message != null)
                    {
                        FB.PrintError(rule.Message);
                    }
                    
                    ok = false;   
                }
            }

            return ok;
        }

        public virtual bool ValidateRule(ValidationRule rule)
        {
            return false;
        }
    }
    #endregion

    #region TextBoxElement
    public class TextBoxElement(FormBuilder formBuilder, string name) : FormElement<string>(formBuilder, name, FormElementType.TextBox)
    {
        public override Control? BuildControl()
        {
            var panel = new TableLayoutPanel
            {
                Name = $"{Name}_Panel",
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoScroll = false,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 0),
                Padding = new Padding(0, 0, 0, 0),
            };

            int offset = FB.Spacing * 2 + 16;
            int minWidth = FB.Width - offset;
        
            panel.ColumnStyles.Clear();
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75F));
            panel.MinimumSize = new Size(minWidth, 0);

            if (Label != null)
            {
                var label = new Label
                {
                    Name = $"{Name}_Label",
                    Dock = DockStyle.Fill,
                    Text = Label + ": ",
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Margin = new Padding(0, 0, 0, 0),
                    Padding = new Padding(0, 0, 0, 0),
                };

                panel.Controls.Add(label, 0, 0);
            }
            
            var textBox = new TextBox
            {
                Name = $"{Name}_TextBox",
                Dock = DockStyle.Fill,
                AutoSize = true,
                Text = DefaultValue,
                Margin = new Padding(0, 0, 0, 0),
                Padding = new Padding(0, 0, 0, 0),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = FB.GetThemeColor("ControlBack"),
                ForeColor = FB.GetThemeColor("ControlFore"),
            };

            panel.Controls.Add(textBox, 1, 0);

            Control = panel;
            return Control;
        }
    }
    #endregion

    #region NumericElement
    public class NumericElement(FormBuilder formBuilder, string name) : FormElement<int>(formBuilder, name, FormElementType.Numeric)
    {
        public override Control? BuildControl()
        {
            var panel = new TableLayoutPanel
            {
                Name = $"{Name}_Panel",
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoScroll = true,
                ColumnCount = 2,
                RowCount = 1,
            };

            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75F));

            if (Label != null)
            {
                var label = new Label
                {
                    Name = $"{Name}_Label",
                    Text = Label,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill
                };

                panel.Controls.Add(label, 0, 0);
            }

            var numericUpDown = new NumericUpDown
            {
                Name = $"{Name}_Numeric",
                Value = DefaultValue,
                Dock = DockStyle.Fill
            };

            panel.Controls.Add(numericUpDown, 1, 0);

            Control = panel;
            return Control;
        }
    }
    #endregion

    #region CheckBoxElement
    public class CheckBoxElement(FormBuilder formBuilder, string name) : FormElement<bool>(formBuilder, name, FormElementType.CheckBox)
    {

        public override Control? BuildControl()
        {
            var checkBox = new CheckBox
            {
                Name = $"{Name}_CheckBox",
                Text = Label,
                Checked = DefaultValue,
                Dock = DockStyle.Top
            };

            Control = checkBox;
            return Control;
        }
    }
    #endregion

    #region FileBoxElement
    public class FileBoxElement(FormBuilder formBuilder, string name) : FormElement<string>(formBuilder, name, FormElementType.FileBox)
    {
        public override Control? BuildControl()
        {
            // Implement file box control
            return Control;
        }
    }
    #endregion

    #region FolderBoxElement
    public class FolderBoxElement(FormBuilder formBuilder, string name) : FormElement<string>(formBuilder, name, FormElementType.FolderBox)
    {
        public override Control? BuildControl()
        {
            // Implement folder box control
            return Control;
        }
    }
    #endregion

    #region ListBoxElement
    public class ListBoxElement(FormBuilder formBuilder, string name) : FormElement<string>(formBuilder, name, FormElementType.ListBox), ISelectableList<string>
    {
        private readonly SelectableList<string> _selectableList = new();

        public string[]? Items { get => _selectableList.Items; set => _selectableList.Items = value; }
        public int DefaultIndex { get => _selectableList.DefaultIndex; set => _selectableList.DefaultIndex = value; }
        public override string? DefaultValue { get => _selectableList.DefaultValue; set => _selectableList.DefaultValue = value; }

        public override Control? BuildControl()
        {
            // Implement ListBox control
            return Control;
        }
    }
    #endregion

    #region ComboBoxElement
    public class ComboBoxElement(FormBuilder formBuilder, string name) : FormElement<string>(formBuilder, name, FormElementType.ComboBox), ISelectableList<string>
    {
        private readonly SelectableList<string> _selectableList = new();

        public string[]? Items { get => _selectableList.Items; set => _selectableList.Items = value; }
        public int DefaultIndex { get => _selectableList.DefaultIndex; set => _selectableList.DefaultIndex = value; }
        public override string? DefaultValue { get => _selectableList.DefaultValue; set => _selectableList.DefaultValue = value; }

        public override Control? BuildControl()
        {
            // Implement ComboBox control
            return Control;
        }
    }
    #endregion

    #region RadioButtonElement
    public class RadioButtonElement(FormBuilder formBuilder, string name) : FormElement<string>(formBuilder, name, FormElementType.RadioButton), ISelectableList<string>
    {
        private readonly SelectableList<string> _selectableList = new();

        public string[]? Items { get => _selectableList.Items; set => _selectableList.Items = value; }
        public int DefaultIndex { get => _selectableList.DefaultIndex; set => _selectableList.DefaultIndex = value; }
        public override string? DefaultValue { get => _selectableList.DefaultValue; set => _selectableList.DefaultValue = value; }

        public override Control? BuildControl()
        {
            // Implement RadioButton control
            return Control;
        }
    }
    #endregion
}