using System;
using System.Collections.Generic;
using System.Net;
using Integral.Abstractions;
using Integral.Channels;
using Integral.Collections;
using Integral.Hosts;
using Integral.Networks;
using Integral.Registries;

namespace Integral.Factories
{
    public sealed class NetworkFactory : IndexedCollection<Registry<Channel>, Enumerable<EndPoint>>, Factory<Network>
    {
        public Network Create()
        {
            ListedCollection<Factory<Host>> hostFactories = new ListedCollection<Factory<Host>>();
            foreach (KeyValuePair<Registry<Channel>, Enumerable<EndPoint>> keyValuePair in this)
            {
                foreach (EndPoint endPoint in keyValuePair.Value)
                {
                    switch (endPoint)
                    {
                        case IPEndPoint ipEndPoint:
                            ServerFactory serverFactory = new ServerFactory();
                            serverFactory.ListenerFactory.IpEndPoint = ipEndPoint;
                            serverFactory.ChannelRegistry = keyValuePair.Key;
                            hostFactories.Add(serverFactory);
                            break;
                        case DnsEndPoint dnsEndPoint:
                            ClientFactory clientFactory = new ClientFactory();
                            clientFactory.ConnectorFactory.DnsEndPoint = dnsEndPoint;
                            clientFactory.ChannelRegistry = keyValuePair.Key;
                            hostFactories.Add(clientFactory);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }

            HostNetwork hostNetwork = new HostNetwork();
            foreach (Factory<Host> hostFactory in hostFactories)
            {
                hostNetwork.Add(hostFactory.Create());
            }

            return hostNetwork;
        }
    }
}