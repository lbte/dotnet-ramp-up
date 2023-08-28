using PRFTLatam.OrdersData.Infrastructure.Models;

namespace PRFTLatam.OrdersData.Services.IServices;

public interface IProductService 
{
    /// <summary>
    /// Finds all products
    /// </summary>
    /// <returns>A <see cref="List"/> of <see cref="Product"/></returns>
    Task <IEnumerable<Product>> GetProductsAsync();
    
    /// <summary>
    /// Finds a Product by its id in the DB
    /// </summary>
    /// <param name="id">The unique id of the product</param>
    /// <returns>A <see cref="Product"/></returns>
    Task <Product> GetProductById(int id);
    
    // /// <summary>
    // /// Creates a new <see cref="Product"/> entity in the DB
    // /// </summary>
    // /// <param name="product">A new product entity</param>
    // /// <returns>The created product with an assigned Id</returns>
    // Task <IEnumerable<Product>> CreateProduct(Product product);
}