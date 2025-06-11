using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public class AddressRepository : GenericRepository<Address, Ulid>, IAddressRepository
{
    public AddressRepository(AppDbContext context) : base(context)
    {
    }

    public Task DeleteAsync(Ulid addressId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasActiveOrdersAsync(Ulid addressId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Address>> SearchByTextAsync(string query)
    {
        query = query.ToLower();

        return await _context.Addresses
            .AsNoTracking()
            .Where(a =>
                EF.Functions.ToTsVector("english", a.City + " " + a.StreetAndBuilding)
                    .Matches(EF.Functions.PhraseToTsQuery("english", query)))
            .OrderByDescending(a => EF.Functions.ToTsVector("english", a.City + " " + a.StreetAndBuilding)
                .Rank(EF.Functions.PhraseToTsQuery("english", query)))
            .ToListAsync();
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