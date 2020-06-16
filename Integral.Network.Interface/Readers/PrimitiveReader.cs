using System;
using System.Threading;
using System.Threading.Tasks;

namespace Integral.Readers
{
    public interface PrimitiveReader
    {
        ValueTask<string> ReadString(CancellationToken cancellationToken);

        ValueTask<bool> ReadBool(CancellationToken cancellationToken);

        ValueTask<byte> ReadByte(CancellationToken cancellationToken);

        ValueTask<sbyte> ReadSByte(CancellationToken cancellationToken);

        ValueTask<short> ReadInt16(CancellationToken cancellationToken);

        ValueTask<int> ReadInt32(CancellationToken cancellationToken);

        ValueTask<long> ReadInt64(CancellationToken cancellationToken);

        ValueTask<ushort> ReadUInt16(CancellationToken cancellationToken);

        ValueTask<uint> ReadUInt32(CancellationToken cancellationToken);

        ValueTask<ulong> ReadUInt64(CancellationToken cancellationToken);

        ValueTask<float> ReadSingle(CancellationToken cancellationToken);

        ValueTask<double> ReadDouble(CancellationToken cancellationToken);

        ValueTask<Guid> ReadGuid(CancellationToken cancellationToken);

        ValueTask<Enumeration> ReadEnum<Enumeration>(CancellationToken cancellationToken)
            where Enumeration : Enum;

        ValueTask<string[]> ReadStringArray(CancellationToken cancellationToken);

        ValueTask<bool[]> ReadBoolArray(CancellationToken cancellationToken);

        ValueTask<byte[]> ReadByteArray(CancellationToken cancellationToken);

        ValueTask<sbyte[]> ReadSByteArray(CancellationToken cancellationToken);

        ValueTask<short[]> ReadInt16Array(CancellationToken cancellationToken);

        ValueTask<int[]> ReadInt32Array(CancellationToken cancellationToken);

        ValueTask<long[]> ReadInt64Array(CancellationToken cancellationToken);

        ValueTask<ushort[]> ReadUInt16Array(CancellationToken cancellationToken);

        ValueTask<uint[]> ReadUInt32Array(CancellationToken cancellationToken);

        ValueTask<ulong[]> ReadUInt64Array(CancellationToken cancellationToken);

        ValueTask<float[]> ReadSingleArray(CancellationToken cancellationToken);

        ValueTask<double[]> ReadDoubleArray(CancellationToken cancellationToken);

        ValueTask<Guid[]> ReadGuidArray(CancellationToken cancellationToken);

        ValueTask<Enumeration[]> ReadEnumArray<Enumeration>(CancellationToken cancellationToken)
            where Enumeration : Enum;
    }
}