using PRFTLatam.OrdersData.Infrastructure.Models;
using PRFTLatam.OrdersData.Services.IServices;
using PRFTLatam.OrdersData.Services;
using System.Diagnostics;

namespace PRFTLatam.OrdersData.Services.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Order> CreateOrder(Order order)
    {
        try
        {
            // find the client and product to see if they exist
            var client = await _unitOfWork.ClientRepository.GetAllAsync(x => x.Id.Equals(order.ClientId), null, "Orders");
            var product = await _unitOfWork.ProductRepository.FindAsync(order.ProductId);
            if(client.ElementAt(0) == null && product == null)
            {
                return null;
            }
            else
            {
                var todaysOrders = await _unitOfWork.OrderRepository.GetAllAsync(x => x.Date.Date == DateTime.Now.Date, null, "Product");
                // If there are more than 10 orders for a given date (same day), the order cannot be registered.
                if (todaysOrders.Count() <= 10)
                {
                    // calculate the price of the order
                    order.Price = product.Price * order.RequiredQuantity;

                    // update the values that change on the client with the new order
                    client.ElementAt(0).Quota = client.ElementAt(0).Quota - order.Price;
                    client.ElementAt(0).OrdersTotal = client.ElementAt(0).OrdersTotal + order.Price;

                    await _unitOfWork.OrderRepository.AddAsync(order);
                    await _unitOfWork.SaveAsync();
                }
            }
        } 
        catch (Exception ex)
        {
            order = null;
        }
        return order;
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
}