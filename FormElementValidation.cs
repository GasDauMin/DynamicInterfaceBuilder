namespace DynamicInterfaceBuilder
{
    public class FormElementValidation
    {
        public FormElement Element {get; set;}

        public FormElementValidation(FormElement element)
        {
            this.Element = element;
        }   

        public bool Validate(bool runtime = false)
        {
            if (Element == null)
                return false;

            return Element.Type switch
            {
                FormElementType.TextBox     => Validate_TextBox(Element as FormElement_TextBox, runtime),
                FormElementType.RadioButton => Validate_RadioButton(Element as FormElement_RadioButton, runtime),
                FormElementType.ComboBox    => Validate_ComboBox(Element as FormElement_ComboBox, runtime),
                FormElementType.Numeric     => Validate_Numeric(Element as FormElement_Numeric, runtime),
                FormElementType.CheckBox    => Validate_CheckBox(Element as FormElement_CheckBox, runtime),
                FormElementType.FileBox     => Validate_FileBox(Element as FormElement_FileBox, runtime),
                FormElementType.FolderBox   => Validate_FolderBox(Element as FormElement_FolderBox, runtime),
                FormElementType.ListBox     => Validate_ListBox(Element as FormElement_ListBox, runtime),
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