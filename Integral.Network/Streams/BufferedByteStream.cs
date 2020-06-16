using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Integral.Streams
{
    internal sealed class BufferedByteStream : ByteStream
    {
        private readonly Stream readStream, writeStream;

        public BufferedByteStream(Stream stream)
        {
            readStream = stream;
            writeStream = new BufferedStream(stream);
        }

        public bool CanRead => readStream.CanRead;

        public bool CanWrite => writeStream.CanWrite;

        public async ValueTask<int> ReadAsync(Memory<byte> memory, CancellationToken cancellationToken)
        {
            return await readStream.ReadAsync(memory, cancellationToken);
        }

        public async ValueTask WriteAsync(ReadOnlyMemory<byte> readOnlyMemory, CancellationToken cancellationToken)
        {
            await writeStream.WriteAsync(readOnlyMemory, cancellationToken);
        }

        public async Task FlushAsync(CancellationToken cancellationToken) => await writeStream.FlushAsync(cancellationToken);

        public void Dispose()
        {
            readStream.Dispose();
            writeStream.Dispose();
        }
    }
}