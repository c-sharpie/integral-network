using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Integral.Connections;
using Integral.Decorators;
using Integral.Streams;
using Integral.Transporters;

namespace Integral.Listeners
{
    internal sealed class WebSocketListener : HttpListenerDecorator, Listener
    {
        private readonly Encoding encoding;

        internal WebSocketListener(Uri uri, Encoding encoding)
            : base(uri) => this.encoding = encoding;

        public async Task<Transporter> Execute(CancellationToken cancellationToken)
        {
            WebSocket webSocket = await Accept();
            return new SocketTransporter(new WebSocketConnection(webSocket), new BufferedByteStream(new WebSocketStream(webSocket)), encoding);
        }
    }
}
