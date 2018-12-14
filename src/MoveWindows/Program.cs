using MoveWindows.NativeCalls;
using MoveWindows.Positions;
using System;
using System.Diagnostics;
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
            KillOtherInstances();

            var generator = new BottomPositionSetGenerator();
            var positions = generator.GetPositions(Screen.AllScreens);
            var positionIndex = 0;
            var handle = User32.FindWindow("Chrome_WidgetWin_2", string.Empty);
            while (true)
            {
                if (handle != IntPtr.Zero)
                {
                    User32.GetWindowRect(handle, out var rectangle);
                    var width = rectangle.Right - rectangle.Left;
                    var height = rectangle.Bottom - rectangle.Top;

                    var cursorPosition = Cursor.Position;
                    var isCursorOnWindow = (IsCursorOnWindow(rectangle, cursorPosition));
                    Console.WriteLine($"{cursorPosition} {isCursorOnWindow}");

                    if (isCursorOnWindow)
                    {
                        Console.WriteLine($"Out of the way bitch ! ({positionIndex}:{positions[positionIndex].Screen.DeviceName} / {positions[positionIndex].Position})");
                        var newPosition = GetPosition(positions[positionIndex], width, height, 20);
                        User32.MoveWindow(handle, newPosition.X, newPosition.Y, width, height, true);
                        positionIndex = (positionIndex + 1) % positions.Count;
                    }

                }
                Thread.Sleep(250);
            }
        }

        private static void KillOtherInstances()
        {
            var processes = Process.GetProcessesByName("MoveWindows")
                .Where(p => Process.GetCurrentProcess().Id != p.Id)
                .ToList();
            foreach (var p in processes)
            {
                p.Kill();
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

        private static bool IsCursorOnWindow(NativeCalls.Rectangle rectangle, Point cursorPosition)
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
