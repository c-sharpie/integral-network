using System.Threading;
using System.Threading.Tasks;
using Integral.Collections;
using Integral.Hosts;

namespace Integral.Networks
{
    internal sealed class HostNetwork : ListedCollection<Host>, Network
    {
        public async Task Initialize(CancellationToken cancellationToken)
        {
            foreach (Host host in this)
            {
                await host.Initialize(cancellationToken);
            }
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            foreach (Host host in this)
            {
                await host.Execute(cancellationToken);
            }
        }
    }
}