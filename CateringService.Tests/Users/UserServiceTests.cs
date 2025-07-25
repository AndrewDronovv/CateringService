using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.Services;
using CateringService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CateringService.Tests.Users;

public sealed class UserServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _unitOfWorkRepository = Substitute.For<IUnitOfWorkRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<UserService>>();

        _userService = new UserService(_userRepository, _unitOfWorkRepository, _mapper, _logger);
    }

    [Fact]
    public async Task Ctor_WhenUserRepositoryNull_ShoulThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new UserService(null!, _unitOfWorkRepository, _mapper, _logger));
        Assert.Equal("userRepository", exception.ParamName);
    }

    [Fact]
    public async Task Ctor_WhenUnitOfWorkRepositoryNull_ShoulThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new UserService(_userRepository, null!, _mapper, _logger));
        Assert.Equal("unitOfWorkRepository", exception.ParamName);
    }

    [Fact]
    public async Task Ctor_WhenMapperNull_ShoulThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new UserService(_userRepository, _unitOfWorkRepository, null!, _logger));
        Assert.Equal("mapper", exception.ParamName);
    }

    [Fact]
    public async Task Ctor_WhenLoggerNull_ShoulThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new UserService(_userRepository, _unitOfWorkRepository, _mapper, null!));
        Assert.Equal("logger", exception.ParamName);
    }
}
