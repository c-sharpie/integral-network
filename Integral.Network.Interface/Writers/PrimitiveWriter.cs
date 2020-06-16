using System;
using System.Threading;
using System.Threading.Tasks;

namespace Integral.Writers
{
    public interface PrimitiveWriter
    {
        ValueTask Write(string value, CancellationToken cancellationToken);

        ValueTask Write(bool value, CancellationToken cancellationToken);

        ValueTask Write(byte value, CancellationToken cancellationToken);

        ValueTask Write(sbyte value, CancellationToken cancellationToken);

        ValueTask Write(short value, CancellationToken cancellationToken);

        ValueTask Write(int value, CancellationToken cancellationToken);

        ValueTask Write(long value, CancellationToken cancellationToken);

        ValueTask Write(ushort value, CancellationToken cancellationToken);

        ValueTask Write(uint value, CancellationToken cancellationToken);

        ValueTask Write(ulong value, CancellationToken cancellationToken);

        ValueTask Write(float value, CancellationToken cancellationToken);

        ValueTask Write(double value, CancellationToken cancellationToken);

        ValueTask Write(Guid value, CancellationToken cancellationToken);

        ValueTask Write<Enumeration>(Enumeration value, CancellationToken cancellationToken)
            where Enumeration : Enum;

        ValueTask Write(string[] value, CancellationToken cancellationToken);

        ValueTask Write(bool[] value, CancellationToken cancellationToken);

        ValueTask Write(byte[] value, CancellationToken cancellationToken);

        ValueTask Write(sbyte[] value, CancellationToken cancellationToken);

        ValueTask Write(short[] value, CancellationToken cancellationToken);

        ValueTask Write(int[] value, CancellationToken cancellationToken);

        ValueTask Write(long[] value, CancellationToken cancellationToken);

        ValueTask Write(ushort[] value, CancellationToken cancellationToken);

        ValueTask Write(uint[] value, CancellationToken cancellationToken);

        ValueTask Write(ulong[] value, CancellationToken cancellationToken);

        ValueTask Write(float[] value, CancellationToken cancellationToken);

        ValueTask Write(double[] value, CancellationToken cancellationToken);

        ValueTask Write(Guid[] value, CancellationToken cancellationToken);

        ValueTask Write<Enumeration>(Enumeration[] value, CancellationToken cancellationToken)
            where Enumeration : Enum;
    }
}