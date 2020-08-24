using System;
using System.Buffers.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Integral.Streams;

namespace Integral.Writers
{
    internal sealed class ByteWriter : PrimitiveWriter
    {
        private readonly WriteStream<ReadOnlyMemory<byte>, ValueTask> writeStream;

        private readonly Encoding encoding;

        private Memory<byte> buffer = new byte[sizeof(long)];

        internal ByteWriter(WriteStream<ReadOnlyMemory<byte>, ValueTask> writeStream, Encoding encoding)
        {
            this.writeStream = writeStream;
            this.encoding = encoding;
        }

        public async ValueTask Write(string value, CancellationToken cancellationToken)
        {
            int length = encoding.GetByteCount(value);
            await Write(length, cancellationToken);
            if (buffer.Length < length)
            {
                // TODO: Implement max string length.
                buffer = new byte[length];
            }

            encoding.GetBytes(value, buffer.Span);
            await writeStream.WriteAsync(buffer.Slice(0, length), cancellationToken);
        }

        public async ValueTask Write(bool value, CancellationToken cancellationToken)
        {
            Write((byte)(value ? 1 : 0));
            await writeStream.WriteAsync(buffer.Slice(0, 1), cancellationToken);
        }

        public async ValueTask Write(byte value, CancellationToken cancellationToken)
        {
            Write(value);
            await writeStream.WriteAsync(buffer.Slice(0, 1), cancellationToken);
        }

        public async ValueTask Write(sbyte value, CancellationToken cancellationToken)
        {
            Write((byte)value);
            await writeStream.WriteAsync(buffer.Slice(0, 1), cancellationToken);
        }

        public async ValueTask Write(short value, CancellationToken cancellationToken)
        {
            BinaryPrimitives.WriteInt16LittleEndian(buffer.Span, value);
            await writeStream.WriteAsync(buffer.Slice(0, sizeof(short)), cancellationToken);
        }

        public async ValueTask Write(int value, CancellationToken cancellationToken)
        {
            BinaryPrimitives.WriteInt32LittleEndian(buffer.Span, value);
            await writeStream.WriteAsync(buffer.Slice(0, sizeof(int)), cancellationToken);
        }

        public async ValueTask Write(long value, CancellationToken cancellationToken)
        {
            BinaryPrimitives.WriteInt64LittleEndian(buffer.Span, value);
            await writeStream.WriteAsync(buffer.Slice(0, sizeof(long)), cancellationToken);
        }

        public async ValueTask Write(ushort value, CancellationToken cancellationToken)
        {
            BinaryPrimitives.WriteUInt16LittleEndian(buffer.Span, value);
            await writeStream.WriteAsync(buffer.Slice(0, sizeof(ushort)), cancellationToken);
        }

        public async ValueTask Write(uint value, CancellationToken cancellationToken)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(buffer.Span, value);
            await writeStream.WriteAsync(buffer.Slice(0, sizeof(uint)), cancellationToken);
        }

        public async ValueTask Write(ulong value, CancellationToken cancellationToken)
        {
            BinaryPrimitives.WriteUInt64LittleEndian(buffer.Span, value);
            await writeStream.WriteAsync(buffer.Slice(0, sizeof(ulong)), cancellationToken);
        }

        public async ValueTask Write(float value, CancellationToken cancellationToken)
        {
            Write(value);
            await writeStream.WriteAsync(buffer.Slice(0, sizeof(float)), cancellationToken);
        }

        public async ValueTask Write(double value, CancellationToken cancellationToken)
        {
            Write(value);
            await writeStream.WriteAsync(buffer.Slice(0, sizeof(double)), cancellationToken);
        }

        public async ValueTask Write(Guid value, CancellationToken cancellationToken) => await writeStream.WriteAsync(value.ToByteArray(), cancellationToken);

        public async ValueTask Write<Enumeration>(Enumeration value, CancellationToken cancellationToken)
            where Enumeration : Enum => await Write(Convert.ToInt32(value), cancellationToken);

        public async ValueTask Write(string[] value, CancellationToken cancellationToken)
        {
            await Write(value.Length, cancellationToken);
            foreach (string element in value)
            {
                await Write(element, cancellationToken);
            }
        }

