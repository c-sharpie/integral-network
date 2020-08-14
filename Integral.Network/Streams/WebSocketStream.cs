using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Integral.Streams
{
    internal sealed class WebSocketStream : AbstractStream
    {
        private readonly WebSocket webSocket;

        internal WebSocketStream(WebSocket webSocket) => this.webSocket = webSocket;

        public override bool CanRead => webSocket.State == WebSocketState.Open || webSocket.State == WebSocketState.CloseSent;

        public override async ValueTask<int> ReadAsync(Memory<byte> memory, CancellationToken cancellationToken = new CancellationToken())
        {
            ValueWebSocketReceiveResult valueTask = await webSocket.ReceiveAsync(memory, cancellationToken);
            return valueTask.Count;
        }

        public override async ValueTask WriteAsync(ReadOnlyMemory<byte> readOnlyMemory, CancellationToken cancellationToken = new CancellationToken())
        {
            await webSocket.SendAsync(readOnlyMemory, WebSocketMessageType.Binary, true, cancellationToken);
        }
    }
}