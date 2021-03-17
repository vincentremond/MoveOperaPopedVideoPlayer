using System.Drawing;
using System.Windows.Forms;
using AutoMovePipWindow.Configuration;

namespace AutoMovePipWindow.Services
{
    internal record PositionTargetAssociation(Rectangle Rectangle, Screen Screen, ScreenPosition Position, SizeConfiguration Size);
}
