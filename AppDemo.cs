using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;
using DynamicInterfaceBuilder.Core.Constants;
using DynamicInterfaceBuilder.Core.Enums;

namespace DynamicInterfaceBuilder
{
    public class AppDemo
    {
        [STAThread]
        static void Main()
        {       
            var application = new App()
            {
                Title = Default.Title,
                //Width = Default.Width,
                //Height = Default.Height
            };
            
            application.Parameters["TestGroup"] = new OrderedDictionary
            {
                { "Type", FormElementType.Group },
                { "Label", "Test group" },
                { "Description", "Test group description" },
                // { "Style", new OrderedDictionary
                //     {
                //         { "ValueControl.FontSize", 15 },
                //         { "ValueControl.FontWeight", FontWeights.Bold },
                //         { "ValueControl.FontFamily", new FontFamily("Consolas") }
                //     }
                // },
                { "Elements", new[] {
                        new OrderedDictionary
                        {
                            { "Type", FormElementType.CheckBox },
                            { "Label", "Test check box" },
                            { "Description", "Test check box description" },
                            { "DefaultValue", true },
                        },
                        new OrderedDictionary
                        {
                            { "Type", FormElementType.CheckBox },
                            { "Label", "Test check box" },
                            { "Description", "Test check box description" },
                            { "DefaultValue", true },
                        },
                        new OrderedDictionary
                        {
                            { "Type", FormElementType.TextBox },
                            { "Label", "Test text box" },
                            { "Description", "Test text box description" },
                            { "DefaultValue", "" },
                            { "Validation", new OrderedDictionary
                                {
                                    { "Type", ValidationType.Required },
                                    { "Value", true },
                                    { "Message", "Test validation description" },
                                    { "Runtime", true }
                                }
                            },
                            // { "Style", new OrderedDictionary
                            //     {
                            //         { "ValueControl.Background", Colors.LightGray.ToString() },
                            //         { "ValueControl.Foreground", Colors.Black.ToString() },
                            //     }
                            // },
                        },
                        new OrderedDictionary
                        {
                            { "GroupName", "NestedGroup" },
                            { "Type", FormElementType.Group },
                            { "Label", "Nested group" },
                            { "Description", "Nested group description" },
                            { "Elements", new[] {
                                    new OrderedDictionary
                                    {
                                        { "Type", FormElementType.TextBox },
                                        { "Label", "Nested text box" },
                                        { "Value", "" },
                                        { "Description", "Nested text box description" },
                                        { "Validation", new OrderedDictionary
                                            {
                                                { "Type", ValidationType.Required },
                                                { "Value", true },
                                                { "Message", "Another test validation description" },
                                                { "Runtime", true }
                                            }
                                        },
                                    }
                                }
                            }
                        },
                        new OrderedDictionary
                        {
                            { "Type", FormElementType.ComboBox },
                            { "Label", "Test combobox" },
                            { "Description", "Test combobox description" },
                            { "DefaultValue", "Item 3" },
                            { "Items", new[] { "Item 1", "Item 2", "Item 3" } }
                        },
                        new OrderedDictionary
                        {
                            { "Type", FormElementType.Button },
                            { "Label", "Test Button" },
                            { "Description", "Click to execute action" },
                            { "Action", new Action<App>((app) => {
                                app.MessageText = "Button clicked! All values:\n\n";
                                
                                var elements = app.GetElements(predicate: e => e.ValueControl != null);
                                foreach (var element in elements)
                                {
                                    var value = app.GetElementValue(element.Name);
                                    app.MessageText += $"{element.Name}: {value}\n";
                                }
                                
                                app.MessageType = MessageType.Info;
                                app.FormBuilder?.AdjustMessageViewer();
                            }) }
                        }
                    }
                }
            };

            application.Run();
        }
    }
}