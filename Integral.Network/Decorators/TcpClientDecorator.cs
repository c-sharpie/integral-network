using System.Net.Sockets;
using Integral.Constants;

namespace Integral.Decorators
{
    internal sealed class TcpClientDecorator : TcpClient
    {
        internal TcpClientDecorator()
        {
            SendBufferSize = ReceiveBufferSize = NetworkConstant.DefaultBufferSize;
            SendTimeout = ReceiveTimeout = NetworkConstant.DefaultTimeout;
            NoDelay = true;
        }
    }
}