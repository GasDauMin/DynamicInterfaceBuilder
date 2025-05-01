using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DynamicInterfaceBuilder.Core.Form;
using DynamicInterfaceBuilder.Core.Form.Enums;
using DynamicInterfaceBuilder.Core.Form.Interfaces;
using DynamicInterfaceBuilder.Core.Form.Models;
using DynamicInterfaceBuilder.Core.Helpers;
using DynamicInterfaceBuilder.Core.Models;
using System.Reflection;

namespace DynamicInterfaceBuilder.Core.Managers
{
    public class ParametersManager : AppBase {

        private static readonly string[] ParsingOrder = new[]
        {
            "Items",
            "DefaultIndex",
            "DefaultValue"
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

        public void ParseParameter(string id, object data, IFormGroup? group = null)
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

            var element = FormElementFactory.Create(type, App.SetElementId(id), App);

            // Set parent-child relationship
            if (group is FormElementBase parentElement)
            {
                element.SetParent(parentElement);
            }

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

            if (group != null)
            {
                group.Elements[id] = element;      
            }
            else
            {
                FormElements[id] = element;
            }
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
                case "Value":
                case "DefaultValue":
                    if (element is ISelectableList<string>)
                    {
                        ParseSelectableList(element, entry);
                    }
                    else
                    {
                        ParseDefaultValue(element, entry);
                    }
                    break;
                case "DefaultIndex":
                case "Items":
                    if (element is ISelectableList<string>)
                    {
                        ParseSelectableList(element, entry);
                    }
                    break;
                case "Elements":
                    if (element is IFormGroup group && entry.Value is IEnumerable groupElements)
                    {
                        ParseGroupElements(group, groupElements);
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
                case "Style":
                    if (entry.Value is IDictionary styleData)
                    {
                        ParseStyle(element, styleData);
                    }
                    break;
            }
        }

        private void ParseGroupElements(IFormGroup group, IEnumerable groupElements)
        {
            foreach (IDictionary groupElementData in groupElements)
            {
                if (groupElementData["Type"] is FormElementType elementType)
                {
                    string? groupElementId;
                    if (groupElementData["Name"] is string exactElementName)
                    {
                        groupElementId = exactElementName;
                    }
                    else
                    if (groupElementData["GroupName"] is string groupElementName)
                    {
                        groupElementId = App.GenerateElementId(group.Name, groupElementName);
                    }
                    else
                    {
                        groupElementId = App.GenerateElementId(group.Name, elementType.GetName());
                    }

                    ParseParameter(groupElementId, groupElementData, group);
                }
            }
        }

        private void ParseDefaultValue(FormElementBase element, DictionaryEntry entry)
        {
            if (entry.Value is string valueString)
                element.TrySetProperty("DefaultValue", valueString);
            else if (entry.Value is int valueInteger)
                element.TrySetProperty("DefaultValue", valueInteger);
            else if (entry.Value is bool valueBoolean)
                element.TrySetProperty("DefaultValue", valueBoolean);
        }

        private void ParseSelectableList(FormElementBase element, DictionaryEntry entry)
        {
            if (element is not  ISelectableList<string> selectableList)
            {
                return;
            }

            switch (entry.Key.ToString())
            {
                case "DefaultValue":
                    if (entry.Value is string defaultValue)
                        selectableList.DefaultValue = defaultValue;
                    break;
                case "DefaultIndex":
                    if (entry.Value is int defaultIndex)
                        selectableList.DefaultIndex = defaultIndex;
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

        private (PropertyInfo? Property, object? TargetObject) GetPropertyFromPath(object rootObject, string propertyPath)
        {
            string[] propertyParts = propertyPath.Split('.');
            object currentObject = rootObject;
            PropertyInfo? property = null;

            for (int i = 0; i < propertyParts.Length; i++)
            {
                if (currentObject == null)
                    break;

                property = currentObject.GetType().GetProperty(propertyParts[i]);
                if (property == null)
                    break;

                if (i < propertyParts.Length - 1)
                {
                    object? value = property.GetValue(currentObject);
                    if (value == null)
                        break;
                    
                    currentObject = value;
                }
            }

            return (property, currentObject);
        }

        private void ParseStyle(FormElementBase element, IDictionary styleData)
        {
            foreach (DictionaryEntry entry in styleData)
            {
                string propertyPath = entry.Key.ToString() ?? string.Empty;

                if (entry.Value == null)
                    continue;
                    
                try
                {
                    var (property, targetObject) = GetPropertyFromPath(element.StyleProperties, propertyPath);

                    if (property != null && property.CanWrite && targetObject != null)
                    {
                        object? convertedValue = TypeConversionHelper.ConvertValueToType(entry.Value, property.PropertyType);
                        
                        if (convertedValue != null)
                        {
                            property.SetValue(targetObject, convertedValue);
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Property {propertyPath} not found on StyleProperties");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error setting style property {propertyPath}: {ex.Message}");
                }
            }
        }
    }
}