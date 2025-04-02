namespace DynamicInterfaceBuilder
{
    public abstract class FormElementBase : IFormElement
    {
        protected readonly FormBuilder FormBuilder;
        
        public string Name { get; protected set; }
        public string? Label { get; set; }
        public string? Description { get; set; }
        public Control? Control { get; protected set; }
        public FormElementType Type { get; protected set; }
        public List<ValidationRule> ValidationRules { get; protected set; } = [];

        protected FormElementBase(FormBuilder formBuilder, string name, FormElementType type)
        {
            FormBuilder = formBuilder;
            Name = name;
            Type = type;
        }

        public abstract Control? BuildControl();

        public virtual bool ValidateControl()
        {
            bool ok = true;

            foreach (var rule in ValidationRules)
            {
                if (!Validate(rule))
                {
                    if (rule.Message != null)
                    {
                        FormBuilder.PrintError(rule.Message);
                    }
                    
                    ok = false;   
                }
            }

            return ok;
        }

        public virtual bool Validate(ValidationRule rule)
        {
            return true;
        }
    }
}
