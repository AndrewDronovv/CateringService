//using CateringService.Application.Abstractions;
//using CateringService.Domain.Entities;
//using CateringService.Domain.Enums;
//using CateringService.Domain.Repositories;
//using Microsoft.Extensions.Configuration;

//namespace CateringService.Application.Services;

//public class UserService : IUserService
//{
//    private readonly IUserRepository _userRepository;
//    private readonly IConfiguration _configuration;

//    public UserService(IUserRepository userRepository, IConfiguration configuration)
//    {
//        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
//        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
//    }

//    public async Task<bool> RegisterAsync(User input, Role role)
//    {
//        var existingUser = await _userRepository.GetByEmailAsync(input.Email);

//        if (existingUser != null)
//        {
//            return false;
//        }

//        //var user = new User
//        //{
//        //    Id = Ulid.NewUlid(),
//        //    Email = input.Email,
//        //    PasswordHash = BCrypt.Net.BCrypt.HashPassword(input.PasswordHash),
//        //    Role = role,
//        //    CreatedAt = DateTime.UtcNow,
//        //    IsBlocked = input.IsBlocked,
//        //};

//        //await _userRepository.AddAsync(user);
//        return true;
//    }
//}
