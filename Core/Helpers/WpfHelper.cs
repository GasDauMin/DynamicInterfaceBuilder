using System.Windows.Controls;
using System.Windows.Media;

namespace DynamicInterfaceBuilder
{
    public class WpfHelper : ApplicationBase
    {
        public WpfHelper(Application application) : base(application)
        {
        }

        public void RecolorObject(Object? obj, string colorKey = "", ColorType colorType = ColorType.Background)
        {
            if (obj == null || string.IsNullOrEmpty(colorKey) || !App.ThemeManager.KeyExists(colorKey))
            {
                return;
            }

            if (obj is Control control)
            {
                switch (colorType)
                {
                    case ColorType.Foreground:
                        control.Foreground = App.GetBrush(colorKey);
                        break;
                    case ColorType.Border:
                        control.BorderBrush = App.GetBrush(colorKey);
                        break;
                    case ColorType.Background:
                        control.Background = App.GetBrush(colorKey);
                        break;
                }
            }
            else if (obj is TextBlock textBlock)
            {
                switch (colorType)
                {
                    case ColorType.Foreground:
                        textBlock.Foreground = App.GetBrush(colorKey);
                        break;
                    case ColorType.Background:
                        textBlock.Background = App.GetBrush(colorKey);
                        break;
                }
            }
            else if (obj is Panel panel)
            {
                switch (colorType)
                {
                    case ColorType.Background:
                        panel.Background = App.GetBrush(colorKey);
                        break;
                }
            }
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