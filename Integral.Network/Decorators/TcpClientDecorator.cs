using System.Net.Sockets;
using Integral.Constants;

namespace Integral.Decorators
{
    internal sealed class TcpClientDecorator : TcpClient
    {
        internal TcpClientDecorator()
        {
            Upgrade(this);
        }

        public static void Upgrade(TcpClient tcpClient)
        {
            tcpClient.SendTimeout = tcpClient.ReceiveTimeout = NetworkConstant.DefaultTimeout.Milliseconds;
            tcpClient.SendBufferSize = tcpClient.ReceiveBufferSize = NetworkConstant.DefaultBufferSize;
            tcpClient.NoDelay = true;
        }
    }
}