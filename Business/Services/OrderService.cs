using Domain.Order;
using Persistence.Infrastructure;
using Persistence.Repositories;

namespace Business.Services;

public interface IOrderService
{
    Task CreateOrderAsync(CreateOrderRequestModel model);
    Task<List<OrderListItem>> GetOrdersAsync();
    Task<OrderInvoiceSummaryModel> GetOrderInvoiceAsync(long orderId);
    Task<List<DiscountedOrderProductsListItemModel>> GetDiscountedProductsAsync();
}

public class OrderService(IOrderRepository orderRepository, IDbContext dbContext) : IOrderService
{
    public async Task CreateOrderAsync(CreateOrderRequestModel model)
    {
        await orderRepository.CreateOrderAsync(model);
        dbContext.Commit();
    }
    
    public async Task<List<OrderListItem>> GetOrdersAsync()
    {
        return await orderRepository.GetOrdersAsync();
    }
    
    public async Task<OrderInvoiceSummaryModel> GetOrderInvoiceAsync(long orderId)
    {
        var items = await orderRepository.GetOrderInvoiceItemsAsync(orderId);
        var total = items.Sum(x => x.Amount);

        return new OrderInvoiceSummaryModel
        {
            Items = items,
            TotalAmount = total
        };
    }

    public async Task<List<DiscountedOrderProductsListItemModel>> GetDiscountedProductsAsync()
    {
        var items = await orderRepository.GetDiscountedProductsAsync();

        return items;
    }
}