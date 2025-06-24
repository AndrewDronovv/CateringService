using CateringService.Domain.Entities.Approved;

namespace CateringService.Domain.Repositories;

public interface IUserRepository : IGenericRepository<User, Ulid>
{
    Task<Ulid> AddAsync(User user);
}