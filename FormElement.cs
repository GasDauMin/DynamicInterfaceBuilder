namespace DynamicInterfaceBuilder
{    
    public abstract class FormElement
    {
        public readonly FormElementValidation Validation;

        public string Name { get; set; }
        public string? Label { get; set; }
        public string? Description { get; set; }

        public FormElementType Type { get; set; }

        public FormElement(string name, FormElementType type)
        {
            Name = name;
            Type = type;

            Validation = new FormElementValidation(this);
        }

        public static FormElement Construct(string name, FormElementType type)
        {
            return type switch
            {
                FormElementType.TextBox => new FormElement_TextBox(name),
                FormElementType.Numeric => new FormElement_Numeric(name),
                FormElementType.CheckBox => new FormElement_CheckBox(name),
                FormElementType.FileBox => new FormElement_FileBox(name),
                FormElementType.FolderBox => new FormElement_FolderBox(name),
                FormElementType.ListBox => new FormElement_ListBox(name),
                FormElementType.RadioButton => new FormElement_RadioButton(name),
                FormElementType.ComboBox => new FormElement_ComboBox(name),
                _ => throw new ArgumentException($"Invalid form element type: {type}")
            };
        }

        public bool Validate(bool runtime = false)
        {
            return Validation.Validate(runtime);
        }
    }

    public class FormElement_TextBox : FormElement
    {
        public string? DefaultValue { get; set; }

        public FormElement_TextBox(string name) : base(name, FormElementType.TextBox)
        {
        }
    }

    public class FormElement_Numeric : FormElement
    {
        public int? DefaultValue { get; set; }

        public FormElement_Numeric(string name) : base(name, FormElementType.Numeric)
        {
        }
    }

    public class FormElement_CheckBox : FormElement
    {
        public bool? DefaultValue { get; set; }

        public FormElement_CheckBox(string name) : base(name, FormElementType.CheckBox)
        {
        }
    }

    public class FormElement_FileBox : FormElement
    {
        public string? DefaultValue { get; set; }

        public FormElement_FileBox(string name) : base(name, FormElementType.FileBox)
        {
        }
    }

    public class FormElement_FolderBox : FormElement
    {
        public string? DefaultValue { get; set; }

        public FormElement_FolderBox(string name) : base(name, FormElementType.FolderBox)
        {
        }
    }
    
    public class FormElement_ListBox : FormElement
    {
        public string[]? DefaultValue { get; set; }

        public FormElement_ListBox(string name) : base(name, FormElementType.ListBox)
        {   
        }
    }
    
    public class FormElement_RadioButton : FormElement, IListBase
    {        
        private readonly ListBase _listBase = new();
        
        public string[]? Value { get => _listBase.Value; set => _listBase.Value = value; }
        public int DefaultIndex { get => _listBase.DefaultIndex; set => _listBase.DefaultIndex = value; }   
        public string DefaultValue { get => _listBase.DefaultValue; set => _listBase.DefaultValue = value; }
        
        public FormElement_RadioButton(string name) : base(name, FormElementType.RadioButton)
        {
            Value = null;
        }

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
        
        public FormElement_ComboBox(string name) : base(name, FormElementType.ComboBox)
        {
            Value = null;
        }

        public FormElement_ComboBox(string name, string[] value) : base(name, FormElementType.ComboBox)
        {
            Value = value;
        }
    }
}