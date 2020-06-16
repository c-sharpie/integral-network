using Integral.Writers;

namespace Integral.Abstractions
{
    public interface Serializable : Writable<PrimitiveWriter>
    {
    }

    public interface Serializable<out Identifier> : Serializable, Identifiable<Identifier>
        where Identifier : notnull
    {
    }
}