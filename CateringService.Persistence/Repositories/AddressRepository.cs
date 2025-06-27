using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public class AddressRepository : GenericRepository<Address, Ulid>, IAddressRepository
{
    public AddressRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<bool> CheckAddressExistsAsync(Ulid addressId)
    {
        return await _context.Addresses
            .AnyAsync(a => a.Id == addressId);
    }

    public async Task DeleteAsync(Ulid addressId)
    {
        var entity = await _context.Addresses
            .FirstOrDefaultAsync(a => a.Id == addressId);

        _context.Addresses.Remove(entity);
    }

    public Task<bool> HasActiveOrdersAsync(Ulid addressId)
    {
        return Task.FromResult(true);
        //return await _context.Addresses
        //    .Where(a => a.Id == addressId)
        //    .SelectMany(a => a.Orders)
        //    .AnyAsync(o => o.IsActive == true);
    }

    // TODO: Доделать метод, необходимо добавить параметр tenantId.
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