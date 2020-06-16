using Integral.Publishers;
using Integral.Subscribers;

namespace Integral.Registries
{
    internal sealed class PublisherRegistry<Publishable> : Registry<Publisher<Publishable>>
        where Publishable : notnull
    {
        private readonly Subscriber<Publishable> subscriber;

        internal PublisherRegistry(Subscriber<Publishable> subscriber) => this.subscriber = subscriber;

        public void Register(Publisher<Publishable> publisher) => publisher.OnPublish += subscriber.OnPublished;

        public void Unregister(Publisher<Publishable> publisher) => publisher.OnPublish -= subscriber.OnPublished;
    }
}