using AutoMapper;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Application.Services;
using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Exceptions;
using CateringService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CateringService.Tests.MenuCategories;

public sealed class MenuCategoryServiceTests
{
    private readonly IMenuCategoryRepository _menuCategoryRepositoryMock;
    private readonly ISupplierRepository _supplierRepositoryMock;
    private readonly IUnitOfWorkRepository _unitOfWorkRepositoryMock;
    private readonly IMapper _mapper;
    private readonly ILogger<MenuCategoryService> _logger;
    private readonly IMenuCategoryService _menuCategoryService;

    public MenuCategoryServiceTests()
    {
        _menuCategoryRepositoryMock = Substitute.For<IMenuCategoryRepository>();
        _supplierRepositoryMock = Substitute.For<ISupplierRepository>();
        _unitOfWorkRepositoryMock = Substitute.For<IUnitOfWorkRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<MenuCategoryService>>();

        _menuCategoryService = new MenuCategoryService(_menuCategoryRepositoryMock, _supplierRepositoryMock, _unitOfWorkRepositoryMock, _mapper, _logger);
    }

    #region Тесты конструктора
    [Fact]
    public void Ctor_WhenMenuCategoryNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new MenuCategoryService(null, _supplierRepositoryMock, _unitOfWorkRepositoryMock, _mapper, _logger));
        Assert.Contains("menuCategoryRepository", exception.Message);
    }

    [Fact]
    public void Ctor_WhenSupplierRepositoryNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new MenuCategoryService(_menuCategoryRepositoryMock, null, _unitOfWorkRepositoryMock, _mapper, _logger));
        Assert.Contains("supplierRepository", exception.Message);
    }

    [Fact]
    public void Ctor_WhenUnitOfWorkNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new MenuCategoryService(_menuCategoryRepositoryMock, _supplierRepositoryMock, null, _mapper, _logger));
        Assert.Contains("unitOfWorkRepository", exception.Message);
    }

    [Fact]
    public void Ctor_WhenMapperNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new MenuCategoryService(_menuCategoryRepositoryMock, _supplierRepositoryMock, _unitOfWorkRepositoryMock, null, _logger));
        Assert.Contains("mapper", exception.Message);
    }

    [Fact]
    public void Ctor_WhenLoggerNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new MenuCategoryService(_menuCategoryRepositoryMock, _supplierRepositoryMock, _unitOfWorkRepositoryMock, _mapper, null));
        Assert.Contains("logger", exception.Message);
    }

    [Fact]
    public void Ctor_WhenAllParameters_ShouldCreateNewInstance()
    {
        var menuCategoryService = new MenuCategoryService(_menuCategoryRepositoryMock, _supplierRepositoryMock, _unitOfWorkRepositoryMock, _mapper, _logger);
        Assert.NotNull(menuCategoryService);
    }
    #endregion

    #region Тесты создания MenuCategory
    [Fact]
    public async Task CreateMenuCategoryAsync_WhenNewMenuCategory_ShouldReturnMenuCategory()
    {
        //Arrange
        Ulid menuCategoryId = Ulid.NewUlid();
        AddMenuCategoryRequest request = new AddMenuCategoryRequest
        {
            Name = "Test MenuCategory",
            SupplierId = Ulid.NewUlid(),
            Description = "Test Description"
        };
        MenuCategoryViewModel menuCategoryViewModel = new MenuCategoryViewModel { Id = menuCategoryId, Name = request.Name, Description = request.Description };
        MenuCategory menuCategory = new MenuCategory { Id = menuCategoryId, Name = request.Name, Description = request.Description };

        _supplierRepositoryMock.CheckSupplierExists(request.SupplierId).Returns(true);
        _mapper.Map<MenuCategory>(request).Returns(menuCategory);
        _menuCategoryRepositoryMock.Add(menuCategory).Returns(menuCategory.Id);
        _menuCategoryRepositoryMock.GetByIdAsync(menuCategory.Id).Returns(Task.FromResult<MenuCategory?>(menuCategory));
        _mapper.Map<MenuCategoryViewModel>(menuCategory).Returns(menuCategoryViewModel);

        //Act
        var result = await _menuCategoryService.CreateMenuCategoryAsync(request);

        //Assert
        Assert.NotNull(result);
        Assert.NotEqual(Ulid.Empty, result.Id);
        Assert.Equal(menuCategoryId, result.Id);
        Assert.Equal("Test MenuCategory", result.Name);
        _menuCategoryRepositoryMock.Received(1).Add(Arg.Any<MenuCategory>());
        await _unitOfWorkRepositoryMock.Received(1).SaveChangesAsync();
        _mapper.Received(1).Map<MenuCategoryViewModel>(menuCategory);
    }

    [Fact]
    public async Task CreateMenuCategoryAsync_WhenSupplierDoesNotExist_ShouldThrowNotFoundException()
    {
        var request = new AddMenuCategoryRequest
        {
            Name = "Test MenuCategory",
            SupplierId = Ulid.NewUlid()
        };

        _supplierRepositoryMock.CheckSupplierExists(request.SupplierId).Returns(false);

        //Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _menuCategoryService.CreateMenuCategoryAsync(request));

        Assert.Contains(nameof(Supplier), exception.Message);
        Assert.Contains(request.SupplierId.ToString(), exception.Message);
    }

    [Fact]
    public async Task CreateMenuCategoryAsync_WhenAddMenuCategoryRequestIsNull_ShouldThrowArgumentNullException()
    {
        //Arrange
        AddMenuCategoryRequest? request = null;

        //Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _menuCategoryService.CreateMenuCategoryAsync(request));
        Assert.Contains(nameof(request), exception.Message);
    }
    #endregion
}