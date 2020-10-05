using System.Threading;
using System.Threading.Tasks;

namespace AutoMovePipWindow.Contracts
{
    internal interface IServiceDaemon
    {
        Task StartAsync(CancellationToken cancellationToken);
    }
}
