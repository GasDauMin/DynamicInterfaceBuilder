using System.Runtime.InteropServices;

namespace DynamicInterfaceBuilder
{
    class WinAPI
    {
        // P/Invoke declaration for ShowScrollBar
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);

        public enum ScrollBarDirection
        {
            SB_HORZ = 0,
            SB_VERT = 1,
            SB_CTL = 2,
            SB_BOTH = 3
        }
    }
}