using CateringService.Domain.Common;

namespace CateringService.Domain.Abstractions;

public interface IBaseService<T, TPrimaryKey> where T : Entity<TPrimaryKey>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(TPrimaryKey id);
    Task<T?> AddAsync(T entity);
    Task DeleteAsync(TPrimaryKey id);
    Task<T?> UpdateAsync(TPrimaryKey key, T entity);
}