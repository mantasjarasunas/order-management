using Dapper;
using Domain.Product;
using Persistence.Infrastructure;

namespace Persistence.Repositories;

public interface IProductRepository
{
    Task CreateProductAsync(CreateProductRequestModel model);
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
}