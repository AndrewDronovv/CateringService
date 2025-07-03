using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Application.Services;
using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Exceptions;
using CateringService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CateringService.Tests.Dishes;

public class DishServiceTests
{
    private readonly IDishRepository _dishRepositoryMock;
    private readonly ISupplierRepository _supplierRepositoryMock;
    private readonly IMenuCategoryRepository _menuCategoryRepositoryMock;
    private readonly IUnitOfWorkRepository _unitOfWorkMock;
    private readonly IMapper _mapper;
    private readonly ILogger<DishService> _logger;
    private readonly IDishService _dishService;
    private readonly ISlugService _slugService;

    public DishServiceTests()
    {
        _dishRepositoryMock = Substitute.For<IDishRepository>();
        _supplierRepositoryMock = Substitute.For<ISupplierRepository>();
        _menuCategoryRepositoryMock = Substitute.For<IMenuCategoryRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWorkRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<DishService>>();
        _slugService = Substitute.For<ISlugService>();

        _dishService = new DishService(_dishRepositoryMock, _supplierRepositoryMock, _unitOfWorkMock, _mapper, _logger, _menuCategoryRepositoryMock, _slugService);
    }

    #region Тесты конструктора
    [Fact]
    public void Ctor_DishRepositoryNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new DishService(null, _supplierRepositoryMock, _unitOfWorkMock, _mapper, _logger, _menuCategoryRepositoryMock, _slugService));
        Assert.Contains("dishRepository", exception.Message);
    }

    [Fact]
    public void Ctor_SupplierRepositoryNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new DishService(_dishRepositoryMock, null, _unitOfWorkMock, _mapper, _logger, _menuCategoryRepositoryMock, _slugService));
        Assert.Contains("supplierRepository", exception.Message);
    }

    [Fact]
    public void Ctor_UnitOfWorkNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new DishService(_dishRepositoryMock, _supplierRepositoryMock, null, _mapper, _logger, _menuCategoryRepositoryMock, _slugService));
        Assert.Contains("unitOfWorkRepository", exception.Message);
    }

    [Fact]
    public void Ctor_MapperNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new DishService(_dishRepositoryMock, _supplierRepositoryMock, _unitOfWorkMock, null, _logger, _menuCategoryRepositoryMock, _slugService));
        Assert.Contains("mapper", exception.Message);
    }

    [Fact]
    public void Ctor_LoggerNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new DishService(_dishRepositoryMock, _supplierRepositoryMock, _unitOfWorkMock, _mapper, null, _menuCategoryRepositoryMock, _slugService));
        Assert.Contains("logger", exception.Message);
    }

    [Fact]
    public void Ctor_MenuCategoryRepositoryNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new DishService(_dishRepositoryMock, _supplierRepositoryMock, _unitOfWorkMock, _mapper, _logger, null, _slugService));
        Assert.Contains("menuCategoryRepository", exception.Message);
    }

    [Fact]
    public void Ctor_AllParameters_CreatesNewInstance()
    {
        var dishService = new DishService(_dishRepositoryMock, _supplierRepositoryMock, _unitOfWorkMock, _mapper, _logger, _menuCategoryRepositoryMock, _slugService);
        Assert.NotNull(_dishService);
    }
    #endregion

    #region Тесты добавления
    [Fact]
    public async Task CreateDishAsync_NewDish_ReturnsDish()
    {
        //Arrange
        Ulid supplierId = Ulid.NewUlid();
        Ulid dishId = Ulid.NewUlid();
        AddDishRequest request = new AddDishRequest
        {
            Name = "Test Dish",
            MenuCategoryId = Ulid.NewUlid(),
            SupplierId = supplierId
        };
        Dish dish = new Dish { Id = dishId, Name = request.Name, SupplierId = supplierId };
        var viewModel = new DishViewModel { Id = dishId, Name = request.Name };

        _supplierRepositoryMock.CheckSupplierExists(supplierId).Returns(true);
        _menuCategoryRepositoryMock.ChechMenuCategoryExists(request.MenuCategoryId).Returns(true);

        _mapper.Map<Dish>(request).Returns(dish);
        _dishRepositoryMock.Add(dish).Returns(dish.Id);
        _dishRepositoryMock.GetByIdAsync(dish.Id).Returns(Task.FromResult<Dish?>(dish));
        _mapper.Map<DishViewModel>(dish).Returns(viewModel);

        //Act
        var result = await _dishService.CreateDishAsync(supplierId, request);

        //Assert
        Assert.NotNull(result);
        Assert.NotEqual(Ulid.Empty, result.Id);
        Assert.Equal(dishId, result.Id);
        Assert.Equal("Test Dish", result.Name);
        _dishRepositoryMock.Received(1).Add(Arg.Any<Dish>());
        await _unitOfWorkMock.Received(1).SaveChangesAsync();
        _mapper.Received(1).Map<DishViewModel>(dish);
    }

    [Fact]
    public async Task CreateDishAsync_SupplierDoesNotExist_ShouldThrowNotFoundException()
    {
        //Arrange
        var supplierId = Ulid.NewUlid();
        var request = new AddDishRequest
        {
            Name = "Test Dish",
            MenuCategoryId = Ulid.NewUlid(),
            SupplierId = supplierId
        };

        _supplierRepositoryMock.CheckSupplierExists(supplierId).Returns(false);

        //Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _dishService.CreateDishAsync(supplierId, request));

        Assert.Contains(nameof(Supplier), exception.Message);
        Assert.Contains(supplierId.ToString(), exception.Message);
    }

    [Fact]
    public async Task CreateDishAsync_MenuCategoryDoesNotExist_ShouldThrowNotFoundException()
    {
        //Arrange
        var supplierId = Ulid.NewUlid();
        var request = new AddDishRequest
        {
            Name = "Test Dish",
            MenuCategoryId = Ulid.NewUlid(),
            SupplierId = supplierId
        };

        _supplierRepositoryMock.CheckSupplierExists(supplierId).Returns(true);
        _menuCategoryRepositoryMock.ChechMenuCategoryExists(request.MenuCategoryId).Returns(false);

        //Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _dishService.CreateDishAsync(supplierId, request));

        Assert.Contains(nameof(MenuCategory), exception.Message);
        Assert.Contains(request.MenuCategoryId.ToString(), exception.Message);
    }

    [Fact]
    public async Task CreateDishAsync_WhenAddDishRequestIsNull_ShouldThrowArgumentNullException()
    {
        //Arrange
        var supplierId = Ulid.NewUlid();
        AddDishRequest? request = null;

        //Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _dishService.CreateDishAsync(supplierId, request));
        Assert.Contains(nameof(request), exception.Message);
    }
    [Fact]
    public async Task CreateDishAsync_WhenSupplierIdIsEmpty_ShouldThrowArgumentException()
    {
        //Arrange
        var supplierId = Ulid.Empty;
        AddDishRequest request = new AddDishRequest();

        //Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _dishService.CreateDishAsync(supplierId, request));
        Assert.Contains(nameof(supplierId), exception.Message);
    }
    #endregion

    #region Тесты получения сущности по Id
    [Fact]
    public async Task GetByIdAsync_DishExists_ReturnsDish()
    {
        //Arrange
        Ulid dishId = Ulid.NewUlid();
        Dish dish = new Dish { Id = dishId, Name = "Pizza" };
        DishViewModel dishViewModel = new DishViewModel { Id = dishId, Name = "Pizza" };

        _dishRepositoryMock
            .GetByIdAsync(dishId)
            .Returns(Task.FromResult<Dish?>(dish));

        _mapper
            .Map<DishViewModel>(dish)
            .Returns(dishViewModel);

        //Act
        var result = await _dishService.GetByIdAsync(dishId);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(dishId, result.Id);
        Assert.Equal("Pizza", result.Name);

        await _dishRepositoryMock.Received(1).GetByIdAsync(dishId);
        _mapper.Received(1).Map<DishViewModel>(dish);
    }

    [Fact]
    public async Task GetByIdAsync_DishDoesNotExist_ShouldThrowNotFoundException()
    {
        //Arrange
        Dish dish = new Dish { Id = Ulid.NewUlid() };
        _dishRepositoryMock.GetByIdAsync(dish.Id).Returns(Task.FromResult<Dish?>(null));

        //Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _dishService.GetByIdAsync(dish.Id));
        Assert.Contains(nameof(Dish), exception.Message);
        Assert.Contains(dish.Id.ToString(), exception.Message);
    }
    #endregion

    //#region Тесты удаления
    //[Fact]
    //public async Task DeleteAsync_ExistingDish_DeletesDish()
    //{
    //    //Arrange
    //    Dish dish = new Dish { Id = Ulid.NewUlid() };
    //    _dishRepositoryMock.GetByIdAsync(dish.Id).Returns(Task.FromResult<Dish?>(dish));

    //    //Act
    //    await _dishService.DeleteAsync(dish.Id);

    //    //Assert
    //    _dishRepositoryMock.Received(1).Delete(dish);
    //    await _unitOfWorkMock.Received(1).SaveChangesAsync();
    //}

    //[Fact]
    //public async Task DeleteAsync_NotExistingDish_ThrowsKeyNotFoundException()
    //{
    //    //Arrange
    //    Dish dish = new Dish { Id = Ulid.NewUlid() };
    //    _dishRepositoryMock.GetByIdAsync(dish.Id).Returns(Task.FromResult<Dish?>(null));

    //    //Act & Assert
    //    var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _dishService.DeleteAsync(dish.Id));
    //    Assert.Contains($"Сущность с Id = {dish.Id} не найдена.", exception.Message);
    //}
    //#endregion



    //#region Тесты получения всех сущностей
    //[Fact]
    //public async Task GetAllAsync_ReturnsDishList()
    //{
    //    //Arrange
    //    var dishes = new List<Dish>
    //    {
    //        new Dish {Id = Ulid.NewUlid(), Name = "Pizza" },
    //        new Dish {Id = Ulid.NewUlid(), Name = "Pasta"}
    //    };

    //    _dishRepositoryMock.GetAllAsync().Returns(Task.FromResult<IEnumerable<Dish>>(dishes));

    //    //Act
    //    var result = await _dishService.GetAllAsync();

    //    //Assert
    //    Assert.NotNull(result);
    //    Assert.Equal(2, dishes.Count);
    //    Assert.Contains(result, d => d.Name == "Pizza");
    //    Assert.Contains(result, d => d.Name == "Pasta");
    //    Assert.All(result, dish => Assert.NotNull(dish.Name));
    //    await _dishRepositoryMock.Received(1).GetAllAsync();
    //}

    //[Fact]
    //public async Task GetAllAsync_WhenNoDishes_ReturnsEmptyList()
    //{
    //    //Arrange
    //    _dishRepositoryMock.GetAllAsync().Returns(Task.FromResult(Enumerable.Empty<Dish>()));

    //    //Act
    //    var result = await _dishService.GetAllAsync();

    //    //Assert
    //    Assert.NotNull(result);
    //    Assert.Empty(result);
    //}
    //#endregion

    //#region Тесты обновления
    //[Fact]
    //public async Task UpdateAsync_ShouldUpdateEntity_WhenEntityExists()
    //{
    //    //Arrange
    //    Dish oldDish = new Dish { Id = Ulid.NewUlid(), Name = "Old name" };
    //    Dish updatedDish = new Dish { Id = oldDish.Id, Name = "New name" };
    //    _dishRepositoryMock.GetByIdAsync(oldDish.Id).Returns(Task.FromResult<Dish?>(oldDish));
    //    _dishRepositoryMock.Update(updatedDish).Returns(updatedDish.Id);

    //    //Act
    //    var result = await _dishService.UpdateAsync(oldDish.Id, updatedDish);

    //    //Assert
    //    Assert.NotNull(result);
    //    Assert.Equal(oldDish.Id, result.Id);
    //    Assert.Equal("New name", result.Name);
    //    await _unitOfWorkMock.Received(1).SaveChangesAsync();
    //}

    //[Fact]
    //public async Task UpdateAsync_NotExistingDish_ThrowsKeyNotFoundException()
    //{
    //    //Arrange
    //    Dish oldDish = new Dish { Id = Ulid.NewUlid() };
    //    Dish newDish = new Dish { Id = oldDish.Id };
    //    _dishRepositoryMock.GetByIdAsync(oldDish.Id).Returns(Task.FromResult<Dish?>(null));

    //    //Act & Assert
    //    var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _dishService.UpdateAsync(oldDish.Id, newDish));
    //    Assert.Contains($"Сущность с ключом {oldDish.Id} не найдена", exception.Message);
    //}
    //#endregion

    //#region Тесты переключения доступности
    //[Fact]
    //public async Task ToggleDishState_ExistingDish_ChangesState()
    //{
    //    //Arrange
    //    Dish dish = new Dish { Id = Ulid.NewUlid(), IsAvailable = true };
    //    _dishRepositoryMock.GetByIdAsync(dish.Id, false).Returns(Task.FromResult<Dish?>(dish));
    //    _dishRepositoryMock.ToggleState(dish).Returns(true);

    //    //Act
    //    var result = await _dishService.ToggleDishStateAsync(dish.Id);

    //    //Assert
    //    Assert.True(result);
    //    Assert.False(dish.IsAvailable);
    //    await _unitOfWorkMock.Received(1).SaveChangesAsync();
    //}

    //[Fact]
    //public async Task ToggleDishState_NotExistingDish_ThrowsKeyNotFoundException()
    //{
    //    //Arrange
    //    Dish dish = new Dish { Id = Ulid.NewUlid(), IsAvailable = true };
    //    _dishRepositoryMock.GetByIdAsync(dish.Id).Returns(Task.FromResult<Dish?>(null));

    //    //Act & Assert
    //    var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _dishService.ToggleDishStateAsync(dish.Id));
    //    Assert.Contains($"Блюдо с Id {dish.Id} не найдено.", exception.Message);
    //}
    //#endregion

    //#region Тесты существования категории меню
    //[Fact]
    //public void CheckMenuCategoryExists_ReturnsTrue_WhenCategoryExists()
    //{
    //    //Arrange
    //    var categoryId = Ulid.NewUlid();
    //    _dishRepositoryMock.CheckMenuCategoryExists(categoryId).Returns(true);

    //    //Act
    //    var result = _dishService.CheckMenuCategoryExists(categoryId);

    //    //Assert
    //    Assert.True(result);
    //}

    //[Fact]
    //public void CheckMenuCategoryExists_ReturnsFalse_WhenCategoryNotExists()
    //{
    //    //Arrange
    //    var categoryId = Ulid.NewUlid();
    //    _dishRepositoryMock.CheckMenuCategoryExists(categoryId).Returns(false);

    //    //Act
    //    var result = _dishService.CheckMenuCategoryExists(categoryId);

    //    //Assert
    //    Assert.False(result);
    //}

    //[Fact]
    //public void CheckMenuCategoryExists_ThrowsArgumentException_WhenCategoryIdIsEmpty()
    //{
    //    //Arrange
    //    var categoryId = Ulid.Empty;

    //    //Act & Assert
    //    Assert.Throws<ArgumentException>(() => _dishService.CheckMenuCategoryExists(categoryId));
    //    Assert.Equal(Ulid.Empty, categoryId);
    //}
    //#endregion

    //#region Тесты поставщика
    //[Fact]
    //public void CheckSupplierExists_ReturnsTrue_WhenSupplierExists()
    //{
    //    //Arrange
    //    var supplierId = Ulid.NewUlid();
    //    _dishRepositoryMock.CheckSupplierExists(supplierId).Returns(true);

    //    //Act
    //    var result = _dishService.CheckSupplierExists(supplierId);

    //    //Assert
    //    Assert.True(result);
    //}

    //[Fact]
    //public void CheckSupplierExists_ReturnsFalse_WhenSupplierNotExists()
    //{
    //    //Arrange
    //    var supplierId = Ulid.NewUlid();
    //    _dishRepositoryMock.CheckSupplierExists(supplierId).Returns(false);

    //    //Act
    //    var result = _dishService.CheckSupplierExists(supplierId);

    //    //Assert
    //    Assert.False(result);
    //}

    //[Fact]
    //public void CheckSupplierExists_ThrowsArgumentException_WhenSupplierIdIsEmpty()
    //{
    //    //Arrange
    //    var supplierId = Ulid.Empty;

    //    //Act & Assert
    //    Assert.Throws<ArgumentException>(() => _dishService.CheckSupplierExists(supplierId));
    //    Assert.Equal(Ulid.Empty, supplierId);
    //}
    //#endregion
}