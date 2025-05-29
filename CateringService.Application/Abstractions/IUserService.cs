using CateringService.Domain.Entities;
using CateringService.Domain.Enums;

namespace CateringService.Application.Abstractions;

public interface IUserService
{
    Task<bool> RegisterAsync(User user, Role role);
}