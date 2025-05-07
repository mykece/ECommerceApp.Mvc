using ETicaret.Applicationn.DTOs.ProductDTOs;

namespace ETicaret.UI.Models.CategoryVMs
{
    public class CategoryProductListVm
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }

        private decimal? _exchange;
        public decimal Exchange
        {
            get { return _exchange ?? 33.40m; }
            set { _exchange = value; }
        }
        public IEnumerable<ProductListDTO> Products { get; set; }
    }
}
