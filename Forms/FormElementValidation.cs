namespace DynamicInterfaceBuilder.Forms
{
    public class FormElementValidation
    {
        public FormElement Element {get; set;}

        public FormElementValidation(FormElement element)
        {
            this.Element = element;
        }   

        public bool Validate(FormElement? element = null)
        {
            element ??= Element;
            if (element == null)
                return false;

            switch (element.Type)
            {
                case FormElementType.TextBox:
                    return Validate_TextBox(element as FormElement_TextBox);

                case FormElementType.RadioButton:
                    return Validate_RadioButton(element as FormElement_RadioButton);

                case FormElementType.ComboBox:
                    return Validate_ComboBox(element as FormElement_ComboBox);

                case FormElementType.Numeric:
                    return Validate_Numeric(element as FormElement_Numeric);

                case FormElementType.CheckBox:
                    return Validate_CheckBox(element as FormElement_CheckBox);

                case FormElementType.FileBox:
                    return Validate_FileBox(element as FormElement_FileBox);

                case FormElementType.FolderBox:
                    return Validate_FolderBox(element as FormElement_FolderBox);

                case FormElementType.ListBox:
                    return Validate_ListBox(element as FormElement_ListBox);
            }

            return true;
        }

        private bool Validate_TextBox(FormElement_TextBox? element)
        {
            return element != null;
        }   

        private bool Validate_RadioButton(FormElement_RadioButton? element)
        {
            return element != null;
        }

        private bool Validate_ComboBox(FormElement_ComboBox? element)
        {
            return element != null;
        }

        private bool Validate_Numeric(FormElement_Numeric? element)
        {
            return element != null;
        }

        private bool Validate_CheckBox(FormElement_CheckBox? element)
        {
            return element != null;
        }

        private bool Validate_FileBox(FormElement_FileBox? element)
        {
            return element != null;
        }

        private bool Validate_FolderBox(FormElement_FolderBox? element)
        {
            return element != null;
        }

        private bool Validate_ListBox(FormElement_ListBox? element)
        {
            return element != null;
        }
    }
}