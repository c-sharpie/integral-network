using System.Threading;
using System.Threading.Tasks;
using Integral.Abstractions;
using Integral.Collections;
using Integral.Hosts;

namespace Integral.Networks
{
    internal sealed class HostNetwork : ListedCollection<Host>, Network
    {
        private readonly Executable<Task> executable;

        internal HostNetwork(Executable<Task> executable) => this.executable = executable;

        public async Task Initialize(CancellationToken cancellationToken)
        {
            foreach (Host host in this)
            {
                await host.Initialize(cancellationToken);
            }
        }

        public async Task Iterate(CancellationToken cancellationToken)
        {
            foreach (Host host in this)
            {
                await host.Iterate(cancellationToken);
            }

            await executable.Execute(cancellationToken);
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            await Initialize(cancellationToken);
            while (!cancellationToken.IsCancellationRequested)
            {
                await Iterate(cancellationToken);
            }
        }
    }
}