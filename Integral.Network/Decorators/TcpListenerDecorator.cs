using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Integral.Decorators
{
    internal abstract class TcpListenerDecorator : TcpListener
    {
        protected TcpListenerDecorator(Uri uri) : base(new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port))
        {
        }

        public override string ToString() => (Active ? LocalEndpoint.ToString() : IPAddress.None.ToString())!;

        public async Task<TcpClient> Accept()
        {
            if (!Active)
            {
                Start();
            }

            TcpClient tcpClient = await AcceptTcpClientAsync();
            TcpClientDecorator.Upgrade(tcpClient);
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