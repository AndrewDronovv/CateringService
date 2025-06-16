using CateringService.Application.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;
using Microsoft.Extensions.Configuration;

namespace CateringService.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public Task<bool> RegisterAsync(UserRegister input)
    {
        throw new NotImplementedException();
    }
}