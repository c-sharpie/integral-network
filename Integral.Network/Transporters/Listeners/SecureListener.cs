using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Integral.Decorators;

namespace Integral.Listeners
{
    internal sealed class SecureListener : StreamListener
    {
        private readonly SslServerAuthenticationOptions sslServerAuthenticationOptions;

        internal SecureListener(SslServerAuthenticationOptions sslServerAuthenticationOptions, Encoding encoding, IPEndPoint ipEndPoint)
            : base(encoding, ipEndPoint) => this.sslServerAuthenticationOptions = sslServerAuthenticationOptions;

        protected override async Task<Stream> Initialize(TcpClient tcpClient, CancellationToken cancellationToken)
        {
            SslStreamDecorator sslStreamDecorator = new SslStreamDecorator(tcpClient);
            await sslStreamDecorator.AuthenticateAsServerAsync(sslServerAuthenticationOptions, cancellationToken);
            return sslStreamDecorator;
        }
    }
}