using System.Runtime.InteropServices;

namespace MoveWindows.NativeCalls
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner

        internal System.Drawing.Rectangle AsDrawingRectangle()
        {
            return new System.Drawing.Rectangle(Left, Top, GetWidth(), GetHeight());
        }

        private int GetHeight()
        {
            return Bottom - Top;
        }

        private int GetWidth()
        {
            return Right - Left;
        }
    }
}
