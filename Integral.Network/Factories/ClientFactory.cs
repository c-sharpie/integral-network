using Integral.Clients;

namespace Integral.Factories
{
    public sealed class ClientFactory : HostFactory<Client>
    {
        public ConnectorFactory ConnectorFactory { get; set; } = new ConnectorFactory();

        public override Client Create() => new SessionClient(ConnectorFactory.Create(), ChannelRegistry!);
    }
}