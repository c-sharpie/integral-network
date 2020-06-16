using Integral.Abstractions;
using Integral.Connectors;

namespace Integral.Listeners
{
    public interface Listener : Connector, Disposable
    {
    }
}