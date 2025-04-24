using CateringService.Domain.Abstractions;
using CateringService.Domain.Common;
using CateringService.Domain.Repositories;

namespace CateringService.Application.Services;

public class BaseService<T, TPrimaryKey> : IBaseService<T, TPrimaryKey> where T : Entity<TPrimaryKey>
{
    private readonly IBaseRepository<T, TPrimaryKey> _repository;
    protected readonly IUnitOfWorkRepository _unitOfWork;

    public BaseService(IBaseRepository<T, TPrimaryKey> repository, IUnitOfWorkRepository unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<T?> AddAsync(T entity)
    {
        var id = _repository.Add(entity);
        await _unitOfWork.SaveChangesAsync();
        return await _repository.GetByIdAsync(id);
    }

    public async Task DeleteAsync(TPrimaryKey id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity != null)
        {
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<T?> GetByIdAsync(TPrimaryKey id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<T?> UpdateAsync(TPrimaryKey key, T entity)
    {
        var oldEntity = await _repository.GetByIdAsync(key);

        if (oldEntity == null)
        {
            throw new Exception($"Сущность с ключом {key} не найдена");
        }

        UpdateEntity(oldEntity, entity);

        var id = _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return await _repository.GetByIdAsync(id);
    }

    protected virtual void UpdateEntity(T oldEntity, T newEntity)
    {
    }
}