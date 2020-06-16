using Integral.Dispatchers;

namespace Integral.Channels
{
    internal abstract class PacketChannel : PacketDispatcher, Channel
    {
        public abstract void Dispose();
    }
}
