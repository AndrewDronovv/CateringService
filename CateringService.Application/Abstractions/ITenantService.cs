using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Domain.Entities;

namespace CateringService.Application.Abstractions;

public interface ITenantService
{
    Task<List<TenantViewModel>> GetTenantsAsync();
    Task<TenantViewModel> GetTenantByIdAsync(Ulid tenantId);
    Task<TenantViewModel?> CreateTenantAsync(AddTenantRequest request);
    Task DeleteTenantAsync(Ulid tenantId);
    Task<Tenant> UpdateTenantAsync(Ulid tenantId, Tenant tenant);
    Task<TenantViewModel> BlockTenantAsync(Ulid tenantId, string blockReason);
    Task<TenantViewModel> UnblockTenantAsync(Ulid tenantId);
}