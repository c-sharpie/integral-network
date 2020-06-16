using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Integral.Decorators
{
    internal sealed class NetworkStreamDecorator : NetworkStream
    {
        private readonly TcpClient tcpClient;

        internal NetworkStreamDecorator(TcpClient tcpClient) : base(tcpClient.Client, false) => this.tcpClient = tcpClient;

        public override bool CanRead => base.CanRead && DataAvailable;

        public override async ValueTask<int> ReadAsync(Memory<byte> memory, CancellationToken cancellationToken = new CancellationToken())
        {
            int bytes = 0;
            ValueTask<int> valueTask = base.ReadAsync(memory, cancellationToken);
            if (valueTask.IsCompleted)
            {
                bytes = valueTask.Result;
            }
            else
            {
                Task<int> task = valueTask.AsTask();
                Task delay = Task.Delay(tcpClient.ReceiveTimeout, cancellationToken);
                if ((await Task.WhenAny(task, delay)) == delay || task.IsFaulted)
                {
                    tcpClient.Close();
                }
                else
                {
                    bytes = task.Result;
                }
            }

            if (bytes == 0)
            {
                tcpClient.Close();
            }

            return bytes;
        }

        public override async ValueTask WriteAsync(ReadOnlyMemory<byte> readOnlyMemory, CancellationToken cancellationToken = new CancellationToken())
        {
            ValueTask valueTask = base.WriteAsync(readOnlyMemory, cancellationToken);
            if (valueTask.IsCompleted)
            {
                await valueTask;
            }
            else
            {
                Task task = valueTask.AsTask();
                Task delay = Task.Delay(tcpClient.SendTimeout, cancellationToken);
                if ((await Task.WhenAny(task, delay)) == delay || task.IsFaulted)
                {
                    tcpClient.Close();
                }
            }
        }
    }
}