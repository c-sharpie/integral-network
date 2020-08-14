using System;
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
    public class LoadTest
    {
        private const bool webSockets = false;

        private const int Connections = 100, Iterations = 100, Ping = 100, Bytes = 100;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private readonly ListedCollection<Task> tasks = new ListedCollection<Task>();

        private readonly ListedCollection<Transporter> transporters = new ListedCollection<Transporter>();

        [TestMethod]
        public void TestMethod()
        {
            Task.WaitAll(Accept(), Connect());
            Task.WaitAll(tasks.ToArray());
            Assert.IsTrue(PingPacket.Ping < Ping);
        }

        private async Task Accept()
        {
            ListenerFactory listenerFactory = new ListenerFactory();
            listenerFactory.Uri = new Uri(webSockets ? "http://localhost:5001/" : "tcp://127.0.0.1:7000");
            using Listener listener = listenerFactory.Create();
            for (int i = 0; i < Connections; i++)
            {
                Transporter transporter = await listener.Execute(cancellationTokenSource.Token);
                EchoPacket echoPacket = new EchoPacket(Bytes, Iterations);
                tasks.Add(ExecuteRead(transporter, echoPacket));
                tasks.Add(ExecuteWrite(transporter, echoPacket));
                transporters.Add(transporter);
            }
        }

        private async Task Connect()
        {
            ConnectorFactory connectorFactory = new ConnectorFactory();
            connectorFactory.Uri = new Uri(webSockets ? "ws://localhost:5001/" : "tcp://127.0.0.1:7000");
            for (int i = 0; i < Connections; i++)
            {
                Connector connector = connectorFactory.Create();
                Transporter transporter = await connector.Execute(cancellationTokenSource.Token);
                PingPacket pingPacket = new PingPacket(Bytes, Iterations);
                tasks.Add(ExecuteRead(transporter, pingPacket));
                tasks.Add(ExecuteWrite(transporter, pingPacket));
                transporters.Add(transporter);
            }
        }

        private async Task ExecuteRead(Transporter transporter, TestPacket testPacket)
        {
            while (!testPacket.ReadComplete)
            {
                await transporter.Read(testPacket, cancellationTokenSource.Token);
                await Task.Yield();
            }
        }

        private async Task ExecuteWrite(Transporter transporter, TestPacket testPacket)
        {
            while (!testPacket.WriteComplete)
            {
                await transporter.Write(testPacket, cancellationTokenSource.Token);
                await Task.Yield();
            }
        }
    }
}