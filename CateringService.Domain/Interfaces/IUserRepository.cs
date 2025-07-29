using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface IUserRepository : IGenericRepository<User, Ulid>
{
    Task DeleteAsync(Ulid userId);
}