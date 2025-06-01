using CateringService.Domain.Entities;
using CateringService.Domain.Exceptions;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly AppDbContext _context;

    public TenantRepository(AppDbContext context)
    {
        _context = context;
    }

    public Ulid Add(Tenant input)
    {
        _context.Tenants.Add(input);
        return input.Id;
    }

    public async Task BlockAsync(Ulid tenantId, string blockReason)
    {
        int updatedRows = await _context.Tenants
            .Where(t => t.Id == tenantId)
            .ExecuteUpdateAsync(t => t
                .SetProperty(x => x.IsActive, false)
                .SetProperty(t => t.BlockReason, blockReason));

        if (updatedRows == 0)
        {
            throw new NotFoundException(nameof(Tenant), tenantId.ToString());
        }
    }
    public async Task UnblockAsync(Ulid tenantId)
    {
        await _context.Tenants
            .Where(t => t.Id == tenantId)
            .ExecuteUpdateAsync(t => t.SetProperty(x => x.IsActive, true)
            .SetProperty(t => t.BlockReason, string.Empty));
    }

    public async Task<IEnumerable<Tenant>> GetAllAsync()
    {
        return await _context.Tenants.ToListAsync();
    }

    public async Task<Tenant?> GetByIdAsync(Ulid tenantId)
    {
        return await _context.Tenants
            .FindAsync(tenantId);
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

    public async Task<bool> CheckTenantExists(Ulid tenantId)
    {
        return await _context.Tenants
            .AnyAsync(t => t.Id == tenantId);
    }

    public async Task DeleteAsync(Ulid tenantId)
    {
        var entity = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Id == tenantId);

        _context.Tenants.Remove(entity);
    }

    public Task<bool> HasRelatedDataAsync(Ulid tenantId)
    {
        throw new NotImplementedException();
    }
}