using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using AutoMovePipWindow.Configuration;
using AutoMovePipWindow.Contracts;
using AutoMovePipWindow.Helpers;
using Rectangle = System.Drawing.Rectangle;

namespace AutoMovePipWindow.Services
{
    internal class ServiceDaemon : IServiceDaemon
    {
        private readonly ISingleInstanceChecker _singleInstanceChecker;
        private readonly ILogger<ServiceDaemon> _logger;
        private readonly PipConfiguration _configuration;
        private readonly ScreenConfigurationLocator _screenConfigurationLocator;

        public ServiceDaemon(ISingleInstanceChecker singleInstanceChecker,
            ILogger<ServiceDaemon> logger,
            PipConfiguration configuration,
            ScreenConfigurationLocator screenConfigurationLocator
        )
        {
            _singleInstanceChecker = singleInstanceChecker;
            _logger = logger;
            _configuration = configuration;
            _screenConfigurationLocator = screenConfigurationLocator;
        }

        public void Start()
        {
            _singleInstanceChecker.KillOtherInstances();
            MainProcess();
        }

        private void MainProcess()
        {
            _logger.LogInformation("Main process starting");

            while (true)
            {
                MoveWindow();

                Thread.Sleep(_configuration.Interval);
            }

            // ReSharper disable once FunctionNeverReturns
        }

        private void MoveWindow()
        {
            var cursorPosition = Cursor.Position;

            var targets = _screenConfigurationLocator.GetTargets();

            var target = targets.FirstOrDefault(t => t.Rectangle.IsPointInside(cursorPosition));
            if (target == null)
            {
                return;
            }

            var browser = _configuration.Browsers[_configuration.TargetBrowser];
            var handle = User32Helper.FindWindow(browser.WindowClassName, browser.WindowTitle);
            if (handle == IntPtr.Zero)
            {
                _logger.LogWarning("Failed to find window handle");
                return;
            }

            if (browser.HideWindow)
            {
                User32Helper.HideWindow(handle);
            }

            var popupPosition = User32Helper.GetWindowPosition(handle);
            var userHasMouseOverPopup = popupPosition.IsPointInside(cursorPosition);
            if (userHasMouseOverPopup)
            {
                return;
            }

            var targetDimensions = new Size(_configuration.Size.Width, _configuration.Size.Height);

            var size = GeometryHelper.IsApproximatelyTheSame(targetDimensions, popupPosition)
                ? popupPosition.Size
                : targetDimensions;

            var newPosition = GetTargetPosition(size, target.Position, target.Screen, _configuration.Size.Margin);
            if (GeometryHelper.IsDifferent(newPosition, popupPosition))
            {
                User32Helper.MoveWindow(handle, newPosition);
            }
        }

        private static Rectangle GetTargetPosition(Size targetSize, ScreenPosition position, Screen targetScreen, int margin)
        {
            var newPosition = GeometryHelper.GetPosition(position, targetScreen.WorkingArea, targetSize, margin);
            return new Rectangle(newPosition, targetSize);
        }
    }
}