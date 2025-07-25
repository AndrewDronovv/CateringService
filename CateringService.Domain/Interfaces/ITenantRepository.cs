﻿using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface ITenantRepository : IGenericRepository<Tenant, Ulid>
{
    Task<IEnumerable<Tenant>> GetAllAsync();
    Task<Tenant?> GetByIdAsync(Ulid tenantId);
    Ulid Add(Tenant tenant);
    Task DeleteAsync(Ulid tenantId);
    Task<Tenant> UpdateAsync(Tenant tenant, bool isNotTracked = false);
    Task BlockAsync(Ulid tenantId, string blockReason);
    Task UnblockAsync(Ulid tenantId);
    Task<bool> CheckActiveTenantExistsAsync(Ulid tenantId);
}