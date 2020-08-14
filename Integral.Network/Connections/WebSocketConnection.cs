using System.Net.WebSockets;
using Integral.Diagnostics;

namespace Integral.Connections
{
    internal sealed class WebSocketConnection : Connection
    {
        private readonly WebSocket webSocket;

        internal WebSocketConnection(WebSocket webSocket)
        {
            this.webSocket = webSocket;

            Log.Write($"Connected {webSocket}");
        }

        public bool Enabled => webSocket.State == WebSocketState.Open;

        public override string ToString() => webSocket?.ToString() ?? string.Empty;

        public void Dispose()
        {
            webSocket.Dispose();

            Log.Write($"Disconnected {webSocket}");
        }
    }
}