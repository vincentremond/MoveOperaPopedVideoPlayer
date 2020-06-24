using System;
using AutoMovePipWindow.NativeCalls;
using Rectangle = System.Drawing.Rectangle;

namespace AutoMovePipWindow.Helpers
{
    internal static class User32Helper
    {
        public static Rectangle GetWindowPosition(IntPtr handle)
        {
            User32.GetWindowRect(handle, out var systemRectangle);
            return systemRectangle.AsDrawingRectangle();
        }

        public static void HideWindow(IntPtr handle)
        {
            var windowLong = User32.GetWindowLong(handle, User32.GwlExStyle);
            var targetWindowLong = (windowLong | User32.WsExToolwindow) & ~User32.WsExAppwindow;
            if (windowLong != targetWindowLong)
            {
                User32.SetWindowLong(handle, User32.GwlExStyle, targetWindowLong);
            }
        }

        public static IntPtr FindWindow(string className, string windowTitle)
        {
            return User32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, className, windowTitle);
        }

        public static void MoveWindow(IntPtr handle, Rectangle newPosition)
        {
            User32.MoveWindow(handle,
                newPosition.X,
                newPosition.Y,
                newPosition.Width,
                newPosition.Height,
                true);
        }
    }
}