using System.Windows.Controls;
using System.Windows.Media;

namespace DynamicInterfaceBuilder
{
    public class WpfHelper : ApplicationBase
    {
        public WpfHelper(Application application) : base(application)
        {
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