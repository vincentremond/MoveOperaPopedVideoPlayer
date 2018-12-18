using System.Diagnostics;
using System.Linq;

namespace MoveWindows
{
    public class SingleInstanceHelper
    {
        public void KillOtherInstances()
        {
            var currentProcess = Process.GetCurrentProcess();
            var allProcessesWithSameName = Process.GetProcessesByName(currentProcess.ProcessName);
            var allOtherProcesses = allProcessesWithSameName.Where(p => p.Id != currentProcess.Id);
            foreach (var p in allOtherProcesses)
            {
                p.Kill();
            }
        }
    }
}