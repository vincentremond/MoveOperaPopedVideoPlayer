using System;

namespace AutoMovePipWindow.Configuration
{
    [Serializable]
    internal class ScreenConfiguration
    {
        public ConditionsConfiguration[] Conditions { get; set; }
        public PositionConfiguration[] Positions { get; set; }
    }
}
