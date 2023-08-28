using PRFTLatam.OrdersData.Infrastructure.Models;

namespace PRFTLatam.OrdersData.Infrastructure;

public interface IUnitOfWork
{
    IRepository<int, Client> ClientRepository{ get; }
    IRepository<int, Product> ProductRepository{ get; }
    IRepository<long, Order> OrderRepository{ get; }

    Task SaveAsync();
}