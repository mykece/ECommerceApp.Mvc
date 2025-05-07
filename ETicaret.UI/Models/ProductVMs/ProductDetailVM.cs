namespace ETicaret.UI.Models.ProductVMs;

public class ProductDetailVM
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal UnitPrice { get; set; }
    public byte[] Image { get; set; }
    public double Quantity { get; set; }
    
}


