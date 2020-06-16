using Integral.Deserializers;
using Integral.Serializers;

namespace Integral.Messengers
{
    public interface Messenger : Serializer, Deserializer
    {
    }
}