using Integral.Publishers;
using Integral.Subscribers;

namespace Integral.Registries
{
    internal sealed class SubscriberRegistry<Subscribable> : Registry<Subscriber<Subscribable>>
        where Subscribable : notnull
    {
        private readonly Publisher<Subscribable> publisher;

        internal SubscriberRegistry(Publisher<Subscribable> publisher) => this.publisher = publisher;

        public void Register(Subscriber<Subscribable> subscriber) => publisher.OnPublish += subscriber.OnPublished;

        public void Unregister(Subscriber<Subscribable> subscriber) => publisher.OnPublish -= subscriber.OnPublished;
    }
}