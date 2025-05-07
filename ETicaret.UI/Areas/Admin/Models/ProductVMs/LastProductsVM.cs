namespace ETicaret.UI.Areas.Admin.Models.ProductVMs
{
    public class LastProductsVM
    {
        public bool IsAuthenticated { get; set; }
        public List<ProductListVM> Products { get; set; }
    }
}
