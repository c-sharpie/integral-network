using System.Threading.Tasks;
using Integral.Abstractions;

namespace Integral.Hosts
{
    public interface Host : Initializable<Task>, Executable<Task>
    {
    }
}