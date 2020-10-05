using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace AutoMovePipWindow
{
    internal class TrayIconContext : ApplicationContext
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly NotifyIcon _notifyIcon;

        public TrayIconContext(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _notifyIcon = CreateNotifyIcon();
        }

        private NotifyIcon CreateNotifyIcon()
        {
            var notifyIcon = new NotifyIcon
            {
                Icon = new Icon("icon.ico"),
                Text = "AutoMovePipWindow",
                Visible = true,
            };
            notifyIcon.DoubleClick += Exit;
            return notifyIcon;
        }

        private void Exit(object _, EventArgs __)
        {
            _notifyIcon.Visible = false;
            _cancellationTokenSource.Cancel();
            Application.Exit();
        }
    }
}
