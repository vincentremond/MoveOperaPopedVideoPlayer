using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using AutoMovePipWindow.Configuration;
using AutoMovePipWindow.Contracts;
using AutoMovePipWindow.Helpers;
using AutoMovePipWindow.NativeCalls;
using Rectangle = System.Drawing.Rectangle;

namespace AutoMovePipWindow.Services
{
    internal class ServiceDaemon : IServiceDaemon
    {
        private readonly ISingleInstanceChecker _singleInstanceChecker;
        private readonly ILogger<ServiceDaemon> _logger;

        public ServiceDaemon(ISingleInstanceChecker singleInstanceChecker, ILogger<ServiceDaemon> logger)
        {
            _singleInstanceChecker = singleInstanceChecker;
            _logger = logger;
        }

        public void Start()
        {
            _singleInstanceChecker.KillOtherInstances();
            MainProcess();
        }

        private void MainProcess()
        {
            _logger.LogInformation("Main process starting");

            var targetPositionForScreens = GetConfiguration();
            while (true)
            {
                MoveWindow(targetPositionForScreens);

                Thread.Sleep(1000);
            }

            // ReSharper disable once FunctionNeverReturns
        }

        private void MoveWindow(Dictionary<Screen, ScreenPositionInformation> targetPositionForScreens)
        {
            var cursorPosition = Cursor.Position;

            var screenWhereCursorIs = Screen.AllScreens.FirstOrDefault(s => GeometryHelper.IsPointInRectangle(cursorPosition, s.Bounds));
            if (screenWhereCursorIs == null)
            {
                return;
            }

            var className = GlobalConfiguration.WindowClassName;
            var windowTitle = GlobalConfiguration.WindowTitle;
            var handle = User32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, className, windowTitle);
            if (handle == IntPtr.Zero)
            {
                _logger.LogWarning("Failed to find window handle");
                return;
            }

            if (GlobalConfiguration.HideWindow)
            {
                HideWindow(handle);
            }

            var popupPosition = GetPipWindowPosition(handle);
            var userHasMouseOverPopup = GeometryHelper.IsPointInRectangle(cursorPosition, popupPosition);
            if (userHasMouseOverPopup)
            {
                return;
            }

            var size = IsApproximatelyTheSame(GlobalConfiguration.TargetDimensions, popupPosition)
                ? popupPosition.Size
                : GlobalConfiguration.TargetDimensions;

            var newPosition = GetTargetPosition(targetPositionForScreens, screenWhereCursorIs, size);
            if (IsDifferent(newPosition, popupPosition))
            {
                User32.MoveWindow(handle,
                    newPosition.X,
                    newPosition.Y,
                    size.Width,
                    size.Height,
                    true);
            }
        }

        private static void HideWindow(IntPtr handle)
        {
            var windowLong = User32.GetWindowLong(handle, User32.GwlExStyle);
            var targetWindowLong = (windowLong | User32.WsExToolwindow) & ~User32.WsExAppwindow;
            if (windowLong != targetWindowLong)
            {
                User32.SetWindowLong(handle, User32.GwlExStyle, targetWindowLong);
            }
        }

        private bool IsApproximatelyTheSame(Size newSize, Rectangle popupPosition)
        {
            return newSize.Width == popupPosition.Width || newSize.Height == popupPosition.Height;
        }

        private static bool IsDifferent(Point newPosition, Rectangle popupPosition)
        {
            return newPosition.X != popupPosition.X || newPosition.Y != popupPosition.Y;
        }

        private static Point GetTargetPosition(Dictionary<Screen, ScreenPositionInformation> targetPositionForScreens, Screen screenWhereCursorIs, Size targetSize)
        {
            var targetPosition = targetPositionForScreens[screenWhereCursorIs];
            var newPosition = GeometryHelper.GetPosition(targetPosition, targetSize.Width, targetSize.Height, GlobalConfiguration.Margin);
            return newPosition;
        }

        private static Rectangle GetPipWindowPosition(IntPtr handle)
        {
            User32.GetWindowRect(handle, out var systemRectangle);
            var popupPosition = systemRectangle.AsDrawingRectangle();
            return popupPosition;
        }

        private static Dictionary<Screen, ScreenPositionInformation> GetConfiguration()
        {
            var screens = Screen.AllScreens;
            var targetPositionForScreens = new Dictionary<Screen, ScreenPositionInformation>();
            targetPositionForScreens[screens[0]] = new ScreenPositionInformation {Screen = screens[1], Position = ScreenPosition.BottomLeft};
            targetPositionForScreens[screens[1]] = new ScreenPositionInformation {Screen = screens[0], Position = ScreenPosition.BottomRight};
            return targetPositionForScreens;
        }
    }
}