using PRFTLatam.OrdersData.Infrastructure.Models;
using PRFTLatam.OrdersData.Services.IServices;

namespace PRFTLatam.OrdersData.Services.Services;

public class ClientService : IClientService
{
    private readonly IUnitOfWork _unitOfWork;
    // private readonly IClientRepository _clientRepository;

    public ClientService(IUnitOfWork unitOfWork) // , IClientRepository clientRepository
    {
        _unitOfWork = unitOfWork;
        // _clientRepository = clientRepository;
    }

    /// <summary>
    /// Creates a new <see cref="Client"/> entity in the DB
    /// </summary>
    /// <param name="client">A new client entity</param>
    /// <returns>The created client with an assigned Id</returns>
    public async Task<Client> CreateClient(Client client)
    {
        await _unitOfWork.ClientRepository.AddAsync(client);
        await _unitOfWork.SaveAsync();
        return client;    
    }

    /// <summary>
    /// Finds a Client by its id in the DB
    /// </summary>
    /// <param name="id">The unique id of the client</param>
    /// <returns>A <see cref="Client"/></returns>
    public async Task<Client> GetClientById(string id)
    {
        return await _unitOfWork.ClientRepository.FindAsync(id);
    }

    /// <summary>
    /// Finds all clients
    /// </summary>
    /// <returns>A <see cref="List"/> of <see cref="Client"/></returns>
    public async Task<IEnumerable<Client>> GetClientsAsync()
    {
        return await _unitOfWork.ClientRepository.GetAllAsync(null, null, "Orders,Orders.Product");
        // return await _clientRepository.GetClientsWithoutOrders(x => !x.Orders.Any());
    }
    
    /// <summary>
    /// Finds all clients
    /// </summary>
    /// <returns>A <see cref="List"/> of <see cref="Client"/></returns>
    public async Task<IEnumerable<Client>> GetClientsWithoutOrders()
    {
        return await _unitOfWork.ClientRepository.GetAllAsync(x => !x.Orders.Any(), null, "Orders");
    }
}