using System.Net;
using System.Net.Security;
using Integral.Connectors;
using Integral.Constants;

namespace Integral.Factories
{
    public sealed class ConnectorFactory : TransportFactory, Factory<Connector>
    {
        public DnsEndPoint DnsEndPoint { get; set; } = NetworkConstant.DefaultDnsEndPoint;

        public Connector Create()
        {
            if (Encrypt)
            {
                SslClientAuthenticationOptions sslClientAuthenticationOptions = new SslClientAuthenticationOptions();
                sslClientAuthenticationOptions.TargetHost = DnsEndPoint.Host;
                return new SecureSocketConnector(sslClientAuthenticationOptions, Encoding, DnsEndPoint);
            }

            return new SocketConnector(Encoding, DnsEndPoint);
        }
    }
}