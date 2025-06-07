using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public class AddressRepository : GenericRepository<Address, Ulid>, IAddressRepository
{
    public AddressRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Address>> SearchByZipAsync(Ulid? tenantId, string zip)
    {
        IQueryable<Address> query = _context.Addresses.AsNoTracking();

        if (tenantId.HasValue)
        {
            query = query.Where(a => a.TenantId == tenantId.Value);
        }

        query = query.Where(a => a.Zip == zip);

        return await query.OrderByDescending(a => a.CreatedAt).ToListAsync();
    }
}