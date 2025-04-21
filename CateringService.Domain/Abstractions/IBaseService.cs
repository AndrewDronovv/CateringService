using CateringService.Domain.Common;

namespace CateringService.Domain.Abstractions;

public interface IBaseService<T, TPrimaryKey> where T : Entity<TPrimaryKey>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<TPrimaryKey> AddAsync(T entity);
    Task DeleteAsync(int id);
    Task<TPrimaryKey> UpdateAsync(T entity);
}
