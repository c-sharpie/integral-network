using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Integral.Readers;
using Integral.Writers;

namespace Integral.Packets
{
    internal sealed class PingPacket : TestPacket
    {
        private static readonly ConcurrentBag<int> Pings = new ConcurrentBag<int>();

        private readonly Stopwatch stopwatch = new Stopwatch();

        private readonly byte[] data;

        internal PingPacket(int size, int iterations)
            : base(iterations) => data = new byte[size];

        internal static double Ping => Pings.DefaultIfEmpty().Average();

        public async override ValueTask Read(PrimitiveReader primitiveReader, CancellationToken cancellationToken)
        {
            Reads++;
            Pings.Add((int)stopwatch.ElapsedMilliseconds);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = await primitiveReader.ReadByte(cancellationToken);
            }

            stopwatch.Reset();
        }

        public async override ValueTask Write(PrimitiveWriter primitiveWriter, CancellationToken cancellationToken)
        {
            Writes++;
            for (int i = 0; i < data.Length; i++)
            {
                await primitiveWriter.Write(data[i], cancellationToken);
            }

            stopwatch.Start();
        }
    }
}