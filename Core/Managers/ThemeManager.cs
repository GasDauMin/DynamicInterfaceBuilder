using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DynamicInterfaceBuilder.Core.Models;
using System.IO;
using System.Windows.Markup;
using System.Collections;
using DynamicInterfaceBuilder.Styles.Enums;

namespace DynamicInterfaceBuilder.Core.Managers
{
    public class ThemeManager : AppBase
    {
        private static readonly List<ResourceDictionary> _customDictionaries = new();

        public ThemeManager(App application) : base(application)
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
                    Source = new Uri("pack://application:,,,/DynamicInterfaceBuilder;component/Styles/Themes/Default.xaml", UriKind.Absolute)
                });
                resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/DynamicInterfaceBuilder;component/Styles/Structures/ControlColours.xaml", UriKind.Absolute)
                });
                resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/DynamicInterfaceBuilder;component/Styles/Structures/ControlBase.xaml", UriKind.Absolute)
                });
                resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/DynamicInterfaceBuilder;component/Styles/Structures/ControlStyles.xaml", UriKind.Absolute)
                });

                // Add any existing custom dictionaries
                foreach (var dict in _customDictionaries)
                {
                    resources.MergedDictionaries.Add(dict);
                }

                // Try to load custom style file from working directory
                LoadWorkingDirectoryStyleFile(resources, (DynamicInterfaceBuilder.App)App);

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

        private static ResourceDictionary ControlBase
        {
            get => Application.Current.Resources.MergedDictionaries[2];
            set => Application.Current.Resources.MergedDictionaries[2] = value;
        }        

        private static ResourceDictionary ControlStyles
        {
            get => Application.Current.Resources.MergedDictionaries[3];
            set => Application.Current.Resources.MergedDictionaries[3] = value;
        }

        public static object GetResource(object key)
        {
            return ThemeDictionary[key];
        }

        public static void SetTheme(ThemeType theme)
        {
            string themeName = theme.GetName();
            if (string.IsNullOrEmpty(themeName))
            {
                return;
            }

            ThemeDictionary = new ResourceDictionary() { Source = new Uri($"pack://application:,,,/DynamicInterfaceBuilder;component/Styles/Themes/{themeName}.xaml", UriKind.Absolute) };
            ControlColours = new ResourceDictionary() { Source = new Uri("pack://application:,,,/DynamicInterfaceBuilder;component/Styles/Structures/ControlColours.xaml", UriKind.Absolute) };

            RefreshControls();
        }

        private static void RefreshControls()
        {
            Collection<ResourceDictionary> merged = Application.Current.Resources.MergedDictionaries;
            ResourceDictionary dictionary = merged[2];

            merged.RemoveAt(2);
            merged.Insert(2, dictionary);

            ClearCustomDictionaries();
            AddCustomDictionaries();
        }

        public static void AddCustomDictionaries()
        {
            foreach (var dict in _customDictionaries)
            {
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
        }

        public static void ClearCustomDictionaries()
        {
            foreach (var dict in _customDictionaries.ToList())
            {
                Application.Current.Resources.MergedDictionaries.Remove(dict);
            }
            _customDictionaries.Clear();
        }

        public static void AddCustomDictionary(ResourceDictionary dictionary)
        {
            ArgumentNullException.ThrowIfNull(dictionary);

            _customDictionaries.Add(dictionary);
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

        public static void RemoveCustomDictionary(ResourceDictionary dictionary)
        {
            ArgumentNullException.ThrowIfNull(dictionary);

            if (_customDictionaries.Remove(dictionary))
            {
                Application.Current.Resources.MergedDictionaries.Remove(dictionary);
            }
        }

        public static void LoadCustomDictionary(string xamlPath)
        {
            try
            {
                var dictionary = new ResourceDictionary
                {
                    Source = new Uri(xamlPath, UriKind.RelativeOrAbsolute)
                };
                AddCustomDictionary(dictionary);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load custom theme dictionary: {ex.Message}");
                throw;
            }
        }

        public static SolidColorBrush? GetBrush(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
                
            if (GetResource(name) is SolidColorBrush brush)
            {
                return brush;
            }
            else if (GetResource(name) is Color color)
            {
                return new SolidColorBrush(color);
            }

            return null;
        }

        public static Color? GetColor(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            if (GetResource(name) is Color color)
            {
                return color;
            }
            else if (GetResource(name) is SolidColorBrush brush)
            {
                return brush.Color;
            }

            return null;
        }

        public static BitmapImage? GetApplicationIcon()
        {
            return Application.Current.Resources["ApplicationIcon"] as BitmapImage;
        }

        /// <summary>
        /// Loads a custom style file from the working directory or from the StylePath specified in startup settings
        /// to allow project-specific style overrides.
        /// </summary>
        /// <param name="resources">The resource dictionary to add the custom styles to</param>
        /// <param name="app">The application instance to get StylePath from startup settings</param>
        private static void LoadWorkingDirectoryStyleFile(ResourceDictionary resources, DynamicInterfaceBuilder.App app)
        {
            try
            {
                string workingDirectory = Directory.GetCurrentDirectory();
                
                // Use StylePath from startup settings, or fall back to default
                string styleFileName = !string.IsNullOrEmpty(app.StartupProperties.StylePath) 
                    ? app.StartupProperties.StylePath 
                    : "FormBuilder.style.xaml";
                
                string customStylePath = Path.IsPathRooted(styleFileName)
                    ? styleFileName  // Use absolute path if provided
                    : Path.Combine(workingDirectory, styleFileName);  // Combine with working directory if relative
                
                if (File.Exists(customStylePath))
                {
                    System.Diagnostics.Debug.WriteLine($"Loading custom style file: {customStylePath}");
                    
                    // Load the XAML file as a ResourceDictionary
                    using (var fileStream = new FileStream(customStylePath, FileMode.Open, FileAccess.Read))
                    {
                        var customStyle = (ResourceDictionary)XamlReader.Load(fileStream);
                        if (customStyle != null)
                        {
                            resources.MergedDictionaries.Add(customStyle);
                            System.Diagnostics.Debug.WriteLine($"Successfully loaded custom style file with {customStyle.Count} resources");
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"No custom style file found at: {customStylePath}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load working directory style file: {ex.Message}");
                // Don't throw - custom styles are optional
            }
        }
    }
}
