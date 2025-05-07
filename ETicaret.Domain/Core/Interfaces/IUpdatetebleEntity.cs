namespace ETicaret.Domain.Core.Interfaces
{
    public interface IUpdatetebleEntity : ICreatebleEntity
    {
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
