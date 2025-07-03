using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;

namespace CateringService.Persistence.Repositories;

public class UserRepository : GenericRepository<User, Ulid>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Ulid> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        return user.Id;
    }
}