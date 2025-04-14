using Business.Services;
using Domain.Order;
using Microsoft.AspNetCore.Mvc;

namespace order_management_api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CreateOrder([FromBody] CreateOrderRequestModel model)
    {
        await orderService.CreateOrderAsync(model);
        return Ok();
    }
    
    [HttpGet]
    public async Task<ActionResult> GetOrders()
    {
        var result = await orderService.GetOrdersAsync();
        return Ok(result);
    }
}