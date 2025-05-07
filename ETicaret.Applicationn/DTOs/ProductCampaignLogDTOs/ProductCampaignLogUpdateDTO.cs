using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.DTOs.ProductCampaignLogDTOs
{
    public class ProductCampaignLogUpdateDTO
    {
        public Guid Id { get; set; }
        public string CampaignName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //public bool IsActive { get; set; } = false;
        public double DiscountPercentage { get; set; }
        public Guid ProductId { get; set; }
        public Guid CampaignId { get; set; }
    }
}
