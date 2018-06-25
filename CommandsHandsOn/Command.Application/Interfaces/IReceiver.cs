using System.Threading.Tasks;
using Command.Application.Enums;

namespace Command.Application.Interfaces
{
    public interface IReceiver
    {
        Task<bool> Cook(CookEnum action);
    }
}
