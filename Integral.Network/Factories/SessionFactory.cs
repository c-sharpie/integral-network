using Integral.Sessions;
using Integral.Transporters;

namespace Integral.Factories
{
    public sealed class SessionFactory : Factory<Session>
    {
        public Transporter? Transporter { get; set; }

        public Session Create() => new TransporterSession(Transporter!);
    }
}
