using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMovePipWindow.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AutoMovePipWindow.Contracts;
using AutoMovePipWindow.Services;
using Microsoft.Extensions.Configuration;

namespace AutoMovePipWindow
{
    internal static class Program
    {
        [STAThread]
        public static async Task Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var t = Worker(cancellationToken);

            Application.Run(new TrayIconContext(cancellationTokenSource));

            await t;
        }

        private static async Task Worker(CancellationToken cancellationToken)
        {
            var serviceProvider = InitApp();
            var daemon = serviceProvider.GetRequiredService<IServiceDaemon>();
            await daemon.StartAsync(cancellationToken);
        }

        private static IServiceProvider InitApp()
        {
            var config = GetConfiguration<PipConfiguration>();
            var serviceProvider = new ServiceCollection();
            serviceProvider.AddSingleton(config);
            serviceProvider.AddTransient<ISingleInstanceChecker, SingleInstanceChecker>();
            serviceProvider.AddTransient<ScreenConfigurationLocator, ScreenConfigurationLocator>();
            serviceProvider.AddTransient<IServiceDaemon, ServiceDaemon>();
            serviceProvider.AddLogging(builder => builder.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace));
            serviceProvider.AddSingleton(config);

            return serviceProvider.BuildServiceProvider();
        }

        private static T GetConfiguration<T>()
        {
            var content = File.ReadAllText("appsettings.yml");
            var deserializer = new YamlDotNet.Serialization.Deserializer();
            return deserializer.Deserialize<T>(content);
        }
    }
}
