using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Interop;

using Point = System.Windows.Point;

namespace ToDoAppClient.Utilities
{
    public static class PositionsUtilities
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public int X;
            public int Y;
        };

        public static Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);

            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        public static int ScreenSide(Window window, double xPos)
        {
            Screen screen = Screen.FromHandle(new WindowInteropHelper(window).Handle);
            float screenWidth = screen.WorkingArea.Width;
            return xPos < screenWidth / 2 ? 0 : 1;
        }
    }
}
