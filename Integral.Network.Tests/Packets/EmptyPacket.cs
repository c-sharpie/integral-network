using System.Threading;
using System.Threading.Tasks;
using Integral.Readers;
using Integral.Writers;

namespace Integral.Packets
{
    internal sealed class EmptyPacket : Packet<byte>
    {
        public byte Identity => 1;

        public async ValueTask Read(PrimitiveReader primitiveReader, CancellationToken cancellationToken)
        {
            _ = await primitiveReader.ReadBool(cancellationToken);
        }

        public async ValueTask Write(PrimitiveWriter primitiveWriter, CancellationToken cancellationToken)
        {
            await primitiveWriter.Write(Identity, cancellationToken);
            await primitiveWriter.Write(true, cancellationToken);
        }
    }
}
