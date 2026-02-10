namespace DynamicInterfaceBuilder.Core.Enums
{
    public enum FormElementType
    {
        Group,
        TextBox,
        ComboBox,
        RadioButton,
        Numeric,
        CheckBox,
        FileBox,
        FolderBox,
        ListBox,
        Button,
    }

    public static class FormElementTypeExtension
    {
        public static string GetName(this FormElementType type)
        {
            return type.ToString();
        }
    }
}