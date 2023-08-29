using PRFTLatam.OrdersData.Infrastructure;
using PRFTLatam.OrdersData.Infrastructure.Models;
using PRFTLatam.OrdersData.Services.IServices;

namespace PRFTLatam.OrdersData.Services.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Finds a Product by its id in the DB
    /// </summary>
    /// <param name="id">The unique id of the product</param>
    /// <returns>A <see cref="Product"/></returns>
    public async Task<Product> GetProductById(int id)
    {
        return await _unitOfWork.ProductRepository.FindAsync(id);
    }

    /// <summary>
    /// Finds all products
    /// </summary>
    /// <returns>A <see cref="List"/> of <see cref="Product"/></returns>
    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _unitOfWork.ProductRepository.GetAllAsync();
    }
}