using CateringService.Domain.Common;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public class GenericRepository<TEntity, TPrimaryKey> : IGenericRepository<TEntity, TPrimaryKey> where TEntity : Entity<TPrimaryKey>
{
    protected readonly AppDbContext _context;
    public GenericRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public TPrimaryKey Add(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        _context.Set<TEntity>().Add(entity);
        return entity.Id;
    }

    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public TPrimaryKey Update(TEntity entity, bool isTracked = false)
    {
        if (isTracked)
        {
            _context.Set<TEntity>().Update(entity);
        }
        else
        {
            _context.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
        return entity.Id;
    }
}