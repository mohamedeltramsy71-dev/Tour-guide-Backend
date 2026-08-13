namespace TourGuide.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> Repository<T>() where T : class;
    IChatRepository Chat { get; }
    Task<int> SaveChangesAsync();
}