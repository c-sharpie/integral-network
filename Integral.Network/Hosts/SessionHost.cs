using System.Threading;
using System.Threading.Tasks;
using Integral.Channels;
using Integral.Factories;
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

        public abstract Task Execute(CancellationToken cancellationToken);

        protected Session Initialize(Transporter transporter)
        {
            SessionFactory sessionFactory = new SessionFactory();
            sessionFactory.Transporter = transporter;
            Session session = sessionFactory.Create();

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