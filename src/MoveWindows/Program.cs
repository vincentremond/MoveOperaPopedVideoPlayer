using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoveWindows.Contracts;
using MoveWindows.Services;

namespace MoveWindows
{
    internal static class Program
    {
        [STAThread]
        public static void Main()
        {
            var serviceProvider = InitApp();
            var daemon = serviceProvider.GetRequiredService<IServiceDaemon>();
            daemon.Start();
        }

        private static IServiceProvider InitApp()
        {
            var serviceProvider = new ServiceCollection();
            serviceProvider.AddTransient<ISingleInstanceChecker, SingleInstanceChecker>();
            serviceProvider.AddTransient<IServiceDaemon, ServiceDaemon>();
            serviceProvider.AddLogging(builder => builder.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace));
            return serviceProvider.BuildServiceProvider();
        }
    }
}