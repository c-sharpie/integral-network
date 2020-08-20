using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Integral.Collections;
using Integral.Connections;
using Integral.Factories;
using Integral.Listeners;
using Integral.Streams;
using Integral.Transporters;

/*
 * The purpose of this hack is to integrate with the Kestrel pipeline rather than using plain HttpListener.
 * The "Use" middleware integration method is called for every request so the sockets go out of scope immediately. 
 * This prevents that scope from returning until the socket is disposed. There doesn't seem to be another way without giving up Kestrel.
 */

namespace Integral.Consumers
{
    public sealed class ExternalWebSocketListenerBridge : ListedCollection<Transporter>, Consumer<WebSocket, Task>, Listener, Factory<Listener>
    {
        private class MiddlewareKeepaliveHack : TaskCompletionSource<object>, Connection, IDisposable
        {
            private readonly Connection connection;

            public MiddlewareKeepaliveHack(Connection connection) => this.connection = connection;

            public bool Enabled => connection.Enabled;

            public void Dispose()
            {
                connection.Dispose();
                this.TrySetResult(new object());
            }
        }

        private readonly Encoding encoding;

        private readonly BufferBlock<Transporter> bufferBlock = new BufferBlock<Transporter>();

        public ExternalWebSocketListenerBridge(Encoding encoding) => this.encoding = encoding;

        public async Task<Transporter> Execute(CancellationToken cancellationToken) => await bufferBlock.ReceiveAsync();

        public async Task Consume(WebSocket webSocket)
        {
            MiddlewareKeepaliveHack middlewareKeepaliveHack = new MiddlewareKeepaliveHack(new WebSocketConnection(webSocket));
            Transporter transporter = new SocketTransporter(middlewareKeepaliveHack, new BufferedByteStream(new WebSocketStream(webSocket)), encoding);
            bufferBlock.Post(transporter);
            this.Add(transporter);

            await middlewareKeepaliveHack.Task;
        }

        public Listener Create() => this;

        public void Dispose()
        {
            foreach (Transporter transporter in this)
            {
                transporter.Dispose();
            }

            this.Clear();
        }
    }
}
