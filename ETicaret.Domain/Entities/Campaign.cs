using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Domain.Entities
{
    public class Campaign : AuditableEntity
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = false;
        public CampaignStatus CampaignStatus { get; set; }
        public double DiscountPercentage { get; set; }
        public byte[]? Image { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}

