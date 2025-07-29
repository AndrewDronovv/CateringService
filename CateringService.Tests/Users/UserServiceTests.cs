using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.Services;
using CateringService.Domain.Entities;
using CateringService.Domain.Exceptions;
using CateringService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CateringService.Tests.Users;

public sealed class UserServiceTests
{
    private readonly IUserRepository _userRepositoryMock;
    private readonly IUnitOfWorkRepository _unitOfWorkRepositoryMock;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _unitOfWorkRepositoryMock = Substitute.For<IUnitOfWorkRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<UserService>>();

        _userService = new UserService(_userRepositoryMock, _unitOfWorkRepositoryMock, _mapper, _logger);
    }

    #region Тесты конструктора
    [Fact]
    public async Task Ctor_WhenUserRepositoryNull_ShoulThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new UserService(null!, _unitOfWorkRepositoryMock, _mapper, _logger));
        Assert.Equal("userRepository", exception.ParamName);
    }

    [Fact]
    public async Task Ctor_WhenUnitOfWorkRepositoryNull_ShoulThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new UserService(_userRepositoryMock, null!, _mapper, _logger));
        Assert.Equal("unitOfWorkRepository", exception.ParamName);
    }

    [Fact]
    public async Task Ctor_WhenMapperNull_ShoulThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new UserService(_userRepositoryMock, _unitOfWorkRepositoryMock, null!, _logger));
        Assert.Equal("mapper", exception.ParamName);
    }

    [Fact]
    public async Task Ctor_WhenLoggerNull_ShoulThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new UserService(_userRepositoryMock, _unitOfWorkRepositoryMock, _mapper, null!));
        Assert.Equal("logger", exception.ParamName);
    }

    [Fact]
    public async Task CreateUserAsync_WhenAddUserRequestIsNull_ShouldThrowArgumentNullException()
    {
        //Arrange
        AddUserRequest? request = null;

        //Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.CreateUserAsync(request));
        Assert.Equal(nameof(request), exception.ParamName);
    }
    #endregion

    #region Тесты создания User
    [Fact]
    public async Task CreateUserAsync_WhenCreatedUserIsNull_ShouldThrowNotFoundException()
    {
        //Arrange
        Ulid userId = Ulid.NewUlid();
        AddUserRequest request = new AddUserRequest { FirstName = "Test", UserType = "customer" };
        Customer customer = new Customer { Id = userId, FirstName = request.FirstName };

        _mapper.Map<Customer>(request).Returns(customer);
        _userRepositoryMock.AddAsync(customer).Returns(userId);
        _userRepositoryMock.GetByIdAsync(userId).Returns(Task.FromResult<User?>(null));

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _userService.CreateUserAsync(request));

        //Assert
        Assert.Contains(nameof(User), exception.Message);
        Assert.Contains(userId.ToString(), exception.Message);
        Assert.IsType<NotFoundException>(exception);
    }
    #endregion
}