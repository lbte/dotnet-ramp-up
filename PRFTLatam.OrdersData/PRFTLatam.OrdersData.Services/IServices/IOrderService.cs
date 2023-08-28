using PRFTLatam.OrdersData.Infrastructure.Models;

namespace PRFTLatam.OrdersData.Services.IServices;

public interface IOrderService 
{
    /// <summary>
    /// Finds all orders
    /// </summary>
    /// <returns>A <see cref="List"/> of <see cref="Order"/></returns>
    Task <IEnumerable<Order>> GetOrdersAsync();
    
    /// <summary>
    /// Finds a Order by its id in the DB
    /// </summary>
    /// <param name="id">The unique id of the order</param>
    /// <returns>A <see cref="Order"/></returns>
    Task <Order> GetOrderById(int id);

    /// <summary>
    /// Finds all orders from <see cref="Client.Name"/>
    /// </summary>
    /// <param name="id">The unique id of the client</param>
    /// <returns>A <see cref="List"/> of <see cref="Order"/></returns>
    Task <IEnumerable<Order>> GetOrdersByClient(int id);
    
    // /// <summary>
    // /// Creates a new <see cref="Order"/> entity in the DB
    // /// </summary>
    // /// <param name="order">A new order entity</param>
    // /// <returns>The created order with an assigned Id</returns>
    // Task <IEnumerable<Order>> CreateOrder(Order order);
}