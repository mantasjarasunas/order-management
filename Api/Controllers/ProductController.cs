using Business.Services;
using Domain.Product;
using Microsoft.AspNetCore.Mvc;

namespace order_management_api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CreateProduct([FromBody] CreateProductRequestModel model)
    {
        await productService.CreateProductAsync(model);
        return Ok();
    }
}