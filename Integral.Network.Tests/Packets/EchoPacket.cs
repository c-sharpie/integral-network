using System.Threading;
using System.Threading.Tasks;
using Integral.Readers;
using Integral.Writers;

namespace Integral.Packets
{
    internal sealed class EchoPacket : TestPacket
    {
        private readonly byte[] data;

        internal EchoPacket(int size, int iterations)
            : base(iterations) => data = new byte[size];

        public async override ValueTask Read(PrimitiveReader primitiveReader, CancellationToken cancellationToken)
        {
            Reads++;
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = await primitiveReader.ReadByte(cancellationToken);
            }
        }

        public async override ValueTask Write(PrimitiveWriter primitiveWriter, CancellationToken cancellationToken)
        {
            Writes++;
            for (int i = 0; i < data.Length; i++)
            {
                await primitiveWriter.Write(data[i], cancellationToken);
            }
        }
    }
}
