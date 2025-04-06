using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DynamicInterfaceBuilder
{
    public class MessageManager : ApplicationBase
    {
        public MessageManager(Application application) : base(application)
        {
        }

        public string FormatMessage(string message, MessageType type = MessageType.None)
        {
            string prefix = type switch
            {
                MessageType.Info => "\u2139\uFE0F ", // ℹ️
                MessageType.Alert => "\uD83D\uDD14 ", // 🔔
                MessageType.Success => "\u2705 ", // ✅
                MessageType.Warning => "\u26A0\uFE0F ", // ⚠️
                MessageType.Error => "\u274C ", // ❌
                MessageType.Debug => "\uD83D\uDEE0\uFE0F ", // 🛠️
                _ => ""
            };
            
            return $"{prefix}{message}{Constants.EndLine}";
        }
    }
}