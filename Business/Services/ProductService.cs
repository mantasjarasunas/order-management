using Domain.Product;
using Persistence.Infrastructure;
using Persistence.Repositories;

namespace Business.Services;

public interface IProductService
{
    Task CreateProductAsync(CreateProductRequestModel model);
    Task<IEnumerable<ProductListItemModel>> GetFilteredProductsAsync(string? searchQuery);
    Task UpdateProductDiscountAsync(long productId, UpdateProductDiscountRequestModel model);
}

public class ProductService(IDbContext dbContext, IProductRepository productRepository) : IProductService
{
    public async Task CreateProductAsync(CreateProductRequestModel model)
    {
        await productRepository.CreateProductAsync(model);
        dbContext.Commit();
    }

    public async Task<IEnumerable<ProductListItemModel>> GetFilteredProductsAsync(string? searchQuery)
    {
        var filteredProducts = await productRepository.GetFilteredProductsAsync(searchQuery);

        return filteredProducts;
    }

    public async Task UpdateProductDiscountAsync(long productId, UpdateProductDiscountRequestModel model)
    {
        await productRepository.UpdateProductDiscountAsync(productId, model);
        dbContext.Commit();
    }
}