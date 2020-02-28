using System;
using System.Drawing;
using MoveWindows.Configuration;

namespace MoveWindows.Helpers
{
    internal static class GeometryHelper
    {
        public static Point GetPosition(ScreenPositionInformation infos, int width, int height, int margin)
        {
            var y = infos.Position.HasFlag(ScreenPosition.Top) ? infos.Screen.WorkingArea.Y + margin
                : infos.Position.HasFlag(ScreenPosition.Bottom) ? infos.Screen.WorkingArea.Y + infos.Screen.WorkingArea.Height - margin - height
                : throw new InvalidOperationException("Position has no verticality");
            var x = infos.Position.HasFlag(ScreenPosition.Left) ? infos.Screen.WorkingArea.X + margin
                : infos.Position.HasFlag(ScreenPosition.Right) ? infos.Screen.WorkingArea.X + infos.Screen.WorkingArea.Width - margin - width
                : throw new InvalidOperationException("Position has no horizontality");
            return new Point(x, y);
        }

        public static bool IsPointInRectangle(Point cursorPosition, Rectangle rectangle)
        {
            return IsBetween(cursorPosition.X, rectangle.Left, rectangle.Right)
                   && IsBetween(cursorPosition.Y, rectangle.Top, rectangle.Bottom);
        }

        private static bool IsBetween(int value, int min, int max)
        {
            if (!(max > min)) throw new InvalidOperationException($"Min ({min}) should be smaller than Man ({max})");

            return min <= value && value <= max;
        }
    }
}