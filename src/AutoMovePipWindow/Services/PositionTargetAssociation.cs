using System.Drawing;
using System.Windows.Forms;
using AutoMovePipWindow.Configuration;

namespace AutoMovePipWindow.Services
{
    internal class PositionTargetAssociation
    {
        public Rectangle Rectangle { get; }
        public Screen Screen { get; }
        public ScreenPosition Position { get; }

        public PositionTargetAssociation(Rectangle rectangle, Screen screen, ScreenPosition position)
        {
            Rectangle = rectangle;
            Screen = screen;
            Position = position;
        }
    }
}