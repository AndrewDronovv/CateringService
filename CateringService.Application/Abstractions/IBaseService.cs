using CateringService.Domain.Common;

namespace CateringService.Domain.Abstractions;

public interface IBaseService<TEntity, TPrimaryKey> where TEntity : Entity<TPrimaryKey>
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(TPrimaryKey id, bool isTrackable = false);
    Task<TEntity?> AddAsync(TEntity entity);
    Task DeleteAsync(TPrimaryKey id);
    Task<TEntity?> UpdateAsync(TPrimaryKey id, TEntity entity);
}