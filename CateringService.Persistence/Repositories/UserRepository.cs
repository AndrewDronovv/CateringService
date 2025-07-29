using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public class UserRepository : GenericRepository<User, Ulid>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task DeleteAsync(Ulid userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        _context.Users.Remove(user);
    }
}