using System;

namespace AutoMovePipWindow.Configuration
{
    [Serializable]
    internal class SizeConfiguration
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Margin { get; set; }
    }
}