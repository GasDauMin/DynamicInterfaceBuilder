namespace TheToolkit
{    
    public abstract class FormElement
    {
        public string Name { get; set; }
        public FormElementType Type { get; set; }
        public bool Validate()
        {
            return new FormElementValidation(this).Validate();
        }

        public FormElement(string name, FormElementType type)
        {
            Name = name;
            Type = type;
        }
    }

    public interface IListBase
    {
        string[]? Value { get; set; }
        int DefaultIndex { get; set; }
        string DefaultValue { get; set; }
    }

    public class ListBase
    {
        public string[]? Value { get; set; }

        private string _defaultValue = "";
        private int _defaultIndex = 0;

        public int DefaultIndex
        {
            get => _defaultIndex;
            set
            {
                _defaultIndex = value;
                if (Value != null && value >= 0 && value < Value.Length)
                {
                    _defaultValue = Value[value];
                }
                else
                {
                    _defaultValue = "";
                    _defaultIndex = 0;
                }
            }
        }

        public string DefaultValue
        {
            get => _defaultValue;
            set
            {
                _defaultValue = value;
                if (Value != null && Array.Exists(Value, item => item == value))
                {
                    _defaultIndex = Array.IndexOf(Value, value);
                }
                else
                {
                    _defaultIndex = 0;
                }
            }
        }
    }

    public class FormElement_TextBox : FormElement
    {
        public string Value { get; set; }

        public FormElement_TextBox(string name, string value) : base(name, FormElementType.TextBox)
        {
            Value = value;
        }
    }

    public class FormElement_Numeric : FormElement
    {
        public int Value { get; set; }

        public FormElement_Numeric(string name, int value) : base(name, FormElementType.Numeric)
        {
            Value = value;
        }
    }

    public class FormElement_CheckBox : FormElement
    {
        public bool Value { get; set; }

        public FormElement_CheckBox(string name, bool value) : base(name, FormElementType.CheckBox)
        {
            Value = value;
        }
    }

    public class FormElement_FileBox : FormElement
    {
        public string Value { get; set; }

        public FormElement_FileBox(string name, string value) : base(name, FormElementType.FileBox)
        {
            Value = value;
        }
    }

    public class FormElement_FolderBox : FormElement
    {
        public string Value { get; set; }

        public FormElement_FolderBox(string name, string value) : base(name, FormElementType.FolderBox)
        {
            Value = value;
        }
    }
    
    public class FormElement_ListBox : FormElement
    {
        public string[] Value { get; set; }

        public FormElement_ListBox(string name, string[] value) : base(name, FormElementType.ListBox)
        {   
            Value = value;
        }
    }
    
    public class FormElement_RadioButton : FormElement, IListBase
    {        
        private readonly ListBase _listBase = new();
        
        public string[]? Value { get => _listBase.Value; set => _listBase.Value = value; }
        public int DefaultIndex { get => _listBase.DefaultIndex; set => _listBase.DefaultIndex = value; }   
        public string DefaultValue { get => _listBase.DefaultValue; set => _listBase.DefaultValue = value; }
        
        public FormElement_RadioButton(string name, string[] value) : base(name, FormElementType.RadioButton)
        {
            Value = value;
        }
    }

    public class FormElement_ComboBox : FormElement, IListBase
    {        
        private readonly ListBase _listBase = new();
        
        public string[]? Value { get => _listBase.Value; set => _listBase.Value = value; }
        public int DefaultIndex { get => _listBase.DefaultIndex; set => _listBase.DefaultIndex = value; }   
        public string DefaultValue { get => _listBase.DefaultValue; set => _listBase.DefaultValue = value; }
        
        public FormElement_ComboBox(string name, string[] value) : base(name, FormElementType.ComboBox)
        {
            Value = value;
        }
    }
}