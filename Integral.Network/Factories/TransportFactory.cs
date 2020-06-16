using System.Security.Cryptography.X509Certificates;
using System.Text;
using Integral.Constants;

namespace Integral.Factories
{
    public abstract class TransportFactory
    {
        public bool Encrypt { get; set; }

        public X509Certificate2? X509Certificate2 { get; set; }

        public Encoding Encoding { get; set; } = NetworkConstant.DefaultEncoding;
    }
}