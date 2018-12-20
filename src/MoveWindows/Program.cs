using MoveWindows.NativeCalls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace MoveWindows
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            new SingleInstanceHelper().KillOtherInstances();
            var screens = Screen.AllScreens;
            var targetPositionForScreens = new Dictionary<Screen, ScreenPositionInformation>();
            targetPositionForScreens[screens[0]] = new ScreenPositionInformation { Screen = screens[1], Position = ScreenPosition.BottomRight };
            targetPositionForScreens[screens[1]] = new ScreenPositionInformation { Screen = screens[0], Position = ScreenPosition.BottomLeft };
            while (true)
            {
                var handle = User32.FindWindowEx(parentHandle: IntPtr.Zero, childAfter: IntPtr.Zero, className: "Chrome_WidgetWin_2", windowTitle: string.Empty);
                if (handle != IntPtr.Zero)
                {
                    var cursorPosition = Cursor.Position;
                    User32.GetWindowRect(handle, out var systemRectangle);
                    var popupPosition = systemRectangle.AsDrawingRectangle();
                    var userHasMouseOverPopup = IsPointInRectangle(cursorPosition, popupPosition);
                    if (!userHasMouseOverPopup)
                    {
                        var screenWhereCursorIs = screens.Single(s => IsPointInRectangle(cursorPosition, s.Bounds));
                        var position = targetPositionForScreens[screenWhereCursorIs];
                        var newPosition = GetPosition(position, popupPosition.Width, popupPosition.Height, 20);
                        User32.MoveWindow(handle, newPosition.X, newPosition.Y, popupPosition.Width, popupPosition.Height, true);
                    }
                }
                Thread.Sleep(1000);
            }
        }

        private static Point GetPosition(ScreenPositionInformation infos, int width, int height, int margin)
        {
            var y = infos.Position.HasFlag(ScreenPosition.Top) ? infos.Screen.WorkingArea.Y + margin
                : infos.Position.HasFlag(ScreenPosition.Bottom) ? infos.Screen.WorkingArea.Y + infos.Screen.WorkingArea.Height - margin - height
                : throw new InvalidOperationException("Position has no verticality");
            var x = infos.Position.HasFlag(ScreenPosition.Left) ? infos.Screen.WorkingArea.X + margin
                : infos.Position.HasFlag(ScreenPosition.Right) ? infos.Screen.WorkingArea.X + infos.Screen.WorkingArea.Width - margin - width
                : throw new InvalidOperationException("Position has no horizontality");
            return new Point(x, y);
        }

        private static bool IsPointInRectangle(Point cursorPosition, System.Drawing.Rectangle rectangle)
        {
            return IsBetween(cursorPosition.X, rectangle.Left, rectangle.Right)
                && IsBetween(cursorPosition.Y, rectangle.Top, rectangle.Bottom);
        }

        private static bool IsBetween(int value, int min, int max)
        {
            if (!(max > min))
            {
                throw new InvalidOperationException($"Min ({min}) should be smaller than Man ({max})");
            }

            return min <= value && value <= max;
        }
    }
}
