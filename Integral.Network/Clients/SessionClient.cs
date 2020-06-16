using System.Threading;
using System.Threading.Tasks;
using Integral.Channels;
using Integral.Connectors;
using Integral.Hosts;
using Integral.Registries;
using Integral.Sessions;
using Integral.Transporters;

namespace Integral.Clients
{
    internal sealed class SessionClient : SessionHost, Client
    {
        private readonly Connector connector;

        private Task<Transporter>? connectTask;

        private Session? session;

        internal SessionClient(Connector connector, Registry<Channel> registry) : base(registry) => this.connector = connector;

        public override Task Initialize(CancellationToken cancellationToken) => connectTask = connector.Execute(cancellationToken);

        public override async Task Execute(CancellationToken cancellationToken)
        {
            if (session != null)
            {
                if (!await Execute(session!, cancellationToken))
                {
                    session = null;
                    connectTask = connector.Execute(cancellationToken);
                }
            }
            else if (connectTask!.IsCompleted)
            {
                session = Initialize(await connectTask);
                connectTask.Dispose();
                connectTask = null;
            }
        }
    }
}