using System.Windows.Forms;
using AutoMovePipWindow.Configuration;

namespace AutoMovePipWindow.DTO
{
    internal record ScreenPositions(Screen[] Screens, PositionConfiguration[] Positions);
}
