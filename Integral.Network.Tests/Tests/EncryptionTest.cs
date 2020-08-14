using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Integral.Connectors;
using Integral.Constructors;
using Integral.Factories;
using Integral.Listeners;
using Integral.Packets;
using Integral.Readers;
using Integral.Transporters;
using Integral.Writers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Integral.Tests
{
    [TestClass]
    public class EncryptionTest : Packet
    {
        //private const bool webSockets = false;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private string result = string.Empty, expected = "test";

        [TestMethod]
        public void TestMethod()
        {
            Task.WaitAll(Accept(), Connect());
            Assert.IsTrue(result.Equals(expected));
        }

        private async Task Accept()
        {
            ListenerFactory listenerFactory = new ListenerFactory();
            listenerFactory.Encrypt = true;
            if (CertificateConstructor.TryFindCertificate(out X509Certificate2 x509Certificate2))
            {
                listenerFactory.X509Certificate2 = x509Certificate2;
            }

            using Listener listener = listenerFactory.Create();
            using Transporter transporter = await listener.Execute(cancellationTokenSource.Token);
            await transporter.Read(this, cancellationTokenSource.Token);
        }

        private async Task Connect()
        {
            ConnectorFactory connectorFactory = new ConnectorFactory();
            connectorFactory.Encrypt = true;
            Connector connector = connectorFactory.Create();
            using Transporter transporter = await connector.Execute(cancellationTokenSource.Token);
            await transporter.Write(this, cancellationTokenSource.Token);
        }

        public async ValueTask Read(PrimitiveReader primitiveReader, CancellationToken cancellationToken) => result = await primitiveReader.ReadString(cancellationToken);

        public async ValueTask Write(PrimitiveWriter primitiveWriter, CancellationToken cancellationToken) => await primitiveWriter.Write(expected, cancellationToken);
    }
}