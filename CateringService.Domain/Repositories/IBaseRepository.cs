using CateringService.Domain.Common;

namespace CateringService.Domain.Repositories;

public interface IBaseRepository<T, TPrimaryKey> where T : Entity<TPrimaryKey>
{
    Task<T?> GetByIdAsync(TPrimaryKey entityId, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    TPrimaryKey Add(T entity);
    void Delete(T entity);
    TPrimaryKey Update(T entity, bool isTracked = false);
}