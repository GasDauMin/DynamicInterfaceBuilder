namespace DynamicInterfaceBuilder
{
    public enum FormElementType
    {
        TextBox,
        ComboBox,
        RadioButton,
        Numeric,
        CheckBox,
        FileBox,
        FolderBox,
        ListBox,
    }

    public enum FormValidationType
    {
        Required,
        Regex,
        FileExists,
        DirectoryExists
    }
}