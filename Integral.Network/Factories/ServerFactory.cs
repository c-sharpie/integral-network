using Integral.Listeners;
using Integral.Servers;

namespace Integral.Factories
{
    public sealed class ServerFactory : HostFactory<Server>
    {
        public Factory<Listener>? ListenerFactory { get; set; }

        public override Server Create() => new SessionServer(ListenerFactory!.Create(), ChannelRegistry!);
    }
}