using System.Windows.Forms;
using AutoMovePipWindow.Configuration;

namespace AutoMovePipWindow.DTO
{
    internal class ScreenPositions
    {
        public ScreenPositions(Screen[] screens, PositionConfiguration[] positions)
        {
            Screens = screens;
            Positions = positions;
        }

        public Screen[] Screens { get; }
        public PositionConfiguration[] Positions { get; }
    }
}