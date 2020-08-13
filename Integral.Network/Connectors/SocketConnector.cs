using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Integral.Connections;
using Integral.Decorators;
using Integral.Streams;
using Integral.Transporters;

namespace Integral.Connectors
{
    internal class SocketConnector : Connector
    {
        private readonly Uri uri;

        private readonly Encoding encoding;

        internal SocketConnector(Uri uri, Encoding encoding)
        {
            this.uri = uri;
            this.encoding = encoding;
        }

        public override string ToString() => uri.ToString();

        public async Task<Transporter> Execute(CancellationToken cancellationToken)
        {
            TcpClient tcpClient = new TcpClientDecorator();
            await tcpClient.ConnectAsync(uri.Host, uri.Port);
            return new SocketTransporter(new SocketConnection(tcpClient), new BufferedByteStream(await Initialize(tcpClient, cancellationToken)), encoding);
        }

        protected virtual Task<Stream> Initialize(TcpClient tcpClient, CancellationToken cancellationToken) => Task.FromResult((Stream)new NetworkStreamDecorator(tcpClient));
    }
}