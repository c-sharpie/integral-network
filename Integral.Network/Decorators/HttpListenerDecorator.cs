using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Integral.Decorators
{
    internal abstract class HttpListenerDecorator
    {
        private readonly HttpListener httpListener = new HttpListener();

        protected HttpListenerDecorator(Uri uri) => httpListener.Prefixes.Add(uri.OriginalString);

        public override string ToString() => httpListener.ToString() ?? IPAddress.None.ToString();

        public async Task<WebSocket> Accept()
        {
            if (!httpListener.IsListening)
            {
                httpListener.Start();
            }

            HttpListenerContext httpListenerContext = await httpListener.GetContextAsync();
            if (httpListenerContext.Request.IsWebSocketRequest)
            {
                WebSocketContext webSocketContext = await httpListenerContext.AcceptWebSocketAsync(null);
                return webSocketContext.WebSocket;
            }
            else
            {
                return await Accept();
            }
        }

        public void Dispose()
        {
            if (httpListener.IsListening)
            {
                httpListener.Stop();
            }
        }
    }
}
