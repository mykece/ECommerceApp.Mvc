namespace ETicaret.Infrastructure.DataAccess.Interfaces
{
    public interface IAsyncRepository
    {
        Task<int> SaveChangesAsync();
    }
}
