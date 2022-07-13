using Core.Messages.Commands;
using System.Threading.Tasks;

namespace Core.Persistence
{
    public interface IUnitOfWork
    {
        Task Save(CreateOrderCommand model);
    }
}
