using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;

namespace CateringService.Application.Abstractions;

public interface ITenantService
{
    Task<List<TenantViewModel>> GetTenantsAsync();
    Task<TenantViewModel> GetTenantByIdAsync(Ulid tenantId);
    Task<TenantViewModel?> CreateTenantAsync(AddTenantRequest request);
    Task DeleteTenantAsync(Ulid tenantId);
    Task<TenantViewModel> UpdateTenantAsync(Ulid tenantId, UpdateTenantRequest requestn);
    Task<TenantViewModel> BlockTenantAsync(Ulid tenantId, string blockReason);
    Task<TenantViewModel> UnblockTenantAsync(Ulid tenantId);
}