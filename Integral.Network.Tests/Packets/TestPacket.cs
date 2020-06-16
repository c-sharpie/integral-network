using System.Threading;
using System.Threading.Tasks;
using Integral.Readers;
using Integral.Writers;

namespace Integral.Packets
{
    internal abstract class TestPacket : Packet
    {
        protected int Iterations { get; }

        protected int Reads { get; set; }

        protected int Writes { get; set; }

        public TestPacket(int iterations) => Iterations = iterations;

        internal bool ReadComplete => Reads == Iterations;

        internal bool WriteComplete => Writes == Iterations;

        internal bool Complete => ReadComplete && WriteComplete;

        public abstract ValueTask Read(PrimitiveReader data, CancellationToken cancellationToken);

        public abstract ValueTask Write(PrimitiveWriter data, CancellationToken cancellationToken);
    }
}
