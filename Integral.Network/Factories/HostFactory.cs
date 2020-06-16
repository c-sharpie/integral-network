using Integral.Channels;
using Integral.Hosts;
using Integral.Registries;

namespace Integral.Factories
{
    public abstract class HostFactory<HostConstraint> : Factory<HostConstraint>
        where HostConstraint : Host
    {
        public Registry<Channel>? ChannelRegistry { get; set; }

        public abstract HostConstraint Create();
    }
}