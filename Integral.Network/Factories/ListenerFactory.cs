using System.Net;
using System.Net.Security;
using Integral.Constants;
using Integral.Listeners;

namespace Integral.Factories
{
    public sealed class ListenerFactory : TransportFactory, Factory<Listener>
    {
        public IPEndPoint IpEndPoint { get; set; } = NetworkConstant.DefaultIpEndPoint;

        public Listener Create()
        {
            if (Encrypt)
            {
                SslServerAuthenticationOptions sslServerAuthenticationOptions = new SslServerAuthenticationOptions();
                sslServerAuthenticationOptions.ServerCertificate = X509Certificate2;
                return new SecureSocketListener(sslServerAuthenticationOptions, Encoding, IpEndPoint);
            }

            return new SocketListener(Encoding, IpEndPoint);
        }
    }
}