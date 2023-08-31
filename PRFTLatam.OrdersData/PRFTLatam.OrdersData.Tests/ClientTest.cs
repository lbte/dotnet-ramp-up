using PRFTLatam.OrdersData.Services;
using PRFTLatam.OrdersData.Services.IServices;
using PRFTLatam.OrdersData.Infrastructure.Models;
using NSubstitute;
using PRFTLatam.OrdersData.Services.Services;
using System.Collections.ObjectModel;
using Azure.Core;

namespace PRFTLatam.OrdersData.Tests;

public class ClientTest
{
    private readonly IRepository<Client> _clientRepository;
    private readonly IRepository<Order> _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClientService _clientService;
    private readonly IOrderService _orderService;
    private readonly Client _newClient;
    private readonly Order _newOrder;

    public ClientTest()
    {
        _clientRepository = Substitute.For<IRepository<Client>>();
        _orderRepository = Substitute.For<IRepository<Order>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _clientService = new ClientService(_unitOfWork);
        _orderService = new OrderService(_unitOfWork);
        _newOrder = new Order(){
            Id = 60,
            ClientId = "50",
            ProductId = 400,
            RequiredQuantity = 2,
            Price = 50000
        };

        _newClient = new Client(){
            Id = "50",
            Name = "Carlos",
            Quota = 500000,
            Orders = new Collection<Order> { _newOrder }
        };
    }

    [Fact]
    public void GetOrdersByClient()
    {
        // Arrange
        _unitOfWork.OrderRepository.Returns(_orderRepository);
        _unitOfWork.ClientRepository.Returns(_clientRepository);
        
        // Act
        var result = _orderService.GetOrdersByClient(_newClient.Id);
        
        // Assert
        // Status that appear when the task is completed: RanToCompletion
        Assert.Equal("RanToCompletion", result.Status.ToString());
    }

    [Fact]
    public async void CreateClientWithoutOrders()
    {
        // Arrange
        _clientRepository.AddAsync(_newClient).Returns(Task.FromResult(_newClient));
        _unitOfWork.ClientRepository.Returns(_clientRepository);

        // Act
        var newClient = await _clientService.CreateClient(_newClient);

        // Assert
        Assert.NotNull(newClient);
    }
}