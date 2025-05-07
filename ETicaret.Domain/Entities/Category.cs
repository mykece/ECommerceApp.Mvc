using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Domain.Entities
{
    public class Category : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[]? Image { get; set; }

        //Nav Prop

        public virtual List<CategorySizeType> CategorySizeTypes { get; set; }
    }
}
