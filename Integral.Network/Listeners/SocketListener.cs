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

namespace Integral.Listeners
{
    internal class SocketListener : TcpListenerDecorator, Listener
    {
        private readonly Encoding encoding;

        internal SocketListener(Uri uri, Encoding encoding) : base(uri) => this.encoding = encoding;

        public async Task<Transporter> Execute(CancellationToken cancellationToken)
        {
            TcpClient tcpClient = await Accept();
            return new SocketTransporter(new SocketConnection(tcpClient), new BufferedByteStream(await Initialize(tcpClient, cancellationToken)), encoding);
        }

        protected virtual Task<Stream> Initialize(TcpClient tcpClient, CancellationToken cancellationToken) => Task.FromResult((Stream)new NetworkStreamDecorator(tcpClient));
    }
}