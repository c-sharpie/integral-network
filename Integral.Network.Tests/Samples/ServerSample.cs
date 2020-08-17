using System;
using System.Threading;
using System.Threading.Tasks;
using Integral.Abstractions;
using Integral.Channels;
using Integral.Packets;
using Integral.Subscribers;

namespace Integral.Registries
{
    internal sealed class ServerSample : Subscriber<EmptyPacket>, Registry<Channel>, Executable<Task>
    {
        private readonly int iterations;

        private int current = 0;

        internal ServerSample(int iterations) => this.iterations = iterations;

        internal event Action? OnComplete;

        public async Task Execute(CancellationToken cancellationToken)
        {
            if (current == iterations)
            {
                OnComplete!();
            }

            await Task.Delay(1);
        }

        public void OnPublished(EmptyPacket emptyPacket) => current++;

        public void Register(Channel channel) => channel.Register(this);

        public void Unregister(Channel channel) => channel.Unregister(this);
    }
}
