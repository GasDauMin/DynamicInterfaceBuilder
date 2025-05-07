using DynamicInterfaceBuilder.Core.Constants;
using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Helpers
{
    public class MessageHelper : AppBase
    {
        public MessageHelper(App application) : base(application)
        {
        }

        public static string FormatMessage(string message, MessageType type = MessageType.None)
        {
            var prefix = type switch
            {
                MessageType.Debug => "🛠️",
                MessageType.Success => "✅",
                MessageType.Info => "ℹ️",
                MessageType.Warning => "⚠️",
                MessageType.Error => "❌",
                MessageType.Alert => "🔔",
                _ => string.Empty
            };
            
            return prefix + message;
        }

        public void ResetMessage()
        {
            App.MessageText = string.Empty;
            App.MessageType = MessageType.None;
        }

        public void AddMessage(string message, MessageType type = MessageType.None)
        {   
            App.MessageText += (string.IsNullOrEmpty(App.MessageText) ? "" : Default.EndLine) + FormatMessage(message, type);

            if (App.MessageType < type)
            {
                App.MessageType = type;
            }
        }
    }
}