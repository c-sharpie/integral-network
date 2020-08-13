using System;
using System.Text;

namespace Integral.Constants
{
    public static class NetworkConstant
    {
        internal static readonly Uri DefaultUri = new Uri("tcp://127.0.0.1:7000");

        internal static readonly Encoding DefaultEncoding = Encoding.Unicode;

        internal const int DefaultBufferSize = 8096;

        internal const int DefaultTimeout = 10000;
    }
}