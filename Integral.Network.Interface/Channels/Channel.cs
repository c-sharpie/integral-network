using Integral.Abstractions;
using Integral.Brokers;

namespace Integral.Channels
{
    public interface Channel : Broker<Serializable<byte>, Deserializable<byte>>, Disposable
    {
    }
}