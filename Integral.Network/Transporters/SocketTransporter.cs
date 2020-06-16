using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Integral.Abstractions;
using Integral.Connections;
using Integral.Readers;
using Integral.Streams;
using Integral.Writers;

namespace Integral.Transporters
{
    internal sealed class SocketTransporter : SocketConnection, Transporter
    {
        private readonly ByteStream byteStream;

        private readonly PrimitiveReader primitiveReader;

        private readonly PrimitiveWriter primitiveWriter;

        internal SocketTransporter(ByteStream byteStream, Encoding encoding, TcpClient tcpClient) : base(tcpClient)
        {
            this.byteStream = byteStream;
            primitiveReader = new ByteReader(byteStream, encoding);
            primitiveWriter = new ByteWriter(byteStream, encoding);
        }

        public async ValueTask Read(Deserializable deserializable, CancellationToken cancellationToken)
        {
            while (byteStream.CanRead)
            {
                await deserializable.Read(primitiveReader, cancellationToken);
            }
        }

        public async ValueTask Write(Serializable serializable, CancellationToken cancellationToken)
        {
            await serializable.Write(primitiveWriter, cancellationToken);
            await byteStream.FlushAsync(cancellationToken);
        }

        public override void Dispose()
        {
            byteStream.Dispose();
            base.Dispose();
        }
    }
}