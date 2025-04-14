using System.Collections;
using System.Collections.Specialized;
using DynamicInterfaceBuilder.Core.Form;
using DynamicInterfaceBuilder.Core.Form.Enums;
using DynamicInterfaceBuilder.Core.Form.Interfaces;
using DynamicInterfaceBuilder.Core.Form.Models;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Managers
{
    public class ParametersManager : AppBase {

        private static readonly string[] ParsingOrder = new[]
        {
            "Items",
            "DefaultIndex",
            "DefaultValue",
            "Elements",
            "Validation",
        };

        public Dictionary<string, object> Parameters { get; set; } = [];
        public Dictionary<string, FormElementBase> FormElements { get; set; } = [];

        public ParametersManager(App application) : base(application)
        {
        }
        
        public void ParseParameters(Dictionary<string, object> parameters)
        {
            if (parameters == null)
                return;

            foreach (var entry in parameters) 
            {
                if (entry.Value is OrderedDictionary || entry.Value is Hashtable)
                {
                    ParseParameter(entry.Key, entry.Value);
                }
            }
        }

        public void ParseParameter(string id, object data)
        {
            if (data == null)
                return;

            IDictionary parameter = data switch
            {
                OrderedDictionary od => od,
                Hashtable ht => ht,
                _ => throw new NotImplementedException(),
            };
                    
            if (parameter["Type"] is not FormElementType type)   
                return;

            var element = FormElementFactory.Create(type, id, App);

            // First pass - ordered properties
            foreach (var propertyName in ParsingOrder)
            {
                if (!parameter.Contains(propertyName))
                    continue;

                ParseProperty(id, element, new DictionaryEntry { Key = propertyName, Value = parameter[propertyName] });
            }

            // Second pass - remaining properties
            foreach (DictionaryEntry entry in parameter)
            {
                if (ParsingOrder.Contains(entry.Key.ToString()) || entry.Key.ToString() == "Type")
                    continue;

                ParseProperty(id, element, entry);
            }

            element.SetupElement();
            FormElements[id] = element;
        }
    
        private void ParseProperty(string id, FormElementBase element, DictionaryEntry entry)
        {   
            if (entry.Value == null)
                return;

            switch (entry.Key.ToString())
            {
                case "Label":
                    element.Label = entry.Value as string;
                    break;
                case "Description":
                    element.Description = entry.Value as string;
                    break;
                case "DefaultValue":
                    if (element is FormElement<string> stringElement && entry.Value is string strValue)
                        stringElement.DefaultValue = strValue;
                    else if (element is FormElement<int> intElement && entry.Value is int intValue)
                        intElement.DefaultValue = intValue;
                    else if (element is FormElement<bool> boolElement && entry.Value is bool boolValue)
                        boolElement.DefaultValue = boolValue;
                    break;
                case "DefaultIndex":
                case "Items":
                    ParseSelectableList(element, entry);
                    break;
                case "Elements":
                    if (element is IFormGroup group && entry.Value is IEnumerable groupElements)
                    {
                        foreach (IDictionary groupElement in groupElements)
                        {
                            var typeId = groupElement["Type"]?.ToString() ?? string.Empty;
                            var childId = FormBuilder.GenerateUniqueId(id,typeId);

                            ParseParameter(childId, groupElement);
                            
                            if (FormElements.TryGetValue(childId, out var childElement))
                            {
                                group.AddChild(childElement);
                            }
                        }
                    }
                    break;
                case "Validation":
                    if (entry.Value is IEnumerable rules)
                    {
                        foreach (IDictionary rule in rules)
                        {
                            ParseValidationRule(element, rule);
                        }
                    }
                    break;
            }
        }

        private void ParseSelectableList(FormElementBase element, DictionaryEntry entry)
        {
            if (element is not ISelectableList<string> selectableList)
                return;

            switch (entry.Key.ToString())
            {
                case "DefaultIndex":
                    if (entry.Value is int index)
                        selectableList.DefaultIndex = index;
                    break;

                case "Items":
                    if (entry.Value is string[] items)
                        selectableList.Items = items;
                    else if (entry.Value is List<string> itemList)
                        selectableList.Items = itemList.ToArray();

                    break;
            }        
        }

        private void ParseValidationRule(FormElementBase element, IDictionary data)
        {
            FormElementValidationRule validationRule = new();

            foreach (DictionaryEntry entry in data)
            {
                switch (entry.Key.ToString())   
                {
                    case "Type":
                        if (entry.Value is FormElementValidationType type)
                        {
                            validationRule.Type = type;
                        }
                        else
                        if (entry.Value is string typeString)
                        {
                            if (Enum.TryParse(typeString, out FormElementValidationType parsedType))
                            {
                                validationRule.Type = parsedType;
                            }
                            else
                            {
                                validationRule.Type = FormElementValidationType.None;
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