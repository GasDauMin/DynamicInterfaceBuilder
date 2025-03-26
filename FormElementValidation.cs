namespace DynamicInterfaceBuilder
{
    public class FormElementValidation
    {
        public List<FormValidationType> Rules { get; set; } = new();
        public FormElementValidation()
        {
        }   

        public bool Validate(FormElement element, bool runtime = false)
        {
            if (element == null)
                return false;

            return element.Type switch
            {
                FormElementType.TextBox     => Validate_TextBox(element as FormElement_TextBox, runtime),
                FormElementType.RadioButton => Validate_RadioButton(element as FormElement_RadioButton, runtime),
                FormElementType.ComboBox    => Validate_ComboBox(element as FormElement_ComboBox, runtime),
                FormElementType.Numeric     => Validate_Numeric(element as FormElement_Numeric, runtime),
                FormElementType.CheckBox    => Validate_CheckBox(element as FormElement_CheckBox, runtime),
                FormElementType.FileBox     => Validate_FileBox(element as FormElement_FileBox, runtime),
                FormElementType.FolderBox   => Validate_FolderBox(element as FormElement_FolderBox, runtime),
                FormElementType.ListBox     => Validate_ListBox(element as FormElement_ListBox, runtime),
                _ => true,
            };
        }

        private bool Validate_TextBox(FormElement_TextBox? element, bool runtime = false)
        {
            return element != null;
        }   

        private bool Validate_RadioButton(FormElement_RadioButton? element, bool runtime = false)
        {
            return element != null;
        }

        private bool Validate_ComboBox(FormElement_ComboBox? element, bool runtime = false)
        {
            return element != null;
        }

        private bool Validate_Numeric(FormElement_Numeric? element, bool runtime = false)
        {
            return element != null;
        }

        private bool Validate_CheckBox(FormElement_CheckBox? element, bool runtime = false)
        {
            return element != null;
        }

        private bool Validate_FileBox(FormElement_FileBox? element, bool runtime = false)
        {
            return element != null;
        }

        private bool Validate_FolderBox(FormElement_FolderBox? element, bool runtime = false)
        {
            return element != null;
        }

        private bool Validate_ListBox(FormElement_ListBox? element, bool runtime = false)
        {
            return element != null;
        }
    }
}