using System.Net;

namespace Integral.Extensions
{
    public static class DnsEndPointExtension
    {
        public static IPEndPoint ToIPEndPoint(this DnsEndPoint dnsEndPoint) => new IPEndPoint(Dns.GetHostAddresses(dnsEndPoint.Host)[0], dnsEndPoint.Port);
    }
}