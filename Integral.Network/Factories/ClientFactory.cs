using Integral.Clients;
using Integral.Connectors;

namespace Integral.Factories
{
    public sealed class ClientFactory : HostFactory<Client>
    {
        public Factory<Connector>? ConnectorFactory { get; set; }

        public override Client Create() => new SessionClient(ConnectorFactory!.Create(), ChannelRegistry!);
    }
}