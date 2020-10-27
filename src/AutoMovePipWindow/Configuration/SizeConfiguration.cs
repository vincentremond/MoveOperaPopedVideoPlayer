using System;
using System.Text.RegularExpressions;

namespace AutoMovePipWindow.Configuration
{
    [Serializable]
    internal class SizeConfiguration
    {
        public int Multiplier { get; }
        public int WidthRatio { get; }
        public int HeightRatio { get; }
        public int Margin { get; }
        public int Width => Multiplier * WidthRatio;
        public int Height => Multiplier * HeightRatio;

        public SizeConfiguration(int multiplier, int widthRatio, int heightRatio, int margin)
        {
            Multiplier = multiplier;
            WidthRatio = widthRatio;
            HeightRatio = heightRatio;
            Margin = margin;
        }

        public static implicit operator SizeConfiguration(string value)
        {
            var pattern =
                "^" +
                $"(?<{nameof(Multiplier)}>\\d+)" +
                "\\*" +
                $"(?<{nameof(WidthRatio)}>\\d+)" +
                "/" +
                $"(?<{nameof(HeightRatio)}>\\d+)" +
                ":" +
                $"(?<{nameof(Margin)}>\\d+)" +
                "$";
            var match = Regex.Match(value, pattern, RegexOptions.None);
            var multiplier = int.Parse(match.Groups[nameof(Multiplier)].Value);
            var widthRatio = int.Parse(match.Groups[nameof(WidthRatio)].Value);
            var heightRatio = int.Parse(match.Groups[nameof(HeightRatio)].Value);
            var margin = int.Parse(match.Groups[nameof(Margin)].Value);
            return new SizeConfiguration(multiplier, widthRatio, heightRatio, margin);
        }
    }
}
