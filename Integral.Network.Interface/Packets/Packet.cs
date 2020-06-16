using Integral.Abstractions;

namespace Integral.Packets
{
    public interface Packet : Serializable, Deserializable
    {
    }

    public interface Packet<out Identifier> : Packet, Serializable<Identifier>, Deserializable<Identifier>
        where Identifier : notnull
    {
    }
}