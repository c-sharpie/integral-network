using Integral.Abstractions;
using Integral.Brokers;

namespace Integral.Dispatchers
{
    internal abstract class PacketDispatcher : PacketBroker, Dispatcher<Serializable<byte>, Deserializable<byte>>
    {
    }
}
