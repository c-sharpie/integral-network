using System.Net.Security;
using System.Net.Sockets;

namespace Integral.Decorators
{
    internal sealed class SslStreamDecorator : SslStream
    {
        internal SslStreamDecorator(TcpClient tcpClient) : base(new NetworkStreamDecorator(tcpClient))
        {
        }
    }
}