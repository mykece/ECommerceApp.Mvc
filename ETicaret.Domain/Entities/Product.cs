namespace ETicaret.Domain.Entities
{
    public class Product : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? OriginalPrice { get; set; } // Yeni eklendi
        public Gender Gender { get; set; }
        public byte[]? Image { get; set; }


        //nav prop
        public virtual List<CategorySizeTypeProduct> CategorySizeTypeProducts { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public Guid? CampaingId { get; set; }

    }
}
