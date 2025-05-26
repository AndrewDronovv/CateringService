using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Repositories;

namespace CateringService.Persistence.Repositories;

public class AddressRepository : GenericRepository<Address, Ulid>, IAddressRepository
{
    public AddressRepository(AppDbContext context) : base(context)
    {
    }
}