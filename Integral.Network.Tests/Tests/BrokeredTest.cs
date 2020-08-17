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

            ServerRegistry serverRegistry = new ServerRegistry(Iterations * Connections);
            serverRegistry.OnComplete += cancellationTokenSource.Cancel;

            ServerFactory serverFactory = new ServerFactory();
            serverFactory.ListenerFactory = listenerFactory;
            serverFactory.ChannelRegistry = serverRegistry;

            NetworkFactory networkFactory = new NetworkFactory();
            networkFactory.Executable = serverRegistry;
            networkFactory.Add(serverFactory);
            Network network = networkFactory.Create();

            tasks.Add(Run(network, cancellationTokenSource.Token));
        }

        private void Connect()
        {
            for (int i = 0; i < Connections; i++)
            {
                ConnectorFactory connectorFactory = new ConnectorFactory();
                connectorFactory.Uri = new Uri(webSockets ? "ws://localhost:5001/" : "tcp://127.0.0.1:7000");

                ClientRegistry clientRegistry = new ClientRegistry(Iterations);

                ClientFactory clientFactory = new ClientFactory();
                clientFactory.ConnectorFactory = connectorFactory;
                clientFactory.ChannelRegistry = clientRegistry;

                NetworkFactory networkFactory = new NetworkFactory();
                networkFactory.Executable = clientRegistry;
                networkFactory.Add(clientFactory);
                Network network = networkFactory.Create();

                tasks.Add(Run(network, cancellationTokenSource.Token));
            }
        }

        private async Task Run(Network network, CancellationToken cancellationToken)
        {
            await network.Initialize(cancellationToken);
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await network.Execute(cancellationToken);
                }
                catch (TaskCanceledException)
                {
                }
            }
        }
    }
}