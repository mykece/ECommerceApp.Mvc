using ETicaret.Domain.Enums;

namespace ETicaret.UI.Areas.Admin.Models.CampaignVMs
{
    public class CampaignListVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public CampaignStatus CampaignStatus { get; set; }
        public byte[]? Image { get; set; }
        public double DiscountPercentage { get; set; }
        public List<string> ProductNames { get; set; } = new List<string>();

    }
}
