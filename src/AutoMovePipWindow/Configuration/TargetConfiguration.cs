using System;

namespace AutoMovePipWindow.Configuration
{
    [Serializable]
    internal class TargetConfiguration
    {
        public int Screen { get; set; } = 0;
        public ScreenPosition Position { get; set; } = ScreenPosition.Undefined;
    }
}