using System;

namespace AutoMovePipWindow.Configuration
{
    [Serializable]
    internal class ConditionsConfiguration
    {
        public bool? Primary { get; set; } = null;
        public string Expression { get; set; } = null;
    }
}