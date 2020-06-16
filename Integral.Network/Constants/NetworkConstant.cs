using System.Net;
using System.Text;

namespace Integral.Constants
{
    public static class NetworkConstant
    {
        public static readonly IPEndPoint DefaultIpEndPoint = new IPEndPoint(IPAddress.Loopback, DefaultPort);

        public static readonly DnsEndPoint DefaultDnsEndPoint = new DnsEndPoint(IPAddress.Loopback.ToString(), DefaultPort);

        internal static readonly Encoding DefaultEncoding = Encoding.Unicode;

        internal const int DefaultBufferSize = 8096;

        internal const int DefaultTimeout = 10000;

        private const int DefaultPort = 7777;
    }
}