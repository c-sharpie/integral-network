using System.Net;

namespace Integral.Extensions
{
    public static class IPEndPointExtension
    {
        public static DnsEndPoint ToDnsEndPoint(this IPEndPoint ipEndPoint) => new DnsEndPoint(ipEndPoint.Address.ToString(), ipEndPoint.Port);
    }
}