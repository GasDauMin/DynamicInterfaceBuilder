using System.Runtime.InteropServices;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Helpers
{
    public class WindowsHelper : AppBase {
        public WindowsHelper(App application) : base(application)
        {
        }

        [DllImport("uxtheme.dll", EntryPoint = "#135", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int SetPreferredAppMode(int preferredAppMode);

        [DllImport("uxtheme.dll", EntryPoint = "#136", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern void FlushMenuThemes();
    }
}