using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public class TenantRepository : GenericRepository<Tenant, Ulid>, ITenantRepository
{
    public TenantRepository(AppDbContext context) : base(context)
    {
    }

    public Ulid Add(Tenant input)
    {
        _context.Tenants.Add(input);
        return input.Id;
    }

    public async Task BlockAsync(Ulid tenantId, string blockReason)
    {
        await _context.Tenants
            .Where(t => t.Id == tenantId)
            .ExecuteUpdateAsync(t => t
                .SetProperty(x => x.IsActive, false)
                .SetProperty(t => t.BlockReason, blockReason));
    }
    public async Task UnblockAsync(Ulid tenantId)
    {
        await _context.Tenants
            .Where(t => t.Id == tenantId)
            .ExecuteUpdateAsync(t => t
                .SetProperty(x => x.IsActive, true)
                .SetProperty(t => t.BlockReason, string.Empty));
    }

    public async Task<IEnumerable<Tenant>> GetAllAsync()
    {
        return await _context.Tenants.ToListAsync();
    }

    public async Task<Tenant?> GetByIdAsync(Ulid tenantId)
    {
        return await _context.Tenants
            .FirstOrDefaultAsync(t => t.Id == tenantId);
    }

    public async Task<Tenant> UpdateAsync(Tenant tenant, bool isNotTracked = false)
    {
        if (isNotTracked)
        {
            _context.Attach(tenant);
            _context.Entry(tenant).State = EntityState.Modified;
        }

        return tenant;
    }

    public async Task<bool> CheckActiveTenantExistsAsync(Ulid tenantId)
    {
        return await _context.Tenants
            .AnyAsync(t => t.Id == tenantId && t.IsActive);
    }

    public async Task DeleteAsync(Ulid tenantId)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Id == tenantId);

        _context.Tenants.Remove(tenant);
    }
}