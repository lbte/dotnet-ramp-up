using PRFTLatam.OrdersData.Infrastructure;
using PRFTLatam.OrdersData.Infrastructure.Models;
using PRFTLatam.OrdersData.Services.IServices;

namespace PRFTLatam.OrdersData.Services.Services;

public class ClientService : IClientService
{
    private readonly IUnitOfWork _unitOfWork;

    public ClientService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public Task<IEnumerable<Client>> CreateClient(Client client)
    {
        throw new NotImplementedException();
    }

    public Task<Client> GetClientById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Client>> GetClientsAsync()
    {
        throw new NotImplementedException();
    }
}