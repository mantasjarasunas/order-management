namespace Domain.Product;

public class ProductListItemModel
{
    public long ProductId { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }
}