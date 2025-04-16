using Dapper;
using Domain.Product;
using Persistence.Infrastructure;

namespace Persistence.Repositories;

public interface IProductRepository
{
    Task CreateProductAsync(CreateProductRequestModel model);
    Task<IEnumerable<ProductListItemModel>> GetFilteredProductsAsync(string? searchQuery);
    Task UpdateProductDiscountAsync(long productId, UpdateProductDiscountRequestModel model);
}

public class ProductRepository(IDbContext dbContext) : DbRepository(dbContext), IProductRepository
{
    public async Task CreateProductAsync(CreateProductRequestModel model)
    {
        var query = @"
            INSERT INTO products (
                name,
                price,
                created_at,
                updated_at
            ) 
            VALUES (
                @Name,
                @Price,
                @DateTime,
                @DateTime
            )
        ";

        await Connection.ExecuteAsync(
            query,
            new
            {
                model.Name,
                model.Price,
                DateTime = DateTime.UtcNow
            }
        );
    }

    public async Task<IEnumerable<ProductListItemModel>> GetFilteredProductsAsync(string? searchQuery)
    {
        const string query = @"
                SELECT 
                    name Name,
                    price Price
                FROM products
                WHERE name ILIKE CONCAT('%', @searchQuery,'%')
            ";

        return await Connection.QueryAsync<ProductListItemModel>(query, new { searchQuery });
    }

    public async Task UpdateProductDiscountAsync(long productId, UpdateProductDiscountRequestModel model)
    {
        const string query = @"
            UPDATE products
            SET
                discount_percentage = @DiscountPercentage,
                discount_quantity_threshold = @DiscountQuantityThreshold,
                updated_at = @DateTime
            WHERE id = @ProductId
        ";

        await Connection.ExecuteAsync(query, new
        {
            ProductId = productId,
            model.DiscountPercentage,
            model.DiscountQuantityThreshold,
            DateTime = DateTime.UtcNow
        });
    }
}