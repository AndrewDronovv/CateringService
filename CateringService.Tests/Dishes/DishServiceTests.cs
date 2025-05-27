//using CateringService.Application.Services;
//using CateringService.Domain.Abstractions;
//using CateringService.Domain.Entities.Approved;
//using CateringService.Domain.Exceptions;
//using CateringService.Domain.Repositories;
//using NSubstitute;

//namespace CateringService.Tests.Dishes;

//public class DishServiceTests
//{
//    private readonly IDishRepository _dishRepositoryMock;
//    private readonly IUnitOfWorkRepository _unitOfWorkMock;
//    private readonly IDishService _dishService;

//    public DishServiceTests()
//    {
//        _dishRepositoryMock = Substitute.For<IDishRepository>();
//        _unitOfWorkMock = Substitute.For<IUnitOfWorkRepository>();
//        _dishService = new DishService(_dishRepositoryMock, _unitOfWorkMock);
//    }

//    #region Тесты конструктора
//    [Fact]
//    public void Ctor_DishRepositoryNull_ThrowsArgumentNullException()
//    {
//        var exception = Assert.Throws<ArgumentNullException>(() => new DishService(null, _unitOfWorkMock));
//        Assert.Contains("repository", exception.Message);
//    }

//    [Fact]
//    public void Ctor_UnitOfWorkNull_ThrowsArgumentNullException()
//    {
//        var exception = Assert.Throws<ArgumentNullException>(() => new DishService(_dishRepositoryMock, null));
//        Assert.Contains("unitOfWork", exception.Message);
//    }

//    [Fact]
//    public void Ctor_AllParameters_CreatesNewInstance()
//    {
//        var dishService = new DishService(_dishRepositoryMock, _unitOfWorkMock);
//        Assert.NotNull(dishService);
//    }
//    #endregion 

//    #region Тесты добавления
//    [Fact]
//    public async Task AddAsync_NewDish_ReturnsDish()
//    {
//        //Arrange
//        Dish dish = new Dish { Id = Ulid.NewUlid() };
//        _dishRepositoryMock.Add(dish).Returns(dish.Id);
//        _dishRepositoryMock.GetByIdAsync(dish.Id).Returns(Task.FromResult<Dish?>(dish));

//        //Act
//        var result = await _dishService.AddAsync(dish);

//        //Assert
//        Assert.NotNull(result);
//        Assert.NotEqual(Ulid.Empty, result.Id);
//        Assert.Equal(dish.Id, result.Id);
//        _dishRepositoryMock.Received(1).Add(dish);
//        await _unitOfWorkMock.Received(1).SaveChangesAsync();
//    }
//    #endregion

//    #region Тесты удаления
//    [Fact]
//    public async Task DeleteAsync_ExistingDish_DeletesDish()
//    {
//        //Arrange
//        Dish dish = new Dish { Id = Ulid.NewUlid() };
//        _dishRepositoryMock.GetByIdAsync(dish.Id).Returns(Task.FromResult<Dish?>(dish));

//        //Act
//        await _dishService.DeleteAsync(dish.Id);

//        //Assert
//        _dishRepositoryMock.Received(1).Delete(dish);
//        await _unitOfWorkMock.Received(1).SaveChangesAsync();
//    }

//    [Fact]
//    public async Task DeleteAsync_NotExistingDish_ThrowsKeyNotFoundException()
//    {
//        //Arrange
//        Dish dish = new Dish { Id = Ulid.NewUlid() };
//        _dishRepositoryMock.GetByIdAsync(dish.Id).Returns(Task.FromResult<Dish?>(null));

//        //Act & Assert
//        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _dishService.DeleteAsync(dish.Id));
//        Assert.Contains($"Сущность с Id = {dish.Id} не найдена.", exception.Message);
//    }
//    #endregion

//    #region Тесты получения сущности по Id
//    [Fact]
//    public async Task GetByIdAsync_ExistingDish_ReturnsDish()
//    {
//        //Arrange
//        Dish dish = new Dish { Id = Ulid.NewUlid() };
//        _dishRepositoryMock.GetByIdAsync(dish.Id, false).Returns(Task.FromResult<Dish?>(dish));

