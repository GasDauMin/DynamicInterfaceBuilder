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

        public Control BuildGroupControl()
        {
            var panel = new FlowLayoutPanel
            {
                Name = $"{GroupName}_GroupPanel",
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            if (!string.IsNullOrEmpty(GroupLabel))
            {
                var groupLabel = new Label
                {
                    Name = $"{GroupName}_GroupLabel",
                    Text = GroupLabel,
                    Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold),
                    Dock = DockStyle.Top
                };
                panel.Controls.Add(groupLabel);
            }

            foreach (var element in _elements)
            {
                panel.Controls.Add(element.BuildControl());
            }

            return panel;
        }
    }
}
