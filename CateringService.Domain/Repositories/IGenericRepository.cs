using CateringService.Domain.Common;

namespace CateringService.Domain.Repositories;

public interface IGenericRepository<TEntity, TPrimaryKey> where TEntity : Entity<TPrimaryKey>
{
    Task<TEntity?> GetByIdAsync(TPrimaryKey entityId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    TPrimaryKey Add(TEntity entity);
    void Delete(TEntity entity);
    TPrimaryKey Update(TEntity entity, bool isTracked = false);
}