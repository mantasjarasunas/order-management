using Domain.Product;

namespace Domain.Order;

public class CreateOrderRequestModel
{
    public List<OrderProductItem> Products { get; set; } = [];
}

public class OrderProductItem
{
    public long ProductId { get; set; }
    public int Quantity { get; set; }
}