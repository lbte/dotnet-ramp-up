using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace PRFTLatam.EmploymentInfo.Domain;

public interface IRepository<TEntity>
where TEntity : class
{
    Task AddAsync(TEntity entity);
    Task<TEntity> FindAsync(string email);
    Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "");
    Task Update(TEntity entity);
    Task Delete(TEntity entity);
    Task Delete(string email);
}