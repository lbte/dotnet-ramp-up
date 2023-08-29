using System.Net;
using Microsoft.AspNetCore.Mvc;
using PRFTLatam.OrdersData.Infrastructure.Models;
using PRFTLatam.OrdersData.Services.IServices;

namespace PRFTLatam.OrdersData.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [Route("GetAllOrders")]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetOrdersAsync();
        return orders.Any() ? Ok(orders) : StatusCode(StatusCodes.Status404NotFound, "There were no orders found to show");
    }

    [HttpGet]
    [Route("GetOrdersByClient")]
    public async Task<IActionResult> GetOrdersByClient(string id)
    {
        var orders = await _orderService.GetOrdersByClient(id);
        return orders.Any() ? Ok(orders) : StatusCode(StatusCodes.Status404NotFound, $"There were no orders found to show for the client with id {id}");
    }
}