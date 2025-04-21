using CateringService.Domain.Common;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public class BaseRepository<T, TPrimaryKey> : IBaseRepository<T, TPrimaryKey> where T : Entity<TPrimaryKey>
{
    protected readonly AppDbContext _context;
    public BaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public TPrimaryKey Add(T entity)
    {
        _context.Set<T>().Add(entity);
        return entity.Id;
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public TPrimaryKey Update(T entity)
    {
        _context.Set<T>().Update(entity);
        return entity.Id;
    }
}