using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Integral.Constants;

namespace Integral.Decorators
{
    internal abstract class TcpListenerDecorator : TcpListener
    {
        protected TcpListenerDecorator(IPEndPoint ipEndPoint) : base(ipEndPoint)
        {
        }

        public bool Enabled => Active;

        public override string ToString() => (Active ? LocalEndpoint.ToString() : IPAddress.None.ToString())!;

        public async Task<TcpClient> Accept()
        {
            if (!Active)
            {
                Start();
            }

            TcpClient tcpClient = await AcceptTcpClientAsync();
            tcpClient.SendBufferSize = tcpClient.ReceiveBufferSize = NetworkConstant.DefaultBufferSize;
            tcpClient.SendTimeout = tcpClient.ReceiveTimeout = NetworkConstant.DefaultTimeout;
            tcpClient.NoDelay = true;
            return tcpClient;
        }

        public void Dispose()
        {
            if (Active)
            {
                Stop();
            }
        }
    }
}