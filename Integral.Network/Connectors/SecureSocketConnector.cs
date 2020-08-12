using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Integral.Decorators;

namespace Integral.Connectors
{
    internal sealed class SecureSocketConnector : SocketConnector
    {
        private readonly SslClientAuthenticationOptions sslClientAuthenticationOptions;

        internal SecureSocketConnector(SslClientAuthenticationOptions sslClientAuthenticationOptions, Encoding encoding, DnsEndPoint dnsEndPoint)
            : base(encoding, dnsEndPoint) => this.sslClientAuthenticationOptions = sslClientAuthenticationOptions;

        protected override async Task<Stream> Initialize(TcpClient tcpClient, CancellationToken cancellationToken)
        {
            SslStreamDecorator sslStreamDecorator = new SslStreamDecorator(tcpClient);
            await sslStreamDecorator.AuthenticateAsClientAsync(sslClientAuthenticationOptions, cancellationToken);
            return sslStreamDecorator;
        }
    }
}