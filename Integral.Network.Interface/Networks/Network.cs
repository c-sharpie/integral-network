using System.Threading.Tasks;
using Integral.Abstractions;

namespace Integral.Networks
{
    public interface Network : Initializable<Task>, Executable<Task>
    {
    }
}