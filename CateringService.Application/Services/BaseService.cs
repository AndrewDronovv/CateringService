using CateringService.Domain.Abstractions;
using CateringService.Domain.Common;
using CateringService.Domain.Repositories;

namespace CateringService.Application.Services;

public class BaseService<T, TPrimaryKey> : IBaseService<T, TPrimaryKey> where T : Entity<TPrimaryKey>
{
    private readonly IBaseRepository<T, TPrimaryKey> _repository;
    protected readonly IUnitOfWork _unitOfWork;

    public BaseService(IBaseRepository<T, TPrimaryKey> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TPrimaryKey> AddAsync(T entity)
    {
        var id = _repository.Add(entity);
        await _unitOfWork.SaveChangesAsync();
        return id;
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

    public async Task<TPrimaryKey> UpdateAsync(T entity)
    {
        var id = _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return id;
    }
}