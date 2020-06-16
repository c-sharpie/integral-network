using System.Security.Cryptography.X509Certificates;

namespace Integral.Constructors
{
    internal static class CertificateConstructor
    {
        internal static bool TryFindCertificate(out X509Certificate2 x509Certificate2, string host = "localhost")
        {
            using X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection x509Certificate2Collection = store.Certificates.Find(X509FindType.FindBySubjectName, host, false);
            if (x509Certificate2Collection.Count > 0)
            {
                x509Certificate2 = x509Certificate2Collection[0];
                return true;
            }

            x509Certificate2 = default!;
            return false;
        }
    }
}