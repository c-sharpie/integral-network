using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Integral.Decorators;

namespace Integral.Listeners
{
    internal sealed class SecureSocketListener : SocketListener
    {
        private readonly SslServerAuthenticationOptions sslServerAuthenticationOptions;

        internal SecureSocketListener(SslServerAuthenticationOptions sslServerAuthenticationOptions, Uri uri, Encoding encoding)
            : base(uri, encoding) => this.sslServerAuthenticationOptions = sslServerAuthenticationOptions;

        protected override async Task<Stream> Initialize(TcpClient tcpClient, CancellationToken cancellationToken)
        {
            SslStreamDecorator sslStreamDecorator = new SslStreamDecorator(tcpClient);
            await sslStreamDecorator.AuthenticateAsServerAsync(sslServerAuthenticationOptions, cancellationToken);
            return sslStreamDecorator;
        }
    }
}