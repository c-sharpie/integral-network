using Integral.Readers;

namespace Integral.Abstractions
{
    public interface Deserializable : Readable<PrimitiveReader>
    {
    }

    public interface Deserializable<out Identifier> : Deserializable, Identifiable<Identifier>
        where Identifier : notnull
    {
    }
}