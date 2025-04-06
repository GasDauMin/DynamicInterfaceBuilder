using System.Collections;
namespace DynamicInterfaceBuilder
{
    public class ParametersManager : ApplicationBase
    {    
        public Dictionary<string, object> Parameters { get; set; } = [];
        public Dictionary<string, FormElementBase> FormElements { get; set; } = [];

        public ParametersManager(Application application) : base(application)
        {
        }
        
        public void ParseParameters(Dictionary<string, object> parameters)
        {
            if (parameters == null)
                return;

            foreach (var entry in parameters) 
            {
                if (entry.Value == null || entry.Value.GetType() != typeof(Hashtable))
                    continue;

                ParseParameter(entry.Key, entry.Value);
            }
        }

        public void ParseParameter(string id, object data)
        {
            if (data == null)
                return;

            if (data is not Hashtable parameter)
                return;
            
            if (parameter["Type"] is not FormElementType type)   
                return;

            var element = FormElementFactory.Create(type, id, App);

            foreach (DictionaryEntry parameterEntry in parameter)
            {
                switch (parameterEntry.Key.ToString())
                {
                    case "Label":
                        element.Label = parameterEntry.Value as string;
                        break;
                    case "Description":
                        element.Description = parameterEntry.Value as string;
                        break;
                    case "DefaultValue":
                        if (element is FormElement<string> stringElement && parameterEntry.Value is string strValue)
                            stringElement.DefaultValue = strValue;
                        else if (element is FormElement<int> intElement && parameterEntry.Value is int intValue)
                            intElement.DefaultValue = intValue;
                        else if (element is FormElement<bool> boolElement && parameterEntry.Value is bool boolValue)
                            boolElement.DefaultValue = boolValue;
                        break;
                    case "Items":
                        if (element is ISelectableList<string> selectableList && parameterEntry.Value is string[] items)
                            selectableList.Items = items;
                        break;
                    case "Validation":
                        if (parameterEntry.Value is Hashtable[] rules)
                        {
                            foreach (var rule in rules)
                            {
                                ParseValidationRule(rule, element);
                            }
                        }
                        break;
                }
            }

            FormElements[id] = element;
        }
    
        public void ParseValidationRule(Hashtable data, FormElementBase element)
        {
            ValidationRule validationRule = new();

            foreach (DictionaryEntry entry in data)
            {
                switch (entry.Key.ToString())   
                {
                    case "Type":
                        if (entry.Value is ValidationType type)
                        {
                            validationRule.Type = type;
                        }
                        else
                        if (entry.Value is string typeString)
                        {
                            if (Enum.TryParse(typeString, out ValidationType parsedType))
                            {
                                validationRule.Type = parsedType;
                            }
                            else
                            {
                                validationRule.Type = ValidationType.None;
                            }
                        }
                        break;
                    case "Value":
                        validationRule.Value = entry.Value;
                        break;
                    case "Message":
                        validationRule.Message = entry.Value as string;
                        break;
                    case "Runtime":
                        validationRule.Runtime = entry.Value as bool?;
                        break;
                }   
            }

            element.ValidationRules.Add(validationRule);
        }
    }
}