using TourGuide.Domain.Interfaces;
using TourGuide.Infrastructure.Data;

namespace TourGuide.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Chat = new ChatRepository(context);
    }

    public IChatRepository Chat { get; }

    public IRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T);
        if (!_repositories.ContainsKey(type))
            _repositories[type] = new GenericRepository<T>(_context);

        return (IRepository<T>)_repositories[type];
    }

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public void Dispose()
        => _context.Dispose();
}