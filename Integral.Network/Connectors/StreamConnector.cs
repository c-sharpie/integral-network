using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Integral.Decorators;
using Integral.Streams;
using Integral.Transporters;

namespace Integral.Connectors
{
    internal class StreamConnector : Connector
    {
        private readonly Encoding encoding;

        private readonly DnsEndPoint dnsEndPoint;

        internal StreamConnector(Encoding encoding, DnsEndPoint dnsEndPoint)
        {
            this.encoding = encoding;
            this.dnsEndPoint = dnsEndPoint;
        }

        public override string ToString() => dnsEndPoint.ToString();

        public async Task<Transporter> Execute(CancellationToken cancellationToken)
        {
            TcpClient tcpClient = new TcpClientDecorator();
            await tcpClient.ConnectAsync(dnsEndPoint.Host, dnsEndPoint.Port);
            return new SocketTransporter(new BufferedByteStream(await Initialize(tcpClient, cancellationToken)), encoding, tcpClient);
        }

        protected virtual Task<Stream> Initialize(TcpClient tcpClient, CancellationToken cancellationToken) => Task.FromResult((Stream)new NetworkStreamDecorator(tcpClient));
    }
}