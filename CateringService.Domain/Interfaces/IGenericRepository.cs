using CateringService.Domain.Common;

namespace CateringService.Domain.Repositories;

public interface IGenericRepository<TEntity, TPrimaryKey> where TEntity : Entity<TPrimaryKey>
{
    Task<TEntity?> GetByIdAsync(TPrimaryKey entityId, bool asNoTracking = false);
    Task<IEnumerable<TEntity>> GetAllAsync();
    TPrimaryKey Add(TEntity entity);
    void Delete(TEntity entity);
    TPrimaryKey Update(TEntity entity, bool isTracked = false);
}