using ETicaret.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.DTOs.CampaignDTOs
{
    public class CampaignListDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public CampaignStatus CampaignStatus { get; set; }
        public double DiscountPercentage { get; set; }
        public byte[]? Image { get; set; }
        public List<Guid> ProductIds { get; set; }
        public List<string> ProductNames { get; set; } = new List<string>();
        public List<Guid> CategoryIds { get; set; }
        public List<string> CategoryNames { get; set; } = new List<string>();

    }
}
