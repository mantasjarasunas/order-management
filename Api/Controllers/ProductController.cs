using Business.Services;
using Domain.Product;
using Microsoft.AspNetCore.Mvc;

namespace order_management_api.Controllers;

/// <summary>
/// Manages operations related to products.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ProductController(IProductService productService) : ControllerBase
{
    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="model">Product creation model.</param>
    [HttpPost]
    public async Task<ActionResult> CreateProduct([FromBody] CreateProductRequestModel model)
    {
        await productService.CreateProductAsync(model);
        return Ok();
    }

    /// <summary>
    /// Retrieves products filtered by an optional search query.
    /// </summary>
    /// <param name="searchQuery">The optional search string to filter products.</param>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductListItemModel>>> GetFilteredProducts([FromQuery] string? searchQuery)
    {
        var products = await productService.GetFilteredProductsAsync(searchQuery);
        return Ok(products);
    }

    /// <summary>
    /// Updates the discount for a specific product.
    /// </summary>
    /// <param name="id">The ID of the product.</param>
    /// <param name="model">Discount update model.</param>
    [HttpPut("{id}/discount")]
    public async Task<ActionResult> UpdateProductDiscount(long id, [FromBody] UpdateProductDiscountRequestModel model)
    {
        await productService.UpdateProductDiscountAsync(id, model);
        return Ok();
    }
}