using System;
using System.Threading;
using System.Threading.Tasks;
using Integral.Abstractions;
using Integral.Channels;
using Integral.Packets;
using Integral.Publishers;

namespace Integral.Registries
{
    internal sealed class ClientRegistry : GenericPublisher<Action<EmptyPacket>>, Registry<Channel>, Executable<Task>
    {
        private int iterations, current = 0;

        internal ClientRegistry(int iterations) => this.iterations = iterations;

        public async Task Execute(CancellationToken cancellationToken)
        {
            if (IsSubscribed && current < iterations)
            {
                Publish(Populate);
                current++;
            }

            await Task.Delay(1);
        }

        public void Register(Channel channel) => channel.Register(this);

        public void Unregister(Channel channel) => channel.Unregister(this);

        private void Populate(EmptyPacket emptyPacket)
        {
        }
    }
}
