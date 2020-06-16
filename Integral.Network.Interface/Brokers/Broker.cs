using System;
using Integral.Publishers;
using Integral.Subscribers;

namespace Integral.Brokers
{
    public interface Broker<in PublisherConstraint, in SubscriberConstraint>
        where PublisherConstraint : notnull
        where SubscriberConstraint : notnull
    {
        void Register<Publishable>(Publisher<Action<Publishable>> publisher)
            where Publishable : PublisherConstraint;

        void Register<Subscribable>(Subscriber<Subscribable> subscriber)
            where Subscribable : SubscriberConstraint;

        void Unregister<Publishable>(Publisher<Action<Publishable>> publisher)
            where Publishable : PublisherConstraint;

        void Unregister<Subscribable>(Subscriber<Subscribable> subscriber)
            where Subscribable : SubscriberConstraint;
    }
}