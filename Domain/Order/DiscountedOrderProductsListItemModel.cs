namespace Domain.Order;

public class DiscountedOrderProductsListItemModel
{
    public string ProductName { get; set; } = default!;
    
    public decimal DiscountPercentage { get; set; }
    
    public int OrdersCount { get; set; }
    
    public decimal TotalAmountOrdered { get; set; }
}