using Dapper;
using Domain.Order;
using Domain.Product;
using Persistence.Infrastructure;

namespace Persistence.Repositories;

public interface IOrderRepository
{
    Task CreateOrderAsync(CreateOrderRequestModel model);
    Task<List<OrderListItem>> GetOrdersAsync();
    Task<List<OrderInvoiceItemModel>> GetOrderInvoiceItemsAsync(long orderId);
    Task<List<DiscountedOrderProductsListItemModel>> GetDiscountedProductsAsync();
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
        const string query = @"
            SELECT 
                o.id AS OrderId,
                o.created_at AS CreatedAt,
                p.id AS ProductId,
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
            query,
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

    public async Task<List<OrderInvoiceItemModel>> GetOrderInvoiceItemsAsync(long orderId)
    {
        var query = @"
            SELECT 
                p.name AS Name,
                op.quantity AS Quantity,
                p.discount_percentage AS DiscountPercentage,
                ROUND(
                    op.quantity * p.price * 
                    CASE 
                        WHEN p.discount_percentage IS NOT NULL 
                        THEN (1 - p.discount_percentage / 100.0)
                        ELSE 1 
                    END, 
                2) AS Amount
            FROM order_products op
            INNER JOIN products p ON p.id = op.product_id
            WHERE op.order_id = @OrderId;
        ";

        var result = await Connection.QueryAsync<OrderInvoiceItemModel>(query, new { OrderId = orderId });
        
        return result.ToList();
    }

    public async Task<List<DiscountedOrderProductsListItemModel>> GetDiscountedProductsAsync()
    {
        const string query = @"
            SELECT 
                p.name AS ProductName,
                p.discount_percentage AS DiscountPercentage,
                COUNT(DISTINCT o.id) AS OrdersCount,
                ROUND(SUM(op.quantity * p.price * (1 - COALESCE(p.discount_percentage, 0) / 100.0)), 2) AS TotalAmountOrdered
            FROM order_products op
            JOIN products p ON op.product_id = p.id
            JOIN orders o ON op.order_id = o.id
            WHERE p.discount_percentage IS NOT NULL
            GROUP BY p.id, p.name, p.discount_percentage
            ORDER BY TotalAmountOrdered DESC;";
        
        var result = await Connection.QueryAsync<DiscountedOrderProductsListItemModel>(query);
        
        return result.ToList();
    }
}
