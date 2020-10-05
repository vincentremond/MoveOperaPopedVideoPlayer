using System;
using System.Drawing;
using AutoMovePipWindow.Configuration;

namespace AutoMovePipWindow.Helpers
{
    internal static class GeometryHelper
    {
        public static Point GetPosition(ScreenPosition position, Rectangle workingArea, Size targetSize, int margin)
        {
            var y
                = position.HasFlag(ScreenPosition.Top) ? workingArea.Y + margin
                : position.HasFlag(ScreenPosition.Bottom) ? (workingArea.Y + workingArea.Height) - margin - targetSize.Height
                : throw new InvalidOperationException("Position has no verticality");
            var x
                = position.HasFlag(ScreenPosition.Left) ? workingArea.X + margin
                : position.HasFlag(ScreenPosition.Right) ? (workingArea.X + workingArea.Width) - margin - targetSize.Width
                : throw new InvalidOperationException("Position has no horizontality");
            return new Point(x, y);
        }

        public static bool IsPointInside(this Rectangle rectangle, Point cursorPosition)
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

        public static bool IsDifferent(Rectangle newPosition, Rectangle popupPosition)
        {
            return newPosition.X != popupPosition.X || newPosition.Y != popupPosition.Y;
        }

        public static bool IsApproximatelyTheSame(Size newSize, Rectangle popupPosition)
        {
            return newSize.Width == popupPosition.Width || newSize.Height == popupPosition.Height;
        }
    }
}
