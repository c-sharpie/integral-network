using System.Threading;
using System.Threading.Tasks;
using Integral.Channels;
using Integral.Collections;
using Integral.Hosts;
using Integral.Listeners;
using Integral.Registries;
using Integral.Sessions;
using Integral.Transporters;

namespace Integral.Servers
{
    internal sealed class SessionServer : SessionHost, Server
    {
        private readonly ListedCollection<Session> sessions = new ListedCollection<Session>();

        private readonly Listener listener;

        private Task<Transporter>? listenTask;

        internal SessionServer(Listener listener, Registry<Channel> registry) : base(registry) => this.listener = listener;

        public override Task Initialize(CancellationToken cancellationToken) => listenTask = listener.Execute(cancellationToken);

        public override async Task Execute(CancellationToken cancellationToken)
        {
            if (listenTask!.IsCompleted)
            {
                sessions.Add(Initialize(await listenTask));
                listenTask.Dispose();
                listenTask = listener.Execute(cancellationToken);
            }

            for (int i = 0; i < sessions.Count; i++)
            {
                if (!await Execute(sessions[i], cancellationToken))
                {
                    sessions.RemoveAt(i);
                }
            }
        }
    }
}