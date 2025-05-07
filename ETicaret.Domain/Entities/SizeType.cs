using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Domain.Entities
{
    public class SizeType : AuditableEntity
    {
        public string SizeTypeName { get; set; }

        //Nav Prop

        public virtual List<CategorySizeType> CategorySizeTypes { get; set; }
        public virtual List<Size> Size { get; set; }


    }
}
