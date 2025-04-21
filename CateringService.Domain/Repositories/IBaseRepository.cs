using CateringService.Domain.Common;

namespace CateringService.Domain.Repositories;

public interface IBaseRepository<T, TPrimaryKey> where T : Entity<TPrimaryKey>
{
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    TPrimaryKey Add(T entity);
    TPrimaryKey Update(T entity);
    void Delete(T entity);
}