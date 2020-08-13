using Integral.Collections;
using Integral.Hosts;
using Integral.Networks;

namespace Integral.Factories
{
    public sealed class NetworkFactory : ListedCollection<Factory<Host>>, Factory<Network>
    {
        public Network Create()
        {
            HostNetwork hostNetwork = new HostNetwork();
            foreach (Factory<Host> hostFactory in this)
            {
                hostNetwork.Add(hostFactory.Create());
            }

            return hostNetwork;
        }
    }
}