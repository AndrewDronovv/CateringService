using CateringService.Domain.Common;
using CateringService.Domain.Entities;
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

    public void Delete(Tenant tenant)
    {
        _context.Tenants.Remove(tenant);
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
}