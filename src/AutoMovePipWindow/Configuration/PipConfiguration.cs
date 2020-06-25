using System;
using System.Collections.Generic;

namespace AutoMovePipWindow.Configuration
{
    [Serializable]
    internal class PipConfiguration
    {
        public SizeConfiguration Size { get; set; }
        public string TargetBrowser { get; set; }
        public int Interval { get; set; }
        public bool AllowOverlap { get; set; }
        public Dictionary<string, BrowserConfiguration> Browsers { get; set; }
        public Dictionary<string, ScreenConfiguration> Screens { get; set; }
    }
}