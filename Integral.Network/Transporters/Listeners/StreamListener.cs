using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Integral.Decorators;
using Integral.Streams;
using Integral.Transporters;

namespace Integral.Listeners
{
    internal class StreamListener : TcpListenerDecorator, Listener
    {
        private readonly Encoding encoding;

        internal StreamListener(Encoding encoding, IPEndPoint ipEndPoint) : base(ipEndPoint) => this.encoding = encoding;

        public async Task<Transporter> Execute(CancellationToken cancellationToken)
        {
            TcpClient tcpClient = await Accept();
            return new SocketTransporter(new BufferedByteStream(await Initialize(tcpClient, cancellationToken)), encoding, tcpClient);
        }

        protected virtual Task<Stream> Initialize(TcpClient tcpClient, CancellationToken cancellationToken) => Task.FromResult((Stream)new NetworkStreamDecorator(tcpClient));
    }
}