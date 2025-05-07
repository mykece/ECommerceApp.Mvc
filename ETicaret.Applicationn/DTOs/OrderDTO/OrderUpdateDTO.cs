using ETicaret.Applicationn.DTOs.OrderDetailDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.DTOs.OrderDTO;

public class OrderUpdateDTO
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public string ShipAdress { get; set; }
    public DateTime? ShippedDate { get; set; }
    public string? ShipVia { get; set; }
    public string ShipCity { get; set; }
    public string ShipCountry { get; set; }
    public int ShipPostalCode { get; set; }
    public Guid CustomerId { get; set; }
    public List<OrderDetailUpdateDTO> OrderDetailUpdateDTOs { get; set; }
}
