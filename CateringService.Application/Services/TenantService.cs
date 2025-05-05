using CateringService.Application.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;

namespace CateringService.Application.Services;

public class TenantService : ITenantService
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWorkRepository _unitOfWork;

    public TenantService(ITenantRepository tenantRepository, IUnitOfWorkRepository unitOfWork)
    {
        _tenantRepository = tenantRepository ?? throw new ArgumentNullException(nameof(tenantRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Tenant?> AddAsync(Tenant tenant)
    {
        var id = _tenantRepository.Add(tenant);
        await _unitOfWork.SaveChangesAsync();
        return await _tenantRepository.GetByIdAsync(id);
    }

    public async Task DeleteAsync(Ulid tenantId)
    {
        var tenant = await _tenantRepository.GetByIdAsync(tenantId);
        if (tenant is null)
        {
            throw new KeyNotFoundException($"Арендатор с Id = {tenantId} не найден.");
        }
        _tenantRepository.Delete(tenant);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<Tenant?> GetTenantByIdAsync(Ulid tenantId)
    {
        return await _tenantRepository.GetByIdAsync(tenantId);
    }

    public async Task<IEnumerable<Tenant>> GetTenantsAsync()
    {
        return await _tenantRepository.GetAllAsync();
    }
}
