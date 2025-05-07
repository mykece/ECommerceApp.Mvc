using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Domain.Entities;

public class Cart 
{
    public List<CartItem> Items { get; set; } = new List<CartItem>();
}
