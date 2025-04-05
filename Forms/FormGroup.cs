using System.Windows;
using System.Windows.Controls;

namespace DynamicInterfaceBuilder
{
    public class FormGroup(FormBuilder formBuilder, string groupName)
    {
        private readonly List<FormElementBase> _elements = new();
        private readonly FormBuilder _formBuilder = formBuilder;

        public string GroupName { get; set; } = groupName;
        public string? GroupLabel { get; set; }

        public void AddElement(FormElementBase element)
        {
            _elements.Add(element);
        }

        public UIElement BuildGroupControl()
        {
            var panel = new StackPanel
            {
                Name = $"{GroupName}_GroupPanel",
                VerticalAlignment = VerticalAlignment.Top,
                Orientation = Orientation.Vertical
            };

            if (!string.IsNullOrEmpty(GroupLabel))
            {
                var groupLabel = new TextBlock
                {
                    Name = $"{GroupName}_GroupLabel",
                    Text = GroupLabel,
                    FontWeight = FontWeights.Bold,
                    FontSize = 12,
                    VerticalAlignment = VerticalAlignment.Top
                };
                panel.Children.Add(groupLabel);
            }

            foreach (var element in _elements)
            {
                var control = element.BuildControl();
                if (control != null)
                {
                    panel.Children.Add(control);
                }
            }

            return panel;
        }
    }
}
