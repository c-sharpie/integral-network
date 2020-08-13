using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Integral.Collections;
using Integral.Connectors;
using Integral.Factories;
using Integral.Listeners;
using Integral.Packets;
using Integral.Transporters;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Integral.Tests
{
    [TestClass]
    public class WebTest
    {
        private const int Connections = 100, Iterations = 100;

        private readonly DataPacket[][] dataPackets = DataPacket.CreatePrimitivePackets(Connections, Iterations);

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private readonly IndexedCollection<Transporter, Task> tasks = new IndexedCollection<Transporter, Task>();

        [TestMethod]
        public void TestMethod()
        {
            void Configure(IApplicationBuilder applicationBuilder)
            {
                applicationBuilder.UseWebSockets();
                applicationBuilder.Use(async (context, next) =>
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    }
                    else
                    {
                        await next();
                    }
                });
            };

            Task task = WebHost.CreateDefaultBuilder(new string[0]).UseKestrel().Configure(Configure).Build().RunAsync();

            using (ClientWebSocket clientWebSocket = new ClientWebSocket())
            {
                clientWebSocket.ConnectAsync(new Uri("wss://localhost:5001/"), CancellationToken.None).Wait();
                //while (clientWebSocket.State == WebSocketState.Open)
                //{ 
                //    await clientWebSocket.SendAsync(bytesToSend, WebSocketMessageType.Text, true, CancellationToken.None);
                //    WebSocketReceiveResult result = await clientWebSocket.ReceiveAsync(bytesReceived, CancellationToken.None);
                //}
            }

            //Task.WaitAll(Accept(), Connect());
            //Task.WaitAll(tasks.Values.ToArray());
            //Assert.IsTrue(DataPacket.Valid(dataPackets));
        }

        private async Task Accept()
        {
            ListenerFactory listenerFactory = new ListenerFactory();
            using Listener listener = listenerFactory.Create();
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