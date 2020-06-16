using System.Threading;
using System.Threading.Tasks;
using Integral.Channels;
using Integral.Constructors;
using Integral.Transporters;

namespace Integral.Sessions
{
    internal sealed class TransporterSession : PacketChannel, Session
    {
        private readonly Transporter transporter;

        private ValueTask readTask = ValueTaskConstructor.Create(), writeTask = ValueTaskConstructor.Create();

        internal TransporterSession(Transporter transporter) => this.transporter = transporter;

        public async Task Execute(CancellationToken cancellationToken)
        {
            bool read = readTask.IsCompleted;
            bool write = writeTask.IsCompleted;

            if (read)
            {
                await readTask;
                readTask = transporter.Read(this, cancellationToken);
            }

            if (write)
            {
                await writeTask;
                writeTask = transporter.Write(this, cancellationToken);
            }

            if (read || write)
            {
                Execute();
            }
        }

        public override void Dispose() => transporter.Dispose();
    }
}