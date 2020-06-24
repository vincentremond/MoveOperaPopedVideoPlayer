using System;

namespace AutoMovePipWindow.Configuration
{
    [Serializable]
    internal class CursorConfiguration
    {
        public int Screen { get; set; } = 0;
        public decimal X { get; set; } = 0m;
        public decimal Y { get; set; } = 0m;
        public decimal Width { get; set; } = 1m;
        public decimal Height { get; set; } = 1m;
    }
}