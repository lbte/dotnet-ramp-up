using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PRFTLatam.OrdersData.Infrastructure.Models;

namespace PRFTLatam.OrdersData.Infrastructure;

public class Repository<TId, TEntity> : IRepository<TId, TEntity>
where TId : struct
where TEntity : BaseEntity<TId>
{
    internal OrdersDataContext _context;
    internal DbSet<TEntity> _dbSet;
    public Repository(OrdersDataContext context)
    {
        _context = context;
        // creation of the set for the specific TEntity type
        _dbSet = context.Set<TEntity>();
    }
    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task Delete(TEntity entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
    }

    public async Task Delete(TId id)
    {
        TEntity entityToDelete = await _dbSet.FindAsync(id);
        Delete(entityToDelete);
    }

    public async Task<TEntity> FindAsync(TId id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> filter = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
        string includeProperties = "")
    {
        IQueryable<TEntity> query = _dbSet;

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

    public async Task Update(TEntity entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }
}