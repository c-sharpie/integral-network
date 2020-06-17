using System.Threading;
using System.Threading.Tasks;
using Integral.Abstractions;
using Integral.Networks;

namespace Integral.Executables
{
    public sealed class NetworkExecutable : Executable<Task>
    {
        private readonly Network network;

        private readonly Executable<Task> executable;

        public NetworkExecutable(Network network, Executable<Task> executable)
        {
            this.network = network;
            this.executable = executable;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            await network.Initialize(cancellationToken);
            while (!cancellationToken.IsCancellationRequested)
            {
                await network.Execute(cancellationToken);
                await executable.Execute(cancellationToken);
                await Task.Delay(1);
            }
        }
    }
}
