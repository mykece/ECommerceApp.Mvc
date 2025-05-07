namespace ETicaret.UI.Areas.Admin.Models.CampaignVMs
{
    public class CampaignDetailVM
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CampaignStatus { get; set; }
        public bool IsActive { get; set; }
        public int DiscountPercentage { get; set; }
        public List<string> ProductNames { get; set; }
    }
}
