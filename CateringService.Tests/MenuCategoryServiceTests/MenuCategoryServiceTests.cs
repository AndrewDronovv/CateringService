using AutoMapper;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Application.Services;
using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CateringService.Tests.MenuCategories;

public class MenuCategoryServiceTests
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
    public void Ctor_MenuCategoryNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new MenuCategoryService(null, _supplierRepositoryMock, _unitOfWorkRepositoryMock, _mapper, _logger));
        Assert.Contains("menuCategoryRepository", exception.Message);
    }

    [Fact]
    public void Ctor_SupplierRepositoryNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new MenuCategoryService(_menuCategoryRepositoryMock, null, _unitOfWorkRepositoryMock, _mapper, _logger));
        Assert.Contains("supplierRepository", exception.Message);
    }

    [Fact]
    public void Ctor_UnitOfWorkNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new MenuCategoryService(_menuCategoryRepositoryMock, _supplierRepositoryMock, null, _mapper, _logger));
        Assert.Contains("unitOfWorkRepository", exception.Message);
    }

    [Fact]
    public void Ctor_MapperNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new MenuCategoryService(_menuCategoryRepositoryMock, _supplierRepositoryMock, _unitOfWorkRepositoryMock, null, _logger));
        Assert.Contains("mapper", exception.Message);
    }

    [Fact]
    public void Ctor_LoggerNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new MenuCategoryService(_menuCategoryRepositoryMock, _supplierRepositoryMock, _unitOfWorkRepositoryMock, _mapper, null));
        Assert.Contains("logger", exception.Message);
    }

    [Fact]
    public void Ctor_AllParameters_CreateNewInstance()
    {
        var menuCategoryService = new MenuCategoryService(_menuCategoryRepositoryMock, _supplierRepositoryMock, _unitOfWorkRepositoryMock, _mapper, _logger);
        Assert.NotNull(menuCategoryService);
    }
    #endregion

    #region Тесты добавления
    [Fact]
    public async Task CreateMenuCategoryAsync_NewMenuCategory_ReturnMenuCategory()
    {
        //Arrange
        AddMenuCategoryRequest request = new AddMenuCategoryRequest
        {
            Name = "Test Menu",
            SupplierId = Ulid.NewUlid(),
            Description = "Test Description"
        };
        MenuCategoryViewModel menuCategoryViewModel = new MenuCategoryViewModel();
        MenuCategory menuCategory = new MenuCategory();
    }
    #endregion
}