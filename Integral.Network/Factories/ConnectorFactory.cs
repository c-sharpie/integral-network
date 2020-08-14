using System;
using System.Net.Security;
using Integral.Connectors;

namespace Integral.Factories
{
    public sealed class ConnectorFactory : TransportFactory, Factory<Connector>
    {
        public Uri? Uri { get; set; }

        public Connector Create()
        {
            switch (Uri!.Scheme)
            {
                case "tcp":
                    if (Encrypt)
                    {
                        SslClientAuthenticationOptions sslClientAuthenticationOptions = new SslClientAuthenticationOptions();
                        sslClientAuthenticationOptions.TargetHost = Uri.DnsSafeHost;
                        return new SecureSocketConnector(sslClientAuthenticationOptions, Uri, Encoding);
                    }

                    return new SocketConnector(Uri, Encoding);
                case "ws":
                case "wss":
                    return new WebSocketConnector(Uri, Encoding);
                case "udp":
                default:
                    throw new NotImplementedException(Uri.Scheme);
            }
        }
    }
}