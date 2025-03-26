namespace DynamicInterfaceBuilder
{
    public class SelectableList
    {
        public string[]? Value { get; set; }

        private string _defaultValue = "";
        private int _defaultIndex = 0;

        public int DefaultIndex
        {
            get => _defaultIndex;
            set
            {
                _defaultIndex = value;
                if (Value != null && value >= 0 && value < Value.Length)
                {
                    _defaultValue = Value[value];
                }
                else
                {
                    _defaultValue = "";
                    _defaultIndex = 0;
                }
            }
        }

        public string DefaultValue
        {
            get => _defaultValue;
            set
            {
                _defaultValue = value;
                if (Value != null && Array.Exists(Value, item => item == value))
                {
                    _defaultIndex = Array.IndexOf(Value, value);
                }
                else
                {
                    _defaultIndex = 0;
                }
            }
        }
    }
}