using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;

namespace CateringService.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<User> GetByLoginAsync(string login)
    {

    }
}