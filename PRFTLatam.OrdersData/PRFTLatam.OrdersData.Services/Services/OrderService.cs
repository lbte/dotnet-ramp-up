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
        return await _unitOfWork.OrderRepository.GetAllAsync(null, null, new Product().GetType().Name);
    }

    /// <summary>
    /// Finds all orders from <see cref="Client.Name"/>
    /// </summary>
    /// <param name="id">The unique id of the client</param>
    /// <returns>A <see cref="List"/> of <see cref="Order"/></returns>
    public async Task<IEnumerable<Order>> GetOrdersByClient(string id)
    {
        return await _unitOfWork.OrderRepository.GetAllAsync(x => x.ClientId.Equals(id), null, "Product");
    }


    /*
    SELECT *
    FROM order
    LEFT JOIN client ON order.client_id = client.id
    WHERE order.client_id IS NULL;
    */

    // https://stackoverflow.com/questions/525194/linq-inner-join-vs-left-join
    // https://stackoverflow.com/questions/48646568/sql-server-join-where-not-exist-on-other-table#:~:text=You%20can%20you%20use%20an%20intelligent%20left%20join,non%20matching%20rows%20then%20use%20LEFT%20JOIN%20instead
}