using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Domain.Entities;
using CateringService.Domain.Exceptions;
using CateringService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace CateringService.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper, ILogger<UserService> logger)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _unitOfWorkRepository = unitOfWorkRepository ?? throw new ArgumentNullException(nameof(unitOfWorkRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<UserViewModel?> CreateUserAsync(AddUserRequest request)
    {
        if (request is null)
        {
            _logger.LogWarning("Входные данные не указаны.");
            throw new ArgumentNullException(nameof(request), "User request is null.");
        }

        _logger.LogInformation("Получен запрос на создание пользователя.");

        User user = new User();

        user = request.UserType.ToLower() switch
        {
            "customer" => _mapper.Map<Customer>(request) ?? throw new InvalidOperationException("Customer mapping failed."),
            "supplier" => _mapper.Map<Supplier>(request) ?? throw new InvalidOperationException("Supplier mapping failed."),
            "broker" => _mapper.Map<Broker>(request) ?? throw new InvalidOperationException("Broker mapping failed."),
            _ => throw new ArgumentOutOfRangeException(nameof(request.UserType), $"Unknown user's type {request.UserType}")
        };

        var userId = _userRepository.Add(user);
        await _unitOfWorkRepository.SaveChangesAsync();

        var createdUser = await _userRepository.GetByIdAsync(userId);
        if (createdUser is null)
        {
            _logger.LogWarning("Ошибка получения пользователя {UserId}.", userId);
            throw new NotFoundException(nameof(User), userId.ToString());
        }

        _logger.LogInformation("Пользователь {CreatedUser.Name} с Id {UserId} успешно создан.", createdUser.LastName, userId);

        return _mapper.Map<UserViewModel>(createdUser);
    }

    public async Task DeleteUserAsync(Ulid userId)
    {
        if (userId == Ulid.Empty)
        {
            _logger.LogWarning("UserId не должен быть пустым.");
            throw new ArgumentException(nameof(userId), "UserId is empty.");
        }

        _logger.LogInformation("Получен запрос на удаление пользователя {UserId}.", userId);

        await _userRepository.DeleteAsync(userId);
        await _unitOfWorkRepository.SaveChangesAsync();

        _logger.LogInformation("Пользователь {UserId} успешно удален.", userId);
    }
}