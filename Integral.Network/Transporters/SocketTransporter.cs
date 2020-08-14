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
    internal sealed class SocketTransporter : Transporter
    {
        private readonly Connection connection;

        private readonly ByteStream byteStream;

        private readonly PrimitiveReader primitiveReader;

        private readonly PrimitiveWriter primitiveWriter;

        internal SocketTransporter(Connection connection, ByteStream byteStream, Encoding encoding)
        {
            this.connection = connection;
            this.byteStream = byteStream;
            primitiveReader = new ByteReader(byteStream, encoding);
            primitiveWriter = new ByteWriter(byteStream, encoding);
        }

        public bool Enabled => connection.Enabled;

        public async ValueTask Read(Deserializable deserializable, CancellationToken cancellationToken)
        {
            if (byteStream.CanRead)
            {
                await deserializable.Read(primitiveReader, cancellationToken);
            }
        }

        public async ValueTask Write(Serializable serializable, CancellationToken cancellationToken)
        {
            await serializable.Write(primitiveWriter, cancellationToken);
            await byteStream.FlushAsync(cancellationToken);
        }

        public void Dispose()
        {
            byteStream.Dispose();
            connection.Dispose();
        }
    }
}