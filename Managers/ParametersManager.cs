using System.Collections;
namespace DynamicInterfaceBuilder
{
    public class ParametersManager
    {    
        public Dictionary<string, object> Parameters { get; set; } = new();
        public Dictionary<string, FormElement> FormElements { get; set; } = new();

        public ParametersManager()
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

            /*
                $builder.Parameters["InputFile"] = @{
                    Type = "TextBox"
                    Label = "Input File"
                    Description = "The input file to process"
                    Required = $true
                    Validation  = @{
                        Rules = @(
                            @{ Type = "Required"; Message = "Input file is required." },
                            @{ Type = "Regex"; Pattern = "^[a-zA-Z0-9_\\-]+\\.txt$"; Message = "Only .txt files are allowed." },
                            @{ Type = "FileExists"; Message = "File must exist." }
                        )
                    }
                }
            */

            // Check if data is correct

            if (data == null)
                return;

            if (data is not Hashtable parameter)
                return;
            
            if (parameter["Type"] is not FormElementType type)   
                return;

            // Construct the form element

            FormElement element = FormElement.Construct(id, type);

            foreach (DictionaryEntry parameterEntry in parameter)
            {
                if (parameterEntry.Key.ToString() == "Validation" && parameterEntry.Value is System.Object[] rules)
                {
                    foreach (var rule in rules.OfType<Hashtable>())
                    {
                        foreach (DictionaryEntry ruleEntry in rule)
                        {
                            Console.WriteLine($"  {ruleEntry.Key}: {ruleEntry.Value}");
                        }
                    }
                }
                else
                {
                    switch (parameterEntry.Key.ToString())
                    {
                        case "Label":
                            element.Label = parameterEntry.Value as string;
                            break;
                        case "Description":
                            element.Description = parameterEntry.Value as string;
                            break;
                    }
                }
            }

            FormElements[id] = element;
        }
    }
}