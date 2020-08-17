using System;
using System.Threading;
using System.Threading.Tasks;
using Integral.Collections;
using Integral.Factories;
using Integral.Networks;
using Integral.Registries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Integral.Tests
{
    [TestClass]
    public class BrokeredTest
    {
        private const bool webSockets = true;

        private const int Connections = 100, Iterations = 100;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private readonly ListedCollection<Task> tasks = new ListedCollection<Task>();

        [TestMethod]
        public void TestMethod()
        {
            Accept();
            Connect();
            Task.WaitAll(tasks.ToArray());
            Assert.IsTrue(cancellationTokenSource.IsCancellationRequested);
        }

        private void Accept()
        {
            ListenerFactory listenerFactory = new ListenerFactory();
            listenerFactory.Uri = new Uri(webSockets ? "http://localhost:5001/" : "tcp://127.0.0.1:7000");

            ServerSample serverSample = new ServerSample(Iterations * Connections);
            serverSample.OnComplete += cancellationTokenSource.Cancel;

            ServerFactory serverFactory = new ServerFactory();
            serverFactory.ListenerFactory = listenerFactory;
            serverFactory.ChannelRegistry = serverSample;

            NetworkFactory networkFactory = new NetworkFactory();
            networkFactory.Executable = serverSample;
            networkFactory.Add(serverFactory);
            Network network = networkFactory.Create();

            tasks.Add(network.Execute(cancellationTokenSource.Token));
        }

        private void Connect()
        {
            for (int i = 0; i < Connections; i++)
            {
                ConnectorFactory connectorFactory = new ConnectorFactory();
                connectorFactory.Uri = new Uri(webSockets ? "ws://localhost:5001/" : "tcp://127.0.0.1:7000");

                ClientSample clientSample = new ClientSample(Iterations);

                ClientFactory clientFactory = new ClientFactory();
                clientFactory.ConnectorFactory = connectorFactory;
                clientFactory.ChannelRegistry = clientSample;

                NetworkFactory networkFactory = new NetworkFactory();
                networkFactory.Executable = clientSample;
                networkFactory.Add(clientFactory);
                Network network = networkFactory.Create();

                tasks.Add(network.Execute(cancellationTokenSource.Token));
            }
        }
    }
}