using System.Drawing;

namespace MoveWindows.Configuration
{
    internal static class GlobalConfiguration
    {
        public static bool HideWindow = true;
#if TARGET_OPERA
        public static string WindowClassName = "Chrome_WidgetWin_2";
        public static string WindowTitle = string.Empty;
#else
        public static string WindowClassName = "MozillaDialogClass";
        public static string WindowTitle = "Picture-in-Picture";
#endif
        public static Size TargetDimensions = new Size(400, 240);
        public static int Margin = 8;
    }
}