using System.Net;
using Microsoft.AspNetCore.Mvc;
using PRFTLatam.OrdersData.Infrastructure.Models;
using PRFTLatam.OrdersData.Services.IServices;

namespace PRFTLatam.OrdersData.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet]
    [Route("GetAllClients")]
    public async Task<IActionResult> GetAllClients()
    {
        var clients = await _clientService.GetClientsAsync();
        return clients.Any() ? Ok(clients) : StatusCode(StatusCodes.Status404NotFound, "There were no clients found to show");
    }

    [HttpGet]
    [Route("GetAllClientsWithoutOrders")]
    public async Task<IActionResult> GetAllClientsWithoutOrders()
    {
        var clients = await _clientService.GetClientsWithoutOrders();
        return clients.Any() ? Ok(clients) : StatusCode(StatusCodes.Status404NotFound, "There were no clients found to show");
    }

    [HttpGet]
    [Route("GetClientsOrdersTotal")]
    public async Task<IActionResult> GetClientsOrdersTotal()
    {
        var clients = await _clientService.GetClientsOrdersTotal();
        return clients.Any() ? Ok(clients) : StatusCode(StatusCodes.Status404NotFound, "There were no clients found to show");
    }

    [HttpPost]
    [Route("CreateClient")]
    public async Task<IActionResult> CreateClient(Client client)
    {
        var newClient = await _clientService.CreateClient(client);
        return Ok(newClient);
    }
}