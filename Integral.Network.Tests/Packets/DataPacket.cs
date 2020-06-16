using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Integral.Readers;
using Integral.Writers;

namespace Integral.Packets
{
    internal class DataPacket : Packet
    {
        private static readonly Random Random = new Random();

        private readonly int iterations;

        private int serializationCounter, deserializationCounter;

        internal DataPacket(int iterations, bool randomize = true)
        {
            this.iterations = iterations;
            if (randomize)
            {
                BoolValue = Random.Next() % 2 == 0;
                ByteValue = (byte)(Random.Next() % byte.MaxValue);
                SByteValue = (sbyte)(Random.Next() % byte.MaxValue - sbyte.MaxValue);
                ShortValue = (short)(Random.Next() % short.MaxValue);
                UShortValue = (ushort)(Random.Next() % ushort.MaxValue);
                IntValue = Random.Next();
                UIntValue = (uint)(Random.Next() * 2);
                LongValue = Random.Next() * 2;
                ULongValue = (ulong)(Random.Next() * 2 - int.MaxValue);
                FloatValue = (float)Random.NextDouble();
                DoubleValue = Random.NextDouble();
                StringValue = new string("Test".OrderBy(s => Random.Next(2) % 2 == 0).ToArray());
            }
            else
            {
                StringValue = string.Empty;
            }
        }

        internal bool BoolValue { get; set; }

        internal byte ByteValue { get; set; }

        internal sbyte SByteValue { get; set; }

        internal short ShortValue { get; set; }

        internal ushort UShortValue { get; set; }

        internal int IntValue { get; set; }

        internal uint UIntValue { get; set; }

        internal long LongValue { get; set; }

        internal ulong ULongValue { get; set; }

        internal float FloatValue { get; set; }

        internal double DoubleValue { get; set; }

        internal string StringValue { get; set; }

        internal bool Complete => !ShouldSerialize && !ShouldDeserialize;

        internal bool ShouldSerialize => serializationCounter < iterations;

        internal bool ShouldDeserialize => deserializationCounter < iterations;

        internal static bool Valid(DataPacket[][] dataMessages) => dataMessages[0].SequenceEqual(dataMessages[1]);

        internal static DataPacket[][] CreatePrimitivePackets(int connections, int iterations)
        {
            DataPacket[] sendMessages = new DataPacket[connections];
            DataPacket[] receiveMessages = new DataPacket[connections];
            for (int i = 0; i < connections; i++)
            {
                sendMessages[i] = new DataPacket(iterations);
                receiveMessages[i] = new DataPacket(iterations, false);
            }

            return new[] { sendMessages, receiveMessages };
        }

        public async ValueTask Read(PrimitiveReader primitiveReader, CancellationToken cancellationToken)
        {
            BoolValue = await primitiveReader.ReadBool(cancellationToken);
            ByteValue = await primitiveReader.ReadByte(cancellationToken);
            SByteValue = await primitiveReader.ReadSByte(cancellationToken);
            ShortValue = await primitiveReader.ReadInt16(cancellationToken);
            UShortValue = await primitiveReader.ReadUInt16(cancellationToken);
            IntValue = await primitiveReader.ReadInt32(cancellationToken);
            UIntValue = await primitiveReader.ReadUInt32(cancellationToken);
            LongValue = await primitiveReader.ReadInt64(cancellationToken);
            ULongValue = await primitiveReader.ReadUInt64(cancellationToken);
            FloatValue = await primitiveReader.ReadSingle(cancellationToken);
            DoubleValue = await primitiveReader.ReadDouble(cancellationToken);
            StringValue = await primitiveReader.ReadString(cancellationToken);
            deserializationCounter++;
        }

        public async ValueTask Write(PrimitiveWriter primitiveWriter, CancellationToken cancellationToken)
        {
            await primitiveWriter.Write(BoolValue, cancellationToken);
            await primitiveWriter.Write(ByteValue, cancellationToken);
            await primitiveWriter.Write(SByteValue, cancellationToken);
            await primitiveWriter.Write(ShortValue, cancellationToken);
            await primitiveWriter.Write(UShortValue, cancellationToken);
            await primitiveWriter.Write(IntValue, cancellationToken);
            await primitiveWriter.Write(UIntValue, cancellationToken);
            await primitiveWriter.Write(LongValue, cancellationToken);
            await primitiveWriter.Write(ULongValue, cancellationToken);
            await primitiveWriter.Write(FloatValue, cancellationToken);
            await primitiveWriter.Write(DoubleValue, cancellationToken);
            await primitiveWriter.Write(StringValue, cancellationToken);
            serializationCounter++;
        }

        public override bool Equals(object? other) => other is DataPacket primitivePacket
                                                     && BoolValue == primitivePacket.BoolValue
                                                     && ByteValue == primitivePacket.ByteValue
                                                     && SByteValue == primitivePacket.SByteValue
                                                     && ShortValue == primitivePacket.ShortValue
                                                     && UShortValue == primitivePacket.UShortValue
                                                     && IntValue == primitivePacket.IntValue
                                                     && UIntValue == primitivePacket.UIntValue
                                                     && LongValue == primitivePacket.LongValue
                                                     && ULongValue == primitivePacket.ULongValue
                                                     && FloatValue == primitivePacket.FloatValue
                                                     && DoubleValue == primitivePacket.DoubleValue
                                                     && StringValue == primitivePacket.StringValue;

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 12;
                hashCode = (hashCode * 397) ^ BoolValue.GetHashCode();
                hashCode = (hashCode * 397) ^ ByteValue.GetHashCode();
                hashCode = (hashCode * 397) ^ SByteValue.GetHashCode();
                hashCode = (hashCode * 397) ^ ShortValue.GetHashCode();
                hashCode = (hashCode * 397) ^ UShortValue.GetHashCode();
                hashCode = (hashCode * 397) ^ IntValue;
                hashCode = (hashCode * 397) ^ (int)UIntValue;
                hashCode = (hashCode * 397) ^ LongValue.GetHashCode();
                hashCode = (hashCode * 397) ^ ULongValue.GetHashCode();
                hashCode = (hashCode * 397) ^ FloatValue.GetHashCode();
                hashCode = (hashCode * 397) ^ DoubleValue.GetHashCode();
                hashCode = (hashCode * 397) ^ StringValue.GetHashCode();
                return hashCode;
            }
        }
    }
}