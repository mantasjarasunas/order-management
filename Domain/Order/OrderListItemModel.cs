using Domain.Product;

namespace Domain.Order;

public class OrderListItem
{
    public long OrderId { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<ProductListItemModel> Products { get; set; } = [];
}