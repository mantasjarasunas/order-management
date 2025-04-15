namespace Domain.Order;

public class OrderInvoiceSummaryModel
{
    public List<OrderInvoiceItemModel> Items { get; set; } = [];
    public decimal TotalAmount { get; set; }
}

public class OrderInvoiceItemModel
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public decimal Amount { get; set; }
}
