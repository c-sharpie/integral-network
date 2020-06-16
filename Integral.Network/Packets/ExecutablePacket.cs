using System;
using System.Threading;
using System.Threading.Tasks;
using Integral.Abstractions;
using Integral.Aggregates;
using Integral.Collections;
using Integral.Publishers;
using Integral.Readers;
using Integral.Subscribers;
using Integral.Writers;

namespace Integral.Packets
{
    internal abstract class ExecutablePacket : Executable, Packet
    {
        private readonly ConcurrentQueuedCollection<Serializable> serializables = new ConcurrentQueuedCollection<Serializable>();

        private readonly ConcurrentQueuedCollection<Executable> deserializables = new ConcurrentQueuedCollection<Executable>();

        private readonly IndexedCollection<byte, Deserializable> index = new IndexedCollection<byte, Deserializable>();

        public async ValueTask Write(PrimitiveWriter primitiveWriter, CancellationToken cancellationToken)
        {
            while (serializables.Remove(out Serializable serializable))
            {
                await serializable.Write(primitiveWriter, cancellationToken);
            }
        }

        public async ValueTask Read(PrimitiveReader primitiveReader, CancellationToken cancellationToken)
        {
            byte key = await primitiveReader.ReadByte(cancellationToken);
            await index[key].Read(primitiveReader, cancellationToken);
        }

        public void Execute()
        {
            while (deserializables.Remove(out Executable executable))
            {
                executable.Execute();
            }
        }

        protected Publisher<Publishable> CreatePublisher<Publishable>(IndirectAggregate<Publishable> indirectAggregate)
            where Publishable : Deserializable<byte>
        {
            DeserializablePublisher<Publishable> deserializablePublisher = new DeserializablePublisher<Publishable>(indirectAggregate, deserializables);
            indirectAggregate.Remove(out Publishable publishable);
            index.Add(publishable.Identity, deserializablePublisher);
            indirectAggregate.Add(publishable);
            return deserializablePublisher;
        }

        protected Subscriber<Action<Subscribable>> CreateSubscriber<Subscribable>(IndirectAggregate<Subscribable> indirectAggregate)
            where Subscribable : Serializable<byte> => new SerializableSubscriber<Subscribable>(indirectAggregate, serializables);
    }
}
