using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MoveWindows.Positions;

namespace MoveWindows.Positions
{
    public class BottomPositionSetGenerator : IPositionSetGenerator
    {
        internal List<ScreenPositionInformation> GetPositions(Screen[] screens)
        {
            var positions = new List<ScreenPositionInformation>();
            foreach (var screen in screens)
            {
                foreach (var position in new[] { ScreenPosition.BottomLeft, ScreenPosition.BottomRight })
                {
                    positions.Add(new ScreenPositionInformation
                    {
                        Screen = screen,
                        Position = position,
                    });
                }
            }
            return positions;
        }
    }
}