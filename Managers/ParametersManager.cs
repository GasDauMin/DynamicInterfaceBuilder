using System.Collections;

namespace DynamicInterfaceBuilder.Managers
{
    public class ParametersManager
    {    
        public Dictionary<string, object> Parameters { get; set; } = new();

        public ParametersManager()
        {
            Parameters = new Dictionary<string, object>();
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
        }

        public void ParseParameter(string id, object value)
        {
            if (value == null || value.GetType() != typeof(Hashtable))
                return;

            var parameter = Parameters[id] = value;
            
        }
    }
}