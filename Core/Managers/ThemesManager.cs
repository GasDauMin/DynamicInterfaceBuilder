using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
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

            resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("Core/UI/Themes/Default.xaml", UriKind.Relative)
            });
            resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("Core/UI/Structures/ControlColours.xaml", UriKind.Relative)
            });
            resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("Core/UI/Structures/Controls.xaml", UriKind.Relative)
            });

            System.Windows.Application.Current.Resources = resources;
        }

        //public static ThemeType CurrentTheme { get; set; }

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
    }
}
