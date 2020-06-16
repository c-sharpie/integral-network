using System.Threading.Tasks;
using Integral.Abstractions;
using Integral.Transporters;

namespace Integral.Connectors
{
    public interface Connector : Executable<Task<Transporter>>
    {
    }
}