using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.DTOs.CampaignDTOs
{
    public class CampaignCreateDTO
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double DiscountPercentage { get; set; }
        public byte[]? Image { get; set; }
        public List<Guid> ProductIds { get; set; }
        public List<Guid> CategoryIds { get; set; }
    }
}
