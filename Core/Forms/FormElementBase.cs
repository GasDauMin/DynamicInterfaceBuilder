namespace DynamicInterfaceBuilder
{
    public abstract class FormElementBase : ApplicationBase, IFormElementBase
    {   
        public string Name { get; protected set; }
        public string? Label { get; set; }
        public string? Description { get; set; }
        public object? Control { get; protected set; }
        public FormElementType Type { get; protected set; }
        public List<ValidationRule> ValidationRules { get; protected set; } = [];

        protected FormElementBase(Application application, string name, FormElementType type) : base(application)
        {
            Name = name;
            Type = type;
        }

        public abstract object? BuildControl();
        public abstract bool ValidateControl();
    }
}
