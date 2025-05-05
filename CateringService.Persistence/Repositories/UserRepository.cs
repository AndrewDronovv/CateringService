//using CateringService.Domain.Entities;
//using CateringService.Domain.Repositories;
//using Microsoft.EntityFrameworkCore;

//namespace CateringService.Persistence.Repositories;

//public class UserRepository(AppDbContext context) : IUserRepository
//{
//    public readonly AppDbContext _context = context ??
//        throw new ArgumentNullException(nameof(context));

//    public async Task AddAsync(User user)
//    {
//        await _context.Users.AddAsync(user);
//    }

//    public async Task<User> GetByEmailAsync(string email)
//    {
//        var userByEmail = await _context.Users
//            .FirstOrDefaultAsync(u => u.Email == email);
//        if (userByEmail is null)
//        {
//            throw new Exception("Пользователя с указанным email не существует");
//        }

//        return userByEmail;
//    }
//}