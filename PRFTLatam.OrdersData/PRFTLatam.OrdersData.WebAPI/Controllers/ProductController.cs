using System.Net;
using Microsoft.AspNetCore.Mvc;
using PRFTLatam.OrdersData.Infrastructure.Models;
using PRFTLatam.OrdersData.Services.IServices;

namespace PRFTLatam.OrdersData.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [Route("GetAllProducts")]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetProductsAsync();
        return products.Any() ? Ok(products) : StatusCode(StatusCodes.Status404NotFound, "There were no products found to show");
    }
}