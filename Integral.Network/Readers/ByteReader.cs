using System;
using System.Buffers.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Integral.Constants;
using Integral.Streams;

namespace Integral.Readers
{
    internal sealed class ByteReader : PrimitiveReader
    {
        private readonly ReadStream<Memory<byte>, ValueTask<int>> readStream;

        private readonly Encoding encoding;

        private Memory<byte> buffer = new byte[PrimitiveConstant.GuidSize];

        internal ByteReader(ReadStream<Memory<byte>, ValueTask<int>> readStream, Encoding encoding)
        {
            this.readStream = readStream;
            this.encoding = encoding;
        }

        public async ValueTask<string> ReadString(CancellationToken cancellationToken)
        {
            int length = await ReadInt32(cancellationToken);
            if (buffer.Length < byte.MaxValue)
            {
                buffer = new byte[byte.MaxValue];
            }

            if (length > byte.MaxValue)
            {
                // TODO
                throw new NotImplementedException();
            }

            await readStream.ReadAsync(buffer.Slice(0, length), cancellationToken);
            return encoding.GetString(buffer.Span.Slice(0, length));
        }

        public async ValueTask<bool> ReadBool(CancellationToken cancellationToken)
        {
            await readStream.ReadAsync(buffer.Slice(0, 1), cancellationToken);
            return ReadByte() == 1;
        }

        public async ValueTask<byte> ReadByte(CancellationToken cancellationToken)
        {
            await readStream.ReadAsync(buffer.Slice(0, 1), cancellationToken);
            return ReadByte();
        }

        public async ValueTask<sbyte> ReadSByte(CancellationToken cancellationToken)
        {
            await readStream.ReadAsync(buffer.Slice(0, 1), cancellationToken);
            return (sbyte)ReadByte();
        }

        public async ValueTask<short> ReadInt16(CancellationToken cancellationToken)
        {
            await readStream.ReadAsync(buffer.Slice(0, sizeof(short)), cancellationToken);
            return BinaryPrimitives.ReadInt16LittleEndian(buffer.Span);
        }

        public async ValueTask<int> ReadInt32(CancellationToken cancellationToken)
        {
            await readStream.ReadAsync(buffer.Slice(0, sizeof(int)), cancellationToken);
            return BinaryPrimitives.ReadInt32LittleEndian(buffer.Span);
        }

        public async ValueTask<long> ReadInt64(CancellationToken cancellationToken)
        {
            await readStream.ReadAsync(buffer.Slice(0, sizeof(long)), cancellationToken);
            return BinaryPrimitives.ReadInt64LittleEndian(buffer.Span);
        }

        public async ValueTask<ushort> ReadUInt16(CancellationToken cancellationToken)
        {
            await readStream.ReadAsync(buffer.Slice(0, sizeof(ushort)), cancellationToken);
            return BinaryPrimitives.ReadUInt16LittleEndian(buffer.Span);
        }

        public async ValueTask<uint> ReadUInt32(CancellationToken cancellationToken)
        {
            await readStream.ReadAsync(buffer.Slice(0, sizeof(uint)), cancellationToken);
            return BinaryPrimitives.ReadUInt32LittleEndian(buffer.Span);
        }

        public async ValueTask<ulong> ReadUInt64(CancellationToken cancellationToken)
        {
            await readStream.ReadAsync(buffer.Slice(0, sizeof(ulong)), cancellationToken);
            return BinaryPrimitives.ReadUInt64LittleEndian(buffer.Span);
        }

        public async ValueTask<float> ReadSingle(CancellationToken cancellationToken)
        {
            await readStream.ReadAsync(buffer.Slice(0, sizeof(float)), cancellationToken);
            return ReadSingle();
        }

        public async ValueTask<double> ReadDouble(CancellationToken cancellationToken)
        {
            await readStream.ReadAsync(buffer.Slice(0, sizeof(double)), cancellationToken);
            return ReadDouble();
        }

        public async ValueTask<Guid> ReadGuid(CancellationToken cancellationToken)
        {
            await readStream.ReadAsync(buffer.Slice(0, PrimitiveConstant.GuidSize), cancellationToken);
            return new Guid(buffer.Span.Slice(0, PrimitiveConstant.GuidSize));
        }

        public async ValueTask<Enumeration> ReadEnum<Enumeration>(CancellationToken cancellationToken)
            where Enumeration : Enum => (Enumeration)Enum.ToObject(typeof(Enumeration), await ReadInt32(cancellationToken));

        public async ValueTask<string[]> ReadStringArray(CancellationToken cancellationToken)
        {
            int count = await ReadInt32(cancellationToken);
            string[] value = new string[count];
            for (int i = 0; i < count; i++)
            {
                value[i] = await ReadString(cancellationToken);
            }

            return value;
        }

        public async ValueTask<bool[]> ReadBoolArray(CancellationToken cancellationToken)
        {
            int count = await ReadInt32(cancellationToken);
            bool[] value = new bool[count];
            for (int i = 0; i < count; i++)
            {
                value[i] = await ReadBool(cancellationToken);
            }

            return value;
        }

