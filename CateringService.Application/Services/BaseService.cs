using CateringService.Domain.Abstractions;
using CateringService.Domain.Common;
using CateringService.Domain.Exceptions;
using CateringService.Domain.Repositories;

namespace CateringService.Application.Services;

public class BaseService<TEntity, TPrimaryKey> : IBaseService<TEntity, TPrimaryKey>
    where TEntity : Entity<TPrimaryKey>
{
    private readonly IGenericRepository<TEntity, TPrimaryKey> _repository;
    protected readonly IUnitOfWorkRepository _unitOfWork;

    public BaseService(IGenericRepository<TEntity, TPrimaryKey> repository, IUnitOfWorkRepository unitOfWork)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<TEntity?> AddAsync(TEntity entity)
    {
        var id = _repository.Add(entity);
        await _unitOfWork.SaveChangesAsync();
        return await _repository.GetByIdAsync(id);
    }

    public async Task DeleteAsync(TPrimaryKey id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Сущность с Id = {id} не найдена.");
        }
        _repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TPrimaryKey id, bool isTrackable = false)
    {
        var entity = await _repository.GetByIdAsync(id, isTrackable);
        if (entity is null)
        {
            throw new NotFoundException(typeof(TEntity).Name, id.ToString());
        }
        return entity;
    }

    public async Task<TEntity?> UpdateAsync(TPrimaryKey id, TEntity entity)
    {
        var oldEntity = await _repository.GetByIdAsync(id);

        if (oldEntity == null)
        {
            throw new Exception($"Сущность с ключом {id} не найдена");
        }

        UpdateEntity(oldEntity, entity);

        var key = _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return await _repository.GetByIdAsync(key);
    }

    protected virtual void UpdateEntity(TEntity oldEntity, TEntity newEntity)
    {
    }
}