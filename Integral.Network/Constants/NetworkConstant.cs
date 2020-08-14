using System;
using System.Text;

namespace Integral.Constants
{
    public static class NetworkConstant
    {
        internal static readonly TimeSpan DefaultTimeout = TimeSpan.FromMilliseconds(10000);

        internal static readonly Encoding DefaultEncoding = Encoding.Unicode;

        internal const int DefaultBufferSize = 8096;
    }
}