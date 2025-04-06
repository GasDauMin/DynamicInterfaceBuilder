namespace DynamicInterfaceBuilder
{   
    public abstract class FormElement<T>(Application application, string name, FormElementType type) : FormElementBase(application, name, type)
    {
        public virtual T? DefaultValue { get; set; }
        public virtual T? Value { get; set; }

        public override bool ValidateControl()
        {
            bool ok = true;

            foreach (var rule in ValidationRules)
            {
                if (!ValidateRule(rule))
                {
                    if (rule.Message != null)
                    {
                        App.MessageManager.AddMessage(rule.Message, MessageType.Error);
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
}