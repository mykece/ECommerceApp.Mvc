namespace ETicaret.Domain.Core.Interfaces
{
    public interface IEntity
    {
        public Guid Id { get; set; }
        public Status Status { get; set; }
    }
}
