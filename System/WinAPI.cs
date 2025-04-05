using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Interop;

namespace DynamicInterfaceBuilder
{
    public static class WinAPI
    {
        /// <summary>
        /// Opens a folder browser dialog using pure WPF techniques
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="initialDirectory">Initial directory to start from</param>
        /// <returns>Selected folder path or null if canceled</returns>
        public static string? BrowseForFolder(string title = "Select Folder", string? initialDirectory = null)
        {
            // WPF doesn't have a built-in folder browser, so we use OpenFileDialog with tricks
            var dialog = new OpenFileDialog
            {
                CheckFileExists = false,
                FileName = "Folder Selection",
                Title = title,
                ValidateNames = false,
                CheckPathExists = true
            };

            if (!string.IsNullOrEmpty(initialDirectory) && Directory.Exists(initialDirectory))
            {
                dialog.InitialDirectory = initialDirectory;
            }

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string? folderPath = Path.GetDirectoryName(dialog.FileName);
                return folderPath;
            }

            return null;
        }
    }
}