using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using System.Diagnostics;

namespace TheToolkit
{
    public class DynamicInterfaceBuilder
    {
        public string Title { get; set; } = "Dynamic Interface";
        public int Width { get; set; } = 600;
        public int Height { get; set; } = 400;
        public bool DarkMode { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> Results { get; set; } = new Dictionary<string, object>();
        protected Dictionary<string, string> ThemeProperties { get; set; } = new Dictionary<string, string>();
        
        private bool IsDarkMode()
        {
            Debug.WriteLine("Checking system dark mode setting...");
            bool ret = true;

            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    Debug.WriteLine($"Registry key opened: {key != null}");
                    if (key != null)
                    {
                        object? value = key.GetValue("AppsUseLightTheme");
                        Debug.WriteLine($"Theme value: {value}");
                        if (value != null && value is int intValue && intValue == 1)
                        {
                            ret = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking dark mode: {ex.Message}");
            }

            Debug.WriteLine($"Dark mode result: {ret}");
            return ret;
        }
        
        public DynamicInterfaceBuilder(string title, int width, int height)
        {
            Debug.WriteLine($"Creating new interface: {title} ({width}x{height})");
            Title = title;
            Width = width;
            Height = height;
            DarkMode = IsDarkMode();
            Debug.WriteLine("Interface builder initialized");

            // Load default theme based on dark mode setting
            // LoadTheme(DarkMode ? "DarkTheme.xml" : "LightTheme.xml");
        }

    }   
}