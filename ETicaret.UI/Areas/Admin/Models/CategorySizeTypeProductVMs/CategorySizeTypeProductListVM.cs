namespace ETicaret.UI.Areas.Admin.Models.CategorySizeTypeProductVMs
{
    public class CategorySizeTypeProductListVM
    {
        public Guid Id { get; set; }
        public byte[]? Image { get; set; }
        public string ProductName { get; set; }
        public string SizeType { get; set; }
        public string Size { get; set; }

        public double Quantity { get; set; }
        public decimal Price { get; set; }
        public Guid SizeId { get; set; }
    }
}
