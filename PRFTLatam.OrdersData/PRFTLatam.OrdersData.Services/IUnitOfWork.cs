using PRFTLatam.OrdersData.Infrastructure.Models;


namespace PRFTLatam.OrdersData.Services;

public interface IUnitOfWork
{
    IRepository<Client> ClientRepository{ get; }
    IRepository<Product> ProductRepository{ get; }
    IRepository<Order> OrderRepository{ get; }

    Task SaveAsync();
}