        public async ValueTask<byte[]> ReadByteArray(CancellationToken cancellationToken)
        {
            int count = await ReadInt32(cancellationToken);
            byte[] value = new byte[count];
            await readStream.ReadAsync(value, cancellationToken);
            return value;
        }

        public async ValueTask<sbyte[]> ReadSByteArray(CancellationToken cancellationToken)
        {
            int count = await ReadInt32(cancellationToken);
            sbyte[] value = new sbyte[count];
            for (int i = 0; i < count; i++)
            {
                value[i] = await ReadSByte(cancellationToken);
            }

            return value;
        }

        public async ValueTask<short[]> ReadInt16Array(CancellationToken cancellationToken)
        {
            int count = await ReadInt32(cancellationToken);
            short[] value = new short[count];
            for (int i = 0; i < count; i++)
            {
                value[i] = await ReadInt16(cancellationToken);
            }

            return value;
        }

        public async ValueTask<int[]> ReadInt32Array(CancellationToken cancellationToken)
        {
            int count = await ReadInt32(cancellationToken);
            int[] value = new int[count];
            for (int i = 0; i < count; i++)
            {
                value[i] = await ReadInt32(cancellationToken);
            }

            return value;
        }

        public async ValueTask<long[]> ReadInt64Array(CancellationToken cancellationToken)
        {
            int count = await ReadInt32(cancellationToken);
            long[] value = new long[count];
            for (int i = 0; i < count; i++)
            {
                value[i] = await ReadInt64(cancellationToken);
            }

            return value;
        }

        public async ValueTask<ushort[]> ReadUInt16Array(CancellationToken cancellationToken)
        {
            int count = await ReadInt32(cancellationToken);
            ushort[] value = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                value[i] = await ReadUInt16(cancellationToken);
            }

            return value;
        }

        public async ValueTask<uint[]> ReadUInt32Array(CancellationToken cancellationToken)
        {
            int count = await ReadInt32(cancellationToken);
            uint[] value = new uint[count];
            for (int i = 0; i < count; i++)
            {
                value[i] = await ReadUInt32(cancellationToken);
            }

            return value;
        }

        public async ValueTask<ulong[]> ReadUInt64Array(CancellationToken cancellationToken)
        {
            int count = await ReadInt32(cancellationToken);
            ulong[] value = new ulong[count];
            for (int i = 0; i < count; i++)
            {
                value[i] = await ReadUInt64(cancellationToken);
            }

            return value;
        }

        public async ValueTask<float[]> ReadSingleArray(CancellationToken cancellationToken)
        {
            int count = await ReadInt32(cancellationToken);
            float[] value = new float[count];
            for (int i = 0; i < count; i++)
            {
                value[i] = await ReadSingle(cancellationToken);
            }

            return value;
        }

        public async ValueTask<double[]> ReadDoubleArray(CancellationToken cancellationToken)
        {
            int count = await ReadInt32(cancellationToken);
            double[] value = new double[count];
            for (int i = 0; i < count; i++)
            {
                value[i] = await ReadDouble(cancellationToken);
            }

            return value;
        }

        public async ValueTask<Guid[]> ReadGuidArray(CancellationToken cancellationToken)
        {
            int count = await ReadInt32(cancellationToken);
            Guid[] value = new Guid[count];
            for (int i = 0; i < count; i++)
            {
                value[i] = await ReadGuid(cancellationToken);
            }

            return value;
        }

        public async ValueTask<Enumeration[]> ReadEnumArray<Enumeration>(CancellationToken cancellationToken)
            where Enumeration : Enum
        {
            int count = await ReadInt32(cancellationToken);
            Enumeration[] value = new Enumeration[count];
            for (int i = 0; i < count; i++)
            {
                value[i] = await ReadEnum<Enumeration>(cancellationToken);
            }

            return value;
        }

        private byte ReadByte() => buffer.Span[0];

        private unsafe float ReadSingle()
        {
            Span<byte> span = buffer.Span;
            uint value = (uint)(span[0] | span[1] << 8 | span[2] << 16 | span[3] << 24);
            return *((float*)&value);
        }

        private unsafe double ReadDouble()
        {
            Span<byte> span = buffer.Span;
            uint low = (uint)(span[0] | span[1] << 8 | span[2] << 16 | span[3] << 24);
            uint high = (uint)(span[4] | span[5] << 8 | span[6] << 16 | span[7] << 24);
            ulong value = (ulong)high << 32 | low;
            return *((double*)&value);
        }

        //private async ValueTask<int> ReadEncodedInt32(CancellationToken cancellationToken)
        //{
        //    int value = 0;
        //    int shift = 0;
        //    byte current;
        //    do
        //    {
        //        current = await ReadByte(cancellationToken);
        //        value |= (current & sbyte.MaxValue) << shift;
        //        shift += 7;
        //    } while ((current & 128) > 0);
        //    return value;
        //}
    }
}