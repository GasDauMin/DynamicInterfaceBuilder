using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace DynamicInterfaceBuilder
{
    public class MessageManager : ApplicationBase
    {
        public MessageManager(Application application) : base(application)
        {
        }

        public string FormatMessage(string message, MessageType type = MessageType.None)
        {
            var prefix = type != MessageType.None ? type switch
            {
                MessageType.Debug => "🛠️",
                MessageType.Success => "✅",
                MessageType.Info => "ℹ️",
                MessageType.Warning => "⚠️",
                MessageType.Error => "❌",
                MessageType.Alert => "🔔"
            } + " " : "";
            
            return prefix + message;
        }
            
        public string TypeColorKey(MessageType messageType, ColorType colorType)
        {
            switch (messageType)
            {
                case MessageType.None:
                    if (colorType == ColorType.Background)
                        return "Message.Background";
                    else
                        return "Message.Foreground";
                default:
                    var type = char.ToUpper(messageType.ToString()[0]) + messageType.ToString().Substring(1);
                    var sfx = colorType == ColorType.Background ? "Bg" : "Fg";
                    return $"Message.{type}{sfx}";
            }
        }

        public string ColorKey(ColorType colorType)
        {
            return TypeColorKey(App.MessageType, colorType);
        }

        public void ResetMessage()
        {
                App.MessageText = string.Empty;
                App.MessageType = MessageType.None;
        }

        public void AddMessage(string message, MessageType type = MessageType.None)
        {   
                App.MessageText += (String.IsNullOrEmpty(App.MessageText) ? "" : Constants.EndLine) + FormatMessage(message, type);

                if (App.MessageType < type)
                {
                    App.MessageType = type;
                }
                
        }
    }
}