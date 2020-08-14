using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Integral.Connections;
using Integral.Streams;
using Integral.Transporters;

namespace Integral.Connectors
{
    internal class WebSocketConnector : Connector
    {
        private readonly Uri uri;

        private readonly Encoding encoding;

        internal WebSocketConnector(Uri uri, Encoding encoding)
        {
            this.uri = uri;
            this.encoding = encoding;
        }

        public override string ToString() => uri.ToString();

        public async Task<Transporter> Execute(CancellationToken cancellationToken)
        {
            ClientWebSocket clientWebSocket = new ClientWebSocket();
            await clientWebSocket.ConnectAsync(uri, cancellationToken);
            return new SocketTransporter(new WebSocketConnection(clientWebSocket), new BufferedByteStream(new WebSocketStream(clientWebSocket)), encoding);
        }
    }
}