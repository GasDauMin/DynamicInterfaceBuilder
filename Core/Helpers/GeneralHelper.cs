using System.Windows.Media;
using DynamicInterfaceBuilder.Core.Models;
using Newtonsoft.Json;

namespace DynamicInterfaceBuilder.Core.Helpers
{
    public class GeneralHelper : AppBase {
        public GeneralHelper(App application) : base(application)
        {
        }

        public static T Clone<T>(T source) where T : class, new()
        {
            if (source == null)
                return new T();

            var json = JsonConvert.SerializeObject(source);
            var clone = JsonConvert.DeserializeObject<T>(json);

            return clone ?? new T();
        }

        public bool IsValidColor(string color = "")
        {
            if (string.IsNullOrEmpty(color))
                return false;
            
            try
            {
                return ColorConverter.ConvertFromString(color) is Color;
            }
            catch
            {
                return false;
            }
        }

        public Color ChangeBrightness(Color color, double factor)
        {
            return Color.FromArgb(
                color.A,
                (byte)Math.Clamp((int)(color.R * (1 + factor)), 0, 255),
                (byte)Math.Clamp((int)(color.G * (1 + factor)), 0, 255),
                (byte)Math.Clamp((int)(color.B * (1 + factor)), 0, 255)
            );
        }
    }
}