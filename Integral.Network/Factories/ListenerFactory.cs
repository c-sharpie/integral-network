using System;
using System.Net.Security;
using Integral.Constants;
using Integral.Listeners;

namespace Integral.Factories
{
    public sealed class ListenerFactory : TransportFactory, Factory<Listener>
    {
        public Uri Uri { get; set; } = NetworkConstant.DefaultUri;

        public Listener Create()
        {
            if (Encrypt)
            {
                SslServerAuthenticationOptions sslServerAuthenticationOptions = new SslServerAuthenticationOptions();
                sslServerAuthenticationOptions.ServerCertificate = X509Certificate2;
                return new SecureSocketListener(sslServerAuthenticationOptions, Uri, Encoding);
            }

            return new SocketListener(Uri, Encoding);
        }
    }
}