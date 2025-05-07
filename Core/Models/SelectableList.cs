namespace DynamicInterfaceBuilder.Core.Models
{
    public class SelectableList<T>
    {
        public T[]? Items { get; set; }

        private T? _defaultValue;
        private int _defaultIndex = 0;

        public int DefaultIndex { get => _defaultIndex; set => SetDefaultIndex(value); }
        public T? DefaultValue { get => _defaultValue; set => SetDefaultValue(value); }

        private T[]? _defaultIndices;
        private T[]? _defaultValues;

        public T[]? DefaultIndices { get => _defaultIndices; set => _defaultIndices = value; }

        public T[]? DefaultValues { get => _defaultValues; set => _defaultValues = value; }

        public void SetDefaultIndex(int index)
        {
            if (Items != null && index >= 0 && index < Items.Length)
            {
                _defaultIndex = index;
                _defaultValue = Items[index];
            }
            else
            {
                _defaultIndex = index;
                _defaultValue = default;
            }
        }

        public void SetDefaultValue(T? value)
        {
            if (Items != null && Array.Exists(Items, item => EqualityComparer<T>.Default.Equals(item, value)))
            {
                _defaultValue = value;
                _defaultIndex = Array.IndexOf(Items, value);
            }
            else
            {
                _defaultValue = value;
                _defaultIndex = default;
            }
        }
    }
}