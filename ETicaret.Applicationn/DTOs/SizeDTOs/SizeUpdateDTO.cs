using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.DTOs.SizeDTOs;

public class SizeUpdateDTO
{
    public Guid Id { get; set; }
    public string SizeName { get; set; }
    public string? Description { get; set; }
    public string? SizeTypeName { get; set; }
    public  Guid SizeTypeId { get; set; }
}
