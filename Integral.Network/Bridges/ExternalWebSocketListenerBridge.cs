using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Integral.Connections;
using Integral.Factories;
using Integral.Listeners;
using Integral.Streams;
using Integral.Transporters;

namespace Integral.Consumers
{
    public sealed class ExternalWebSocketListenerBridge : Consumer<WebSocket>, Listener, Factory<Listener>
    {
        private readonly Encoding encoding;

        private readonly BufferBlock<WebSocket> bufferBlock = new BufferBlock<WebSocket>();

        public ExternalWebSocketListenerBridge(Encoding encoding) => this.encoding = encoding;

        public async Task<Transporter> Execute(CancellationToken cancellationToken)
        {
            WebSocket webSocket = await bufferBlock.ReceiveAsync();
            return new SocketTransporter(new WebSocketConnection(webSocket), new BufferedByteStream(new WebSocketStream(webSocket)), encoding);
        }

        public void Consume(WebSocket webSocket) => bufferBlock.Post(webSocket);

        public Listener Create() => this;

        public void Dispose()
        {
        }
    }
}
