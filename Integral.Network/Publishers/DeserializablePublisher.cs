using System.Threading;
using System.Threading.Tasks;
using Integral.Abstractions;
using Integral.Aggregates;
using Integral.Collections;
using Integral.Readers;

namespace Integral.Publishers
{
    internal sealed class DeserializablePublisher<Data> : GenericPublisher<Data>, Executable, Deserializable
        where Data : Deserializable
    {
        private readonly IndirectAggregate<Data> cache;

        private readonly IndirectIncreasingAggregate<Executable> parent;

        private readonly ConcurrentQueuedCollection<Data> queue = new ConcurrentQueuedCollection<Data>();

        internal DeserializablePublisher(IndirectAggregate<Data> cache, IndirectIncreasingAggregate<Executable> parent)
        {
            this.cache = cache;
            this.parent = parent;
        }

        public async ValueTask Read(PrimitiveReader primitiveReader, CancellationToken cancellationToken)
        {
            cache.Remove(out Data data);
            await data.Read(primitiveReader, cancellationToken);
            queue.Add(data);
            parent.Add(this);
        }

        public void Execute()
        {
            queue.Remove(out Data data);
            Publish(data);
            cache.Add(data);
        }
    }
}