using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository {get; }
        IOrderRepository OrderRepository {get; }
        IUserRepository UserRepository {get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}