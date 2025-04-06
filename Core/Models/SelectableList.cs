namespace DynamicInterfaceBuilder
{
    public class SelectableList<T>
    {
        public T[]? Items { get; set; }

        private T? _defaultValue;
        private int _defaultIndex = 0;

        public int DefaultIndex
        {
            get => _defaultIndex;
            set
            {
                _defaultIndex = value;
                if (Items != null && value >= 0 && value < Items.Length)
                {
                    _defaultValue = Items[value];
                }
                else
                {
                    _defaultValue = default;
                    _defaultIndex = 0;
                }
            }
        }

        public T? DefaultValue
        {
            get => _defaultValue;
            set
            {
                _defaultValue = value;
                if (Items != null && Array.Exists(Items, item => EqualityComparer<T>.Default.Equals(item, value)))
                {
                    _defaultIndex = Array.IndexOf(Items, value);
                }
                else
                {
                    _defaultValue = default;
                    _defaultIndex = 0;
                }
            }
        }
    }
}