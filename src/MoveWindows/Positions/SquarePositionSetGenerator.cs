using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MoveWindows.Positions
{
    public class SquarePositionSetGenerator : IPositionSetGenerator
    {
        private static List<ScreenPositionInformation> GetPositionSetBottom(Screen[] screens)
        {
            var positions = new List<ScreenPositionInformation>();
            var screen = screens.Last();
            foreach (var position in new[] { ScreenPosition.TopLeft, ScreenPosition.TopRight, ScreenPosition.BottomRight, ScreenPosition.BottomLeft })
            {
                positions.Add(new ScreenPositionInformation
                {
                    Screen = screen,
                    Position = position,
                });
            }
            return positions;
        }
    }
}