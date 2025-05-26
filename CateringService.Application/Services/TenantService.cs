using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Responses;
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

    public async Task<Tenant?> AddTenantAsync(Tenant tenant)
    {
        var id = _tenantRepository.Add(tenant);
        await _unitOfWork.SaveChangesAsync();
        return await _tenantRepository.GetByIdAsync(id);
    }

    public async Task<TenantViewModel> BlockTenantAsync(Ulid tenantId, string blockReason)
    {
        var tenant = await _tenantRepository.GetByIdAsync(tenantId);
        if (tenant is null)
        {
            throw new KeyNotFoundException($"Арендатор с Id = {tenantId} не найден.");
        }

        return await _tenantRepository.BlockAsync(tenantId, blockReason).ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                throw new Exception($"Не удалось заблокировать арендатора с Id = {tenantId}.", t.Exception);
            }
            return new TenantViewModel
            {
                Id = tenantId,
                Name = tenant.Name,
                IsActive = false,
                BlockReason = blockReason
            };
        });
    }

    public async Task<TenantViewModel> UnblockTenantAsync(Ulid tenantId)
    {
        var tenant = await _tenantRepository.GetByIdAsync(tenantId);
        if (tenant is null)
        {
            throw new KeyNotFoundException($"Арендатор с Id = {tenantId} не найден.");
        }

        return await _tenantRepository.UnblockAsync(tenantId).ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                throw new Exception($"Не удалось разблокировать арендатора с Id = {tenantId}.", t.Exception);
            }
            return new TenantViewModel
            {
                Id = tenantId,
                Name = tenant.Name,
                IsActive = true,
                BlockReason = string.Empty
            };
        });
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

    public async Task<Tenant?> UpdateTenantAsync(Ulid tenantId, Tenant tenant)
    {
        var oldTenant = await _tenantRepository.GetByIdAsync(tenantId);

        if (oldTenant == null)
        {
            throw new Exception($"Сущность с ключом {tenantId} не найдена");
        }

        UpdateTenant(oldTenant, tenant);

        var updatedTenant = _tenantRepository.UpdateAsync(tenant);
        await _unitOfWork.SaveChangesAsync();
        return await updatedTenant;
    }

    private void UpdateTenant(Tenant oldTenant, Tenant newTenant)
    {
        if (!oldTenant.Name.Equals(newTenant.Name, StringComparison.Ordinal))
        {
            oldTenant.Name = newTenant.Name;
        }
    }
}