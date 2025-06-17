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

    public async Task<bool> RegisterAsync(UserRegister input)
    {
        var existingUser = await _userRepository.GetByLoginAsync(input.Login);

        if (existingUser != null)
        {
            return false;
        }

        var user = new User
        {
            Login = input.Login,
            Password = BCrypt.Net.BCrypt.HashPassword(input.Password)
        };

        await _userRepository.AddAsync(user);

        return true;
    }
}