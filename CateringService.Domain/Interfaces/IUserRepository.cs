using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface IUserRepository : IGenericRepository<User, Ulid>
{
    Task<Ulid> AddAsync(User user);
}