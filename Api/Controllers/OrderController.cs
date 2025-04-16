using Business.Services;
using Domain.Order;
using Microsoft.AspNetCore.Mvc;

namespace order_management_api.Controllers;

/// <summary>
/// Manages operations related to orders.
/// </summary>
[ApiController]
[Route("[controller]")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="model">Order details.</param>
    [HttpPost]
    public async Task<ActionResult> CreateOrder([FromBody] CreateOrderRequestModel model)
    {
        await orderService.CreateOrderAsync(model);
        return Ok();
    }

    /// <summary>
    /// Retrieves a list of all orders.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> GetOrders()
    {
        var result = await orderService.GetOrdersAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves an invoice for a specific order.
    /// </summary>
    /// <param name="orderId">The ID of the order.</param>
    [HttpGet("{orderId}/invoice")]
    public async Task<ActionResult<OrderInvoiceItemModel>> GetOrderInvoice(long orderId)
    {
        var result = await orderService.GetOrderInvoiceAsync(orderId);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves report with a list of discounted products
    /// </summary>
    [HttpGet("discounted-products")]
    public async Task<ActionResult<List<DiscountedOrderProductsListItemModel>>> GetDiscountedProducts()
    {
        var report = await orderService.GetDiscountedProductsAsync();
        return Ok(report);
    }
}