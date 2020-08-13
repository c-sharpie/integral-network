using System;
using System.Net.Security;
using Integral.Connectors;
using Integral.Constants;

namespace Integral.Factories
{
    public sealed class ConnectorFactory : TransportFactory, Factory<Connector>
    {
        public Uri Uri { get; set; } = NetworkConstant.DefaultUri;

        public Connector Create()
        {
            if (Encrypt)
            {
                SslClientAuthenticationOptions sslClientAuthenticationOptions = new SslClientAuthenticationOptions();
                sslClientAuthenticationOptions.TargetHost = Uri.DnsSafeHost;
                return new SecureSocketConnector(sslClientAuthenticationOptions, Uri, Encoding);
            }

            return new SocketConnector(Uri, Encoding);
        }
    }
}