using Domain.Product;
using Persistence.Infrastructure;
using Persistence.Repositories;

namespace Business.Services;

public interface IProductService
{
    Task CreateProductAsync(CreateProductRequestModel model);
}

public class ProductService(IDbContext dbContext, IProductRepository productRepository) : IProductService
{
    public async Task CreateProductAsync(CreateProductRequestModel model)
    {
        await productRepository.CreateProductAsync(model);
        dbContext.Commit();
    }
}
