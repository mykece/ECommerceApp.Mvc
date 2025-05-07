using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETicaret.UI.Areas.Admin.Models.CampaignVMs
{
    public class CampaignUpdateVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double DiscountPercentage { get; set; }
        public List<Guid> ProductIds { get; set; }
        public SelectList ProductList { get; set; }
        public IFormFile? NewImage { get; set; }
        public byte[]? ExistingImage { get; set; }

        // Formdan JSON formatında gelen ProductIds değerlerini almak için kullanılan property
        //public string ProductIdsJson { get; set; }

        // Kategori bilgileri için yeni propertyler
        public List<Guid> CategoryIds { get; set; }
        public SelectList CategoryList { get; set; }
        //public string CategoryIdsJson { get; set; }
        public bool IsActive { get; set; }

    }
}
