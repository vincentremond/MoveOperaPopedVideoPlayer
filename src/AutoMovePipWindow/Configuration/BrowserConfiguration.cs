using System;

namespace AutoMovePipWindow.Configuration
{
    [Serializable]
    internal class BrowserConfiguration
    {
        public string WindowClassName { get; set; }
        public string WindowTitle { get; set; }
        public bool HideWindow { get; set; }
    }
}