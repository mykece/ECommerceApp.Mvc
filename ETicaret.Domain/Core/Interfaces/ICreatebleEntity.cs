namespace ETicaret.Domain.Core.Interfaces
{
    public interface ICreatebleEntity : IEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
