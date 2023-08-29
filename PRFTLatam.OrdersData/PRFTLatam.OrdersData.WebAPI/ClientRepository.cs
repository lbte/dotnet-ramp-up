using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PRFTLatam.OrdersData.Infrastructure.Models;
using PRFTLatam.OrdersData.Services;

namespace PRFTLatam.OrdersData.WebAPI;

public class ClientRepository : Repository<Client>, IClientRepository
{
    public ClientRepository(OrdersDataContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Client>> GetClientsWithoutOrders(
        Expression<Func<Client, bool>> filter = null, 
        Func<IQueryable<Client>, IOrderedQueryable<Client>> orderBy = null, 
        string includeProperties = "Orders")
    {
        IQueryable<Client> query = _dbSet;

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        foreach(var property in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(property); // Specifies the related objects to include in the query results
        }

        if (orderBy is not null)
        {
            return await orderBy(query).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
    }
}