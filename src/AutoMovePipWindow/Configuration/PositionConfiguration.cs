using System;

namespace AutoMovePipWindow.Configuration
{
    [Serializable]
    internal class PositionConfiguration
    {
        public CursorConfiguration Cursor { get; set; }
        public TargetConfiguration Target { get; set; }
    }
}