//        //Act
//        var result = await _dishService.GetByIdAsync(dish.Id);

//        //Assert
//        Assert.NotNull(result);
//        Assert.Equal(dish.Id, result.Id);
//        await _dishRepositoryMock.Received(1).GetByIdAsync(dish.Id, false);
//    }

//    [Fact]
//    public async Task GetByIdAsync_NotExistingDish_ThrowsNotFoundException()
//    {
//        //Arrange
//        Dish dish = new Dish { Id = Ulid.NewUlid() };
//        _dishRepositoryMock.GetByIdAsync(dish.Id, false).Returns(Task.FromResult<Dish?>(null));

//        //Act & Assert
//        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _dishService.GetByIdAsync(dish.Id));
//        Assert.Contains(typeof(Dish).Name, exception.Message);
//        Assert.Contains(dish.Id.ToString(), exception.Message);
//    }
//    #endregion

//    #region Тесты получения всех сущностей
//    [Fact]
//    public async Task GetAllAsync_ReturnsDishList()
//    {
//        //Arrange
//        var dishes = new List<Dish>
//        {
//            new Dish {Id = Ulid.NewUlid(), Name = "Pizza" },
//            new Dish {Id = Ulid.NewUlid(), Name = "Pasta"}
//        };

//        _dishRepositoryMock.GetAllAsync().Returns(Task.FromResult<IEnumerable<Dish>>(dishes));

//        //Act
//        var result = await _dishService.GetAllAsync();

//        //Assert
//        Assert.NotNull(result);
//        Assert.Equal(2, dishes.Count);
//        Assert.Contains(result, d => d.Name == "Pizza");
//        Assert.Contains(result, d => d.Name == "Pasta");
//        Assert.All(result, dish => Assert.NotNull(dish.Name));
//        await _dishRepositoryMock.Received(1).GetAllAsync();
//    }

//    [Fact]
//    public async Task GetAllAsync_WhenNoDishes_ReturnsEmptyList()
//    {
//        //Arrange
//        _dishRepositoryMock.GetAllAsync().Returns(Task.FromResult(Enumerable.Empty<Dish>()));

//        //Act
//        var result = await _dishService.GetAllAsync();

//        //Assert
//        Assert.NotNull(result);
//        Assert.Empty(result);
//    }
//    #endregion

//    #region Тесты обновления
//    [Fact]
//    public async Task UpdateAsync_ShouldUpdateEntity_WhenEntityExists()
//    {
//        //Arrange
//        Dish oldDish = new Dish { Id = Ulid.NewUlid(), Name = "Old name" };
//        Dish updatedDish = new Dish { Id = oldDish.Id, Name = "New name" };
//        _dishRepositoryMock.GetByIdAsync(oldDish.Id).Returns(Task.FromResult<Dish?>(oldDish));
//        _dishRepositoryMock.Update(updatedDish).Returns(updatedDish.Id);

//        //Act
//        var result = await _dishService.UpdateAsync(oldDish.Id, updatedDish);

//        //Assert
//        Assert.NotNull(result);
//        Assert.Equal(oldDish.Id, result.Id);
//        Assert.Equal("New name", result.Name);
//        await _unitOfWorkMock.Received(1).SaveChangesAsync();
//    }

//    [Fact]
//    public async Task UpdateAsync_NotExistingDish_ThrowsKeyNotFoundException()
//    {
//        //Arrange
//        Dish oldDish = new Dish { Id = Ulid.NewUlid() };
//        Dish newDish = new Dish { Id = oldDish.Id };
//        _dishRepositoryMock.GetByIdAsync(oldDish.Id).Returns(Task.FromResult<Dish?>(null));

//        //Act & Assert
//        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _dishService.UpdateAsync(oldDish.Id, newDish));
//        Assert.Contains($"Сущность с ключом {oldDish.Id} не найдена", exception.Message);
//    }
//    #endregion

