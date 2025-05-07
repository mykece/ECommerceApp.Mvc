namespace ETicaret.Domain.Core.BaseEntities
{
    public class AuditableEntity : BaseEntity, IDeletetebleEntity
    {
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }
}
