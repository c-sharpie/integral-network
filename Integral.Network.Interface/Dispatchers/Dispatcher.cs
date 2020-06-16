using Integral.Abstractions;
using Integral.Brokers;

namespace Integral.Dispatchers
{
    public interface Dispatcher<in PublisherConstraint, in SubscriberConstraint> : Broker<PublisherConstraint, SubscriberConstraint>, Executable
        where PublisherConstraint : notnull
        where SubscriberConstraint : notnull
    {
    }
}