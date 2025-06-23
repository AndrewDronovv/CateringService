using CateringService.Domain.Entities.Approved;

namespace CateringService.Domain.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user);
}