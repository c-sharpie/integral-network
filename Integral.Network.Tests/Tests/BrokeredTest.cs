using System.Threading;
using System.Threading.Tasks;
using Integral.Abstractions;
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
            ServerRegistry serverRegistry = new ServerRegistry(Iterations * Connections);
            serverRegistry.OnComplete += cancellationTokenSource.Cancel;

            ServerFactory serverFactory = new ServerFactory();
            serverFactory.ChannelRegistry = serverRegistry;

            NetworkFactory networkFactory = new NetworkFactory();
            networkFactory.Add(serverFactory);
            Network network = networkFactory.Create();

            tasks.Add(Run(network, serverRegistry, cancellationTokenSource.Token));
        }

        private void Connect()
        {
            for (int i = 0; i < Connections; i++)
            {
                ClientRegistry clientRegistry = new ClientRegistry(Iterations);

                ClientFactory clientFactory = new ClientFactory();
                clientFactory.ChannelRegistry = clientRegistry;

                NetworkFactory networkFactory = new NetworkFactory();
                networkFactory.Add(clientFactory);
                Network network = networkFactory.Create();

                tasks.Add(Run(network, clientRegistry, cancellationTokenSource.Token));
            }
        }

        private async Task Run(Network network, Executable<Task> executable, CancellationToken cancellationToken)
        {
            await network.Initialize(cancellationToken);
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await network.Execute(cancellationToken);
                    await executable.Execute(cancellationToken);
                }
                catch (TaskCanceledException)
                {
                }
            }
        }
    }
}