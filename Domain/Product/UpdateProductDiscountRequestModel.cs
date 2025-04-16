namespace Domain.Product;

public class UpdateProductDiscountRequestModel
{
    public decimal DiscountPercentage { get; set; }

    public int DiscountQuantityThreshold { get; set; }
}