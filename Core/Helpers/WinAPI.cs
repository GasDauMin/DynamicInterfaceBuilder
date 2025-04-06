using System.Runtime.InteropServices;

namespace DynamicInterfaceBuilder
{
    public class WinAPI
    {
        [DllImport("uxtheme.dll", EntryPoint = "#135", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int SetPreferredAppMode(int preferredAppMode);

        [DllImport("uxtheme.dll", EntryPoint = "#136", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern void FlushMenuThemes();
    }
}