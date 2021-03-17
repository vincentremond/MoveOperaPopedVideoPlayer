using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMovePipWindow.Configuration;
using AutoMovePipWindow.Contracts;
using AutoMovePipWindow.Helpers;
using Microsoft.Extensions.Logging;
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

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _singleInstanceChecker.KillOtherInstances();
            await MainProcessAsync(cancellationToken);
        }

        private async Task MainProcessAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Main process starting");

            while (!cancellationToken.IsCancellationRequested)
            {
                MoveWindow();
                await Task.Delay(_configuration.Interval);
            }
        }

        private void MoveWindow()
        {
            var cursorPosition = Cursor.Position;
            var allScreens = Screen.AllScreens;

            var targets = _screenConfigurationLocator.GetTargets(allScreens);

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

            if (_configuration.AllowOverlap)
            {
                var userHasMouseOverPopup = popupPosition.IsPointInside(cursorPosition);
                if (userHasMouseOverPopup)
                {
                    return;
                }
            }

            var targetDimensions = new Size(target.Size.Width, target.Size.Height);

            var size = GeometryHelper.IsApproximatelyTheSame(targetDimensions, popupPosition)
                ? popupPosition.Size
                : targetDimensions;

            var newPosition = GetTargetPosition(size, target.Position, target.Screen, target.Size.Margin);
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
