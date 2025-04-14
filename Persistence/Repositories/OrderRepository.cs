using Dapper;
using Domain.Order;
using Domain.Product;
using Persistence.Infrastructure;

namespace Persistence.Repositories;

public interface IOrderRepository
{
    Task CreateOrderAsync(CreateOrderRequestModel model);
    Task<List<OrderListItem>> GetOrdersAsync();
}

public class OrderRepository(IDbContext dbContext) : DbRepository(dbContext), IOrderRepository
{
    public async Task CreateOrderAsync(CreateOrderRequestModel model)
    {
        const string insertOrderQuery = @"
            INSERT INTO orders (created_at)
            VALUES (NOW())
            RETURNING id;
        ";

        var orderId = await Connection.ExecuteScalarAsync<long>(insertOrderQuery);

        const string insertOrderProductQuery = @"
            INSERT INTO order_products (order_id, product_id, quantity)
            VALUES (@OrderId, @ProductId, @Quantity);
        ";

        var orderProducts = model.Products.Select(p => new {
            OrderId = orderId,
            p.ProductId,
            p.Quantity
        });

        await Connection.ExecuteAsync(insertOrderProductQuery, orderProducts);
    }
    
    public async Task<List<OrderListItem>> GetOrdersAsync()
    {
        const string sql = @"
            SELECT 
                o.id AS OrderId,
                o.created_at AS CreatedAt,
                p.id AS Id,
                p.name AS Name,
                p.price AS Price,
                op.quantity AS Quantity
            FROM orders o
            JOIN order_products op ON o.id = op.order_id
            JOIN products p ON op.product_id = p.id
            ORDER BY o.created_at DESC;
        ";

        var orderDict = new Dictionary<long, OrderListItem>();

        var result = await Connection.QueryAsync<OrderListItem, ProductListItemModel, OrderListItem>(
            sql,
            (order, product) =>
            {
                if (!orderDict.TryGetValue(order.OrderId, out var currentOrder))
                {
                    currentOrder = order;
                    currentOrder.Products = [];
                    orderDict.Add(order.OrderId, currentOrder);
                }

                currentOrder.Products.Add(product);
                return currentOrder;
            },
            splitOn: "ProductId"
        );

        return orderDict.Values.ToList();
    }
}
