namespace ETicaret.Domain.Core.Interfaces
{
    public interface IDeletetebleEntity
    {
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
