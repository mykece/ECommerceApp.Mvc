using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Domain.Entities
{
    public class Size : AuditableEntity
    {
        public string SizeName { get; set; }
        public string? Description { get; set; }
        public string? SizeTypeName { get; set; }

        // nav prop
        public virtual SizeType SizeType{ get; set; }
        public Guid SizeTypeId { get; set; }
    }
}
