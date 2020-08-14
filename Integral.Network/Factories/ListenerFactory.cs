using System;
using System.Net.Security;
using Integral.Listeners;

namespace Integral.Factories
{
    public sealed class ListenerFactory : TransportFactory, Factory<Listener>
    {
        public Uri? Uri { get; set; }

        public Listener Create()
        {
            switch (Uri!.Scheme)
            {
                case "tcp":
                    if (Encrypt)
                    {
                        SslServerAuthenticationOptions sslServerAuthenticationOptions = new SslServerAuthenticationOptions();
                        sslServerAuthenticationOptions.ServerCertificate = X509Certificate2;
                        return new SecureSocketListener(sslServerAuthenticationOptions, Uri, Encoding);
                    }

                    return new SocketListener(Uri, Encoding);
                case "http":
                    return new WebSocketListener(Uri, Encoding);
                case "udp":
                default:
                    throw new NotImplementedException(Uri.Scheme);
            }
        }
    }
}