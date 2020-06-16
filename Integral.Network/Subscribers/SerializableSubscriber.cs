using System;
using System.Threading;
using System.Threading.Tasks;
using Integral.Abstractions;
using Integral.Aggregates;
using Integral.Collections;
using Integral.Writers;

namespace Integral.Subscribers
{
    internal sealed class SerializableSubscriber<Data> : Subscriber<Action<Data>>, Serializable
        where Data : Serializable
    {
        private readonly IndirectAggregate<Data> cache;

        private readonly IndirectIncreasingAggregate<Serializable> parent;

        private readonly ConcurrentQueuedCollection<Data> queue = new ConcurrentQueuedCollection<Data>();

        public SerializableSubscriber(IndirectAggregate<Data> cache, IndirectIncreasingAggregate<Serializable> parent)
        {
            this.cache = cache;
            this.parent = parent;
        }

        public async ValueTask Write(PrimitiveWriter primitiveWriter, CancellationToken cancellationToken)
        {
            queue.Remove(out Data data);
            await data.Write(primitiveWriter, cancellationToken);
            cache.Add(data);
        }

        public void OnPublished(Action<Data> action)
        {
            cache.Remove(out Data data);
            action(data);
            queue.Add(data);
            parent.Add(this);
        }
    }
}