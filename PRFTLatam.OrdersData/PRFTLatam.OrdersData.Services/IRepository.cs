using System.Linq.Expressions;
using PRFTLatam.OrdersData.Infrastructure.Models;

namespace PRFTLatam.OrdersData.Services;

public interface IRepository<TEntity>
where TEntity : class
{
    /// <summary>
    /// Add an entity to the database acording to the TEntity type
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task AddAsync(TEntity entity);
    
    /// <summary>
    /// Find a specific entity according to its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The TEntity found by the id input parameter</returns>
    Task<TEntity> FindAsync(object id);
    
    /// <summary>
    /// Get all items in a table of the database for the specific TEntity type
    /// </summary>
    /// <param name="filter">Lambda expression based on the TEntity type, which will return a Boolean value. Ex: x => x.Client.Name.ToLower().Equals(client.ToLower())</param>
    /// <param name="orderBy">Lambda expression which will return an ordered version of the IQueryable object for the TEntity type. Ex: x => x.OrderBy(x => x.Id) / q => q.OrderBy(s => s.LastName)</param>
    /// <param name="includeProperties">Parameter that lets the caller provide a comma-delimited list of navigation properties for eager loading (you do everything when asked). Ex: new Client().GetType().Name</param>
    /// <returns>IEnumerable of the TEntity entities found</returns>
    Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "");
    
    /// <summary>
    /// Update an entity's values by passing in the entity
    /// </summary>
    /// <param name="entity">Entity with the updated values</param>
    /// <returns></returns>
    Task Update(TEntity entity);
    
    /// <summary>
    /// Delete an entity by passing in the entity
    /// </summary>
    /// <param name="entity">Entity to delete</param>
    /// <returns></returns>
    Task Delete(TEntity entity);
    
    /// <summary>
    /// Delete an entity by passing in the entity id
    /// </summary>
    /// <param name="id">id related to the TEntity that'll be deleted</param>
    /// <returns></returns>
    Task Delete(object id);
}