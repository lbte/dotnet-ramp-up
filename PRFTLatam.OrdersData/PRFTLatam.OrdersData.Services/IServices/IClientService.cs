using PRFTLatam.OrdersData.Infrastructure.Models;

namespace PRFTLatam.OrdersData.Services.IServices;

public interface IClientService 
{
    /// <summary>
    /// Finds all clients
    /// </summary>
    /// <returns>A <see cref="List"/> of <see cref="Client"/></returns>
    Task <IEnumerable<Client>> GetClientsAsync();
    
    /// <summary>
    /// Finds a Client by its id in the DB
    /// </summary>
    /// <param name="id">The unique id of the client</param>
    /// <returns>A <see cref="Client"/></returns>
    Task <Client> GetClientById(string id);
    
    /// <summary>
    /// Creates a new <see cref="Client"/> entity in the DB
    /// </summary>
    /// <param name="client">A new client entity</param>
    /// <returns>The created client with an assigned Id</returns>
    Task <Client> CreateClient(Client client);


    Task<IEnumerable<Client>> GetClientsWithoutOrders();

    Task<IEnumerable<Client>> GetClientsOrdersTotal();
}