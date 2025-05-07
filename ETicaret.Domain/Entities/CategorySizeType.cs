using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Domain.Entities
{
    public class CategorySizeType : BaseEntity
    {
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public Guid SizeTypeId { get; set; }
        public virtual SizeType SizeType { get; set; }
        public virtual List<CategorySizeTypeProduct> CategorySizeTypeProducts { get; set; }
    }
}
