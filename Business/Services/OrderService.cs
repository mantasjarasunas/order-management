using Domain.Order;
using Persistence.Infrastructure;
using Persistence.Repositories;

namespace Business.Services;

public interface IOrderService
{
    Task CreateOrderAsync(CreateOrderRequestModel model);
    Task<List<OrderListItem>> GetOrdersAsync();
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
}