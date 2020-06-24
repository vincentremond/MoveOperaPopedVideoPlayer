using System;
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
        public static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var serviceProvider = InitApp();
            var daemon = serviceProvider.GetRequiredService<IServiceDaemon>();
            daemon.Start();
        }

        private static IServiceProvider InitApp()
        {
            var config = GetConfiguration();
            var serviceProvider = new ServiceCollection();
            serviceProvider.AddTransient<ISingleInstanceChecker, SingleInstanceChecker>();
            serviceProvider.AddTransient<ScreenConfigurationLocator, ScreenConfigurationLocator>();
            serviceProvider.AddTransient<IServiceDaemon, ServiceDaemon>();
            serviceProvider.AddLogging(builder => builder.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace));
            serviceProvider.AddSingleton(config);

            return serviceProvider.BuildServiceProvider();
        }

        private static PipConfiguration GetConfiguration()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddYamlFile("appsettings.yml", optional: false, reloadOnChange: false)
                .Build();

            return config.Get<PipConfiguration>();
        }
    }
}