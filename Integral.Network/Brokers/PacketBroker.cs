using System;
using Integral.Abstractions;
using Integral.Aggregates;
using Integral.Collections;
using Integral.Factories;
using Integral.Functions;
using Integral.Packets;
using Integral.Publishers;
using Integral.Registries;
using Integral.Subscribers;

namespace Integral.Brokers
{
    internal abstract class PacketBroker : ExecutablePacket, Broker<Serializable<byte>, Deserializable<byte>>
    {
        private readonly BoxedCollection boxedCollection = new BoxedCollection();

        public void Register<Publishable>(Publisher<Action<Publishable>> publisher)
            where Publishable : Serializable<byte> => TryAdd(CreatePublisherRegistry<Publishable>).Register(publisher);

        public void Register<Subscribable>(Subscriber<Subscribable> subscriber)
            where Subscribable : Deserializable<byte> => TryAdd(CreateSubscriberRegistry<Subscribable>).Register(subscriber);

        public void Unregister<Publishable>(Publisher<Action<Publishable>> publisher)
            where Publishable : Serializable<byte> => TryRemove(publisher);

        public void Unregister<Subscribable>(Subscriber<Subscribable> subscriber)
            where Subscribable : Deserializable<byte> => TryRemove(subscriber);

        private Registry<Publisher<Action<Publishable>>> CreatePublisherRegistry<Publishable>()
            where Publishable : Serializable<byte> => new PublisherRegistry<Action<Publishable>>(CreateSubscriber(TryAdd(CreateCache<Publishable>)));

        private Registry<Subscriber<Subscribable>> CreateSubscriberRegistry<Subscribable>()
            where Subscribable : Deserializable<byte> => new SubscriberRegistry<Subscribable>(CreatePublisher(TryAdd(CreateCache<Subscribable>)));

        private IndirectAggregate<Cacheable> CreateCache<Cacheable>()
            where Cacheable : notnull => new GeneratedCollection<Cacheable>(new QueuedCollection<Cacheable>(), new ActivatorFactory<Cacheable>());

        private void TryRemove<Registrant>(Registrant registrant)
            where Registrant : notnull
        {
            if (boxedCollection.Peek(out Registry<Registrant> registry))
            {
                registry.Unregister(registrant);
            }
        }

        private Registrant TryAdd<Registrant>(Function<Registrant> function)
            where Registrant : notnull
        {
            if (!boxedCollection.Peek(out Registrant registrant))
            {
                registrant = function();
                boxedCollection.Add(registrant);
            }

            return registrant;
        }
    }
}