        public async ValueTask Write(bool[] value, CancellationToken cancellationToken)
        {
            await Write(value.Length, cancellationToken);
            foreach (bool element in value)
            {
                await Write(element, cancellationToken);
            }
        }

        public async ValueTask Write(byte[] value, CancellationToken cancellationToken)
        {
            await Write(value.Length, cancellationToken);
            await writeStream.WriteAsync(value, cancellationToken);
        }

        public async ValueTask Write(sbyte[] value, CancellationToken cancellationToken)
        {
            await Write(value.Length, cancellationToken);
            foreach (sbyte element in value)
            {
                await Write(element, cancellationToken);
            }
        }

        public async ValueTask Write(short[] value, CancellationToken cancellationToken)
        {
            await Write(value.Length, cancellationToken);
            foreach (short element in value)
            {
                await Write(element, cancellationToken);
            }
        }

        public async ValueTask Write(int[] value, CancellationToken cancellationToken)
        {
            await Write(value.Length, cancellationToken);
            foreach (int element in value)
            {
                await Write(element, cancellationToken);
            }
        }

        public async ValueTask Write(long[] value, CancellationToken cancellationToken)
        {
            await Write(value.Length, cancellationToken);
            foreach (long element in value)
            {
                await Write(element, cancellationToken);
            }
        }

        public async ValueTask Write(ushort[] value, CancellationToken cancellationToken)
        {
            await Write(value.Length, cancellationToken);
            foreach (ushort element in value)
            {
                await Write(element, cancellationToken);
            }
        }

        public async ValueTask Write(uint[] value, CancellationToken cancellationToken)
        {
            await Write(value.Length, cancellationToken);
            foreach (uint element in value)
            {
                await Write(element, cancellationToken);
            }
        }

        public async ValueTask Write(ulong[] value, CancellationToken cancellationToken)
        {
            await Write(value.Length, cancellationToken);
            foreach (ulong element in value)
            {
                await Write(element, cancellationToken);
            }
        }

        public async ValueTask Write(float[] value, CancellationToken cancellationToken)
        {
            await Write(value.Length, cancellationToken);
            foreach (float element in value)
            {
                await Write(element, cancellationToken);
            }
        }

        public async ValueTask Write(double[] value, CancellationToken cancellationToken)
        {
            await Write(value.Length, cancellationToken);
            foreach (double element in value)
            {
                await Write(element, cancellationToken);
            }
        }

        public async ValueTask Write(Guid[] value, CancellationToken cancellationToken)
        {
            await Write(value.Length, cancellationToken);
            foreach (Guid element in value)
            {
                await Write(element, cancellationToken);
            }
        }

        public async ValueTask Write<Enumeration>(Enumeration[] value, CancellationToken cancellationToken)
            where Enumeration : Enum
        {
            await Write(value.Length, cancellationToken);
            foreach (Enumeration element in value)
            {
                await Write(element, cancellationToken);
            }
        }

        private void Write(byte value) => buffer.Span[0] = value;

        private unsafe void Write(float value)
        {
            Span<byte> span = buffer.Span;
            uint convertedValue = *(uint*)&value;
            span[0] = (byte)convertedValue;
            span[1] = (byte)(convertedValue >> 8);
            span[2] = (byte)(convertedValue >> 16);
            span[3] = (byte)(convertedValue >> 24);
        }

        private unsafe void Write(double value)
        {
            Span<byte> span = buffer.Span;
            ulong convertedValue = *(ulong*)&value;
            span[0] = (byte)convertedValue;
            span[1] = (byte)(convertedValue >> 8);
            span[2] = (byte)(convertedValue >> 16);
            span[3] = (byte)(convertedValue >> 24);
            span[4] = (byte)(convertedValue >> 32);
            span[5] = (byte)(convertedValue >> 40);
            span[6] = (byte)(convertedValue >> 48);
            span[7] = (byte)(convertedValue >> 56);
        }

        //private async ValueTask WriteEncodedInt32(int value, CancellationToken cancellationToken)
        //{
        //    while (value > sbyte.MaxValue)
        //    {
        //        await Write((byte)(value | 128), cancellationToken);
        //        value >>= 7;
        //    }

        //    await Write((byte)value, cancellationToken);
        //}
    }
}