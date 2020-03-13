using System;

namespace AutoMovePipWindow.Configuration
{
    [Flags]
    internal enum ScreenPosition
    {
        Top = 1,
        Right = 2,
        Bottom = 4,
        Left = 8,

        BottomLeft = Bottom | Left,
        BottomRight = Bottom | Right,
        TopLeft = Top | Left,
        TopRight = Top | Right
    }
}