//    #region Тесты переключения доступности
//    [Fact]
//    public async Task ToggleDishState_ExistingDish_ChangesState()
//    {
//        //Arrange
//        Dish dish = new Dish { Id = Ulid.NewUlid(), IsAvailable = true };
//        _dishRepositoryMock.GetByIdAsync(dish.Id, false).Returns(Task.FromResult<Dish?>(dish));
//        _dishRepositoryMock.ToggleState(dish).Returns(true);

//        //Act
//        var result = await _dishService.ToggleDishStateAsync(dish.Id);

//        //Assert
//        Assert.True(result);
//        Assert.False(dish.IsAvailable);
//        await _unitOfWorkMock.Received(1).SaveChangesAsync();
//    }

//    [Fact]
//    public async Task ToggleDishState_NotExistingDish_ThrowsKeyNotFoundException()
//    {
//        //Arrange
//        Dish dish = new Dish { Id = Ulid.NewUlid(), IsAvailable = true };
//        _dishRepositoryMock.GetByIdAsync(dish.Id).Returns(Task.FromResult<Dish?>(null));

//        //Act & Assert
//        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _dishService.ToggleDishStateAsync(dish.Id));
//        Assert.Contains($"Блюдо с Id {dish.Id} не найдено.", exception.Message);
//    }
//    #endregion

//    #region Тесты существования категории меню
//    [Fact]
//    public void CheckMenuCategoryExists_ReturnsTrue_WhenCategoryExists()
//    {
//        //Arrange
//        var categoryId = Ulid.NewUlid();
//        _dishRepositoryMock.CheckMenuCategoryExists(categoryId).Returns(true);

//        //Act
//        var result = _dishService.CheckMenuCategoryExists(categoryId);

//        //Assert
//        Assert.True(result);
//    }

//    [Fact]
//    public void CheckMenuCategoryExists_ReturnsFalse_WhenCategoryNotExists()
//    {
//        //Arrange
//        var categoryId = Ulid.NewUlid();
//        _dishRepositoryMock.CheckMenuCategoryExists(categoryId).Returns(false);

//        //Act
//        var result = _dishService.CheckMenuCategoryExists(categoryId);

//        //Assert
//        Assert.False(result);
//    }

//    [Fact]
//    public void CheckMenuCategoryExists_ThrowsArgumentException_WhenCategoryIdIsEmpty()
//    {
//        //Arrange
//        var categoryId = Ulid.Empty;

//        //Act & Assert
//        Assert.Throws<ArgumentException>(() => _dishService.CheckMenuCategoryExists(categoryId));
//        Assert.Equal(Ulid.Empty, categoryId);
//    }
//    #endregion

//    #region Тесты поставщика
//    [Fact]
//    public void CheckSupplierExists_ReturnsTrue_WhenSupplierExists()
//    {
//        //Arrange
//        var supplierId = Ulid.NewUlid();
//        _dishRepositoryMock.CheckSupplierExists(supplierId).Returns(true);

//        //Act
//        var result = _dishService.CheckSupplierExists(supplierId);

//        //Assert
//        Assert.True(result);
//    }

//    [Fact]
//    public void CheckSupplierExists_ReturnsFalse_WhenSupplierNotExists()
//    {
//        //Arrange
//        var supplierId = Ulid.NewUlid();
//        _dishRepositoryMock.CheckSupplierExists(supplierId).Returns(false);

//        //Act
//        var result = _dishService.CheckSupplierExists(supplierId);

//        //Assert
//        Assert.False(result);
//    }

//    [Fact]
//    public void CheckSupplierExists_ThrowsArgumentException_WhenSupplierIdIsEmpty()
//    {
//        //Arrange
//        var supplierId = Ulid.Empty;

//        //Act & Assert
//        Assert.Throws<ArgumentException>(() => _dishService.CheckSupplierExists(supplierId));
//        Assert.Equal(Ulid.Empty, supplierId);
//    }
//    #endregion
//}