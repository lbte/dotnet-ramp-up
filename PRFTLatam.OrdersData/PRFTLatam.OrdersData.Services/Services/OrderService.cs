using PRFTLatam.OrdersData.Infrastructure;
using PRFTLatam.OrdersData.Infrastructure.Models;
using PRFTLatam.OrdersData.Services.IServices;

namespace PRFTLatam.OrdersData.Services.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Finds a Order by its id in the DB
    /// </summary>
    /// <param name="id">The unique id of the order</param>
    /// <returns>A <see cref="Order"/></returns>
    public async Task<Order> GetOrderById(int id)
    {
        return await _unitOfWork.OrderRepository.FindAsync(id);
    }

    /// <summary>
    /// Finds all orders
    /// </summary>
    /// <returns>A <see cref="List"/> of <see cref="Order"/></returns>
    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        return await _unitOfWork.OrderRepository.GetAllAsync();
    }

    /// <summary>
    /// Finds all orders from <see cref="Client.Name"/>
    /// </summary>
    /// <param name="id">The unique id of the client</param>
    /// <returns>A <see cref="List"/> of <see cref="Order"/></returns>
    public async Task<IEnumerable<Order>> GetOrdersByClient(int id)
    {
        return await _unitOfWork.OrderRepository.GetAllAsync(x => x.Client.Id.Equals(id), x => x.OrderBy(x => x.Id), new Client().GetType().Name);
    }
}