using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface IUserRepository
{
    Task<User> GetByEmailAsync(string email);
    Task AddAsync(User user);
}