using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Integral.Collections;
using Integral.Connectors;
using Integral.Factories;
using Integral.Listeners;
using Integral.Packets;
using Integral.Transporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Integral.Tests
{
    [TestClass]
    public class SpeedTest
    {
        private const bool webSockets = false;

        private const int Connections = 100, Iterations = 100;

        private readonly DataPacket[][] dataPackets = DataPacket.CreatePrimitivePackets(Connections, Iterations);

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private readonly IndexedCollection<Transporter, Task> tasks = new IndexedCollection<Transporter, Task>();

        [TestMethod]
        public void TestMethod()
        {
            Task.WaitAll(Accept(), Connect());
            Task.WaitAll(tasks.Values.ToArray());
            Assert.IsTrue(DataPacket.Valid(dataPackets));
        }

        private async Task Accept()
        {
            ListenerFactory listenerFactory = new ListenerFactory();
            listenerFactory.Uri = new Uri(webSockets ? "http://localhost:5001/" : "tcp://127.0.0.1:7000");
            Listener listener = listenerFactory.Create();
            for (int i = 0; i < Connections; i++)
            {
                DataPacket dataPacket = dataPackets[1][i];
                Transporter transporter = await listener.Execute(cancellationTokenSource.Token);
                tasks.Add(transporter, Receive(transporter, dataPacket));
            }
        }

        private async Task Connect()
        {
            ConnectorFactory connectorFactory = new ConnectorFactory();
            connectorFactory.Uri = new Uri(webSockets ? "ws://localhost:5001/" : "tcp://127.0.0.1:7000");
            for (int i = 0; i < Connections; i++)
            {
                DataPacket dataPacket = dataPackets[0][i];
                Connector connector = connectorFactory.Create();
                Transporter transporter = await connector.Execute(cancellationTokenSource.Token);
                tasks.Add(transporter, Send(transporter, dataPacket));
            }
        }

        private async Task Send(Transporter transporter, DataPacket dataPacket)
        {
            while (dataPacket.ShouldSerialize)
            {
                await transporter.Write(dataPacket, cancellationTokenSource.Token);
            }
        }

        private async Task Receive(Transporter transporter, DataPacket dataPacket)
        {
            while (dataPacket.ShouldDeserialize)
            {
                await transporter.Read(dataPacket, cancellationTokenSource.Token);
            }
        }
    }
}