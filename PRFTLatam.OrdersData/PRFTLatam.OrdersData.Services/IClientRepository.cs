using System.Linq.Expressions;
using PRFTLatam.OrdersData.Infrastructure.Models;

namespace PRFTLatam.OrdersData.Services;

public interface IClientRepository : IRepository<Client>
{
    Task<IEnumerable<Client>> GetClientsWithoutOrders(
        Expression<Func<Client, bool>> filter = null, 
        Func<IQueryable<Client>, IOrderedQueryable<Client>> orderBy = null, 
        string includeProperties = "Orders");
}