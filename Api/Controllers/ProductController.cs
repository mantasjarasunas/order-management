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
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductListItemModel>>> GetFilteredProducts([FromQuery] string? searchQuery)
    {
        var products = await productService.GetFilteredProductsAsync(searchQuery);
        return Ok(products);
    }
    
    [HttpPut("{id}/discount")]
    public async Task<ActionResult> UpdateProductDiscount(long id, [FromBody] UpdateProductDiscountRequestModel model)
    {
        await productService.UpdateProductDiscountAsync(id, model);
        return Ok();
    }
}