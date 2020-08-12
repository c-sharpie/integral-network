using System.Net.Sockets;
using Integral.Diagnostics;

namespace Integral.Connections
{
    internal sealed class SocketConnection : Connection
    {
        private readonly TcpClient tcpClient;

        private readonly string address;

        internal SocketConnection(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;

            Socket socket = tcpClient.Client;
            address = $"[ { socket.LocalEndPoint } -> { socket.RemoteEndPoint } ]";

            Log.Write("Connected " + address);
        }

        public bool Enabled => tcpClient.Connected;

        public override string ToString() => address;

        public void Dispose()
        {
            tcpClient.Dispose();

            Log.Write("Disconnected " + address);
        }
    }
}