using Integral.Servers;

namespace Integral.Factories
{
    public sealed class ServerFactory : HostFactory<Server>
    {
        public ListenerFactory ListenerFactory { get; set; } = new ListenerFactory();

        public override Server Create() => new SessionServer(ListenerFactory.Create(), ChannelRegistry!);
    }
}