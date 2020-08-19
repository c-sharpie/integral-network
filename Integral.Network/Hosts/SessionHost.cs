using System.Threading;
using System.Threading.Tasks;
using Integral.Channels;
using Integral.Registries;
using Integral.Sessions;
using Integral.Transporters;

namespace Integral.Hosts
{
    internal abstract class SessionHost : Host
    {
        private readonly Registry<Channel> registry;

        protected SessionHost(Registry<Channel> registry) => this.registry = registry;

        public abstract Task Initialize(CancellationToken cancellationToken);

        public abstract Task Iterate(CancellationToken cancellationToken);

        protected Session Initialize(Transporter transporter)
        {
            Session session = new TransporterSession(transporter);

            registry.Register(session);

            return session;
        }

        protected async Task<bool> Execute(Session session, CancellationToken cancellationToken)
        {
            try
            {
                await session.Execute(cancellationToken);
            }
            catch
            {
                registry.Unregister(session);
                session.Dispose();
                return false;
            }

            return true;
        }
    }
}