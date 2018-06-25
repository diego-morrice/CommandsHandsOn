using System.Threading.Tasks;

namespace Command.Application.Interfaces
{
    public interface IOrderCommand
    {
        bool IsPrepared { get; set; }
        Task<bool> Prepare();
    }
}
