using System.Diagnostics;
using System.Linq;
using MoveWindows.Contracts;

namespace MoveWindows.Services
{
    internal class SingleInstanceChecker : ISingleInstanceChecker
    {
        public void KillOtherInstances()
        {
            var currentProcess = Process.GetCurrentProcess();
            if (currentProcess.ProcessName.StartsWith("dotnet"))
            {
                return;
            }

            var allOtherProcesses = Process.GetProcessesByName(currentProcess.ProcessName)
                .Where(p => p.Id != currentProcess.Id);
            foreach (var p in allOtherProcesses) p.Kill();
        }
    }
}