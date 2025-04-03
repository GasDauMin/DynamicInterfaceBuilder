using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DynamicInterfaceBuilder
{
    public class MessageManager(FormBuilder formBuilder) : INotifyPropertyChanged
    {
        public readonly FormBuilder FormBuilder = formBuilder;
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _messageText = string.Empty;
        
        public string MessageText
        {
            get => _messageText;
            set => SetProperty(ref _messageText, value);
        }

        public void Clear()
        {
            MessageText = string.Empty;
        }

        public void Print(string message, MessageType type = MessageType.None)
        {
            string prefix = type switch
            {
                MessageType.Info => "ℹ️ ",
                MessageType.Alert => "🔔 ",
                MessageType.Success => "✅ ",
                MessageType.Warning => "⚠️ ",
                MessageType.Error => "❌ ",
                MessageType.Debug => "🛠️ ",
                _ => ""
            };
            
            MessageText += $"{prefix}{message}{Constants.EndLine}";
        }

        protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            
            T oldValue = field;
            field = value;
            
            // Notify about the change
            OnPropertyChanged(propertyName, oldValue, value);
            
            return true;
        }

        protected virtual void OnPropertyChanged<T>(string? propertyName, T oldValue, T newValue)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            
            if (propertyName == nameof(MessageText) && oldValue is string oldText && newValue is string newText)
            {
                FormBuilder.OnMessageTextChanged?.Invoke(oldText, newText);
            }
        }
    }
}