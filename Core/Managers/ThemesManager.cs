using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DynamicInterfaceBuilder.Core.Models;
using DynamicInterfaceBuilder.Core.UI.Enums;

namespace DynamicInterfaceBuilder.Core.Managers
{
    public class ThemesManager : AppBase
    {
        public ThemesManager(App application) : base(application)
        {
            Init();
        }

        private void Init()
        {
            var resources = new ResourceDictionary();

            try
            {
                resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/DynamicInterfaceBuilder;component/Core/UI/Themes/Default.xaml", UriKind.Absolute)
                });
                resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/DynamicInterfaceBuilder;component/Core/UI/Structures/ControlColours.xaml", UriKind.Absolute)
                });
                resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/DynamicInterfaceBuilder;component/Core/UI/Structures/Controls.xaml", UriKind.Absolute)
                });

                System.Windows.Application.Current.Resources = resources;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load theme resources: {ex.Message}");
                throw; // Re-throw as this is a critical error
            }
        }

        private static ResourceDictionary ThemeDictionary
        {
            get => Application.Current.Resources.MergedDictionaries[0];
            set => Application.Current.Resources.MergedDictionaries[0] = value;
        }

        private static ResourceDictionary ControlColours
        {
            get => Application.Current.Resources.MergedDictionaries[1];
            set => Application.Current.Resources.MergedDictionaries[1] = value;
        }

        public static void SetTheme(ThemeType theme)
        {
            string themeName = theme.GetName();
            if (string.IsNullOrEmpty(themeName))
            {
                return;
            }

            //CurrentTheme = theme;

            ThemeDictionary = new ResourceDictionary() { Source = new Uri($"Core/UI/Themes/{themeName}.xaml", UriKind.Relative) };
            ControlColours = new ResourceDictionary() { Source = new Uri("Core/UI/Structures/ControlColours.xaml", UriKind.Relative) };

            RefreshControls();
        }

        private static void RefreshControls()
        {
            // This seems to be faster than reloading the whole file, and it also seems to work
            Collection<ResourceDictionary> merged = Application.Current.Resources.MergedDictionaries;
            ResourceDictionary dictionary = merged[2];
            merged.RemoveAt(2);
            merged.Insert(2, dictionary);

            // If the above doesn't work then fall back to this
            // Application.Current.Resources.MergedDictionaries[2] = new ResourceDictionary() { Source = new Uri("Themes/Controls.xaml", UriKind.Relative) };
        }

        public static object GetResource(object key)
        {
            return ThemeDictionary[key];
        }

        public static SolidColorBrush GetBrush(string name)
        {
            return GetResource(name) is SolidColorBrush brush ? brush : new SolidColorBrush(Colors.White);
        }

        public static BitmapImage? GetApplicationIcon()
        {
            return Application.Current.Resources["ApplicationIcon"] as BitmapImage;
        }
    }
}
