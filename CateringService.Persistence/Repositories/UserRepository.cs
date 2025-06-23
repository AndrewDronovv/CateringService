using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Repositories;

namespace CateringService.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task AddAsync(User user)
    {
        throw new NotImplementedException();
    }
}