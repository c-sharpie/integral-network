using System.Threading.Tasks;
using Integral.Abstractions;
using Integral.Channels;

namespace Integral.Sessions
{
    public interface Session : Channel, Executable<Task>
    {
    }
}