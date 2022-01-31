using System.Runtime.InteropServices;
using System;

namespace ToDoAppClient.Core.Helpers
{
    public static class HwndResizeHandler
    {
        public static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = false;
                    break;
            }
            return (IntPtr)0;
        }

        public static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            MinMaxInfo mmi = (MinMaxInfo)Marshal.PtrToStructure(lParam, typeof(MinMaxInfo));
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (monitor != IntPtr.Zero)
            {
                MonitorInfo monitorInfo = new MonitorInfo();
                GetMonitorInfo(monitor, monitorInfo);
                Rect rcWorkArea = monitorInfo.rcWork;
                Rect rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }
            Marshal.StructureToPtr(mmi, lParam, true);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int x;
            public int y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MinMaxInfo
        {
            public Point ptReserved;
            public Point ptMaxSize;
            public Point ptMaxPosition;
            public Point ptMinTrackSize;
            public Point ptMaxTrackSize;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MonitorInfo
        {
            public int cbSize = Marshal.SizeOf(typeof(MonitorInfo));
            public Rect rcMonitor = new Rect();
            public Rect rcWork = new Rect();
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public static readonly Rect Empty = new Rect();
            public int Width { get { return Math.Abs(right - left); } }
            public int Height { get { return bottom - top; } }
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MonitorInfo lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
    }
}
