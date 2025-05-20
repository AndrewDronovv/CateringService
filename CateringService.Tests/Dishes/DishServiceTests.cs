using CateringService.Application.Services;
using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Exceptions;
using CateringService.Domain.Repositories;
using NSubstitute;

namespace CateringService.Tests.Dishes;

public class DishServiceTests
{
    private readonly IDishRepository _dishRepositorySubstitute;
    private readonly IUnitOfWorkRepository _unitOfWorkSubstitute;
    private readonly IDishService _dishService;

    public DishServiceTests()
    {
        _dishRepositorySubstitute = Substitute.For<IDishRepository>();
        _unitOfWorkSubstitute = Substitute.For<IUnitOfWorkRepository>();
        _dishService = new DishService(_dishRepositorySubstitute, _unitOfWorkSubstitute);
    }

    [Fact]
    public void Ctor_DishRepositoryNull_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new DishService(null, _unitOfWorkSubstitute));
        Assert.Contains("repository", exception.Message);
    }

    [Fact]
    public void Ctor_UnitOfWorkNull_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new DishService(_dishRepositorySubstitute, null));
        Assert.Contains("unitOfWork", exception.Message);
    }

    [Fact]
    public void Ctor_AllParameters_CreatesNewInstance()
    {
        var dishService = new DishService(_dishRepositorySubstitute, _unitOfWorkSubstitute);
        Assert.NotNull(dishService);
    }

    [Fact]
    public async Task AddAsync_NewDish_ReturnsDish()
    {
        //Arrange
        Dish dish = new Dish { Id = Ulid.NewUlid() };
        _dishRepositorySubstitute.Add(dish).Returns(dish.Id);
        _dishRepositorySubstitute.GetByIdAsync(dish.Id).Returns(Task.FromResult<Dish?>(dish));

        //Act
        var result = await _dishService.AddAsync(dish);

        //Assert
        Assert.NotNull(result);
        Assert.NotEqual(Ulid.Empty, result.Id);
        Assert.Equal(dish.Id, result.Id);
        _dishRepositorySubstitute.Received(1).Add(dish);
        await _unitOfWorkSubstitute.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteAsync_ExistingDish_DeletesDish()
    {
        //Arrange
        Dish dish = new Dish { Id = Ulid.NewUlid() };
        _dishRepositorySubstitute.GetByIdAsync(dish.Id).Returns(Task.FromResult<Dish?>(dish));

        //Act
        await _dishService.DeleteAsync(dish.Id);

        //Assert
        _dishRepositorySubstitute.Received(1).Delete(dish);
        await _unitOfWorkSubstitute.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteAsync_NotExistingDish_ThrowsKeyNotFoundException()
    {
        //Arrange
        Dish dish = new Dish { Id = Ulid.NewUlid() };
        _dishRepositorySubstitute.GetByIdAsync(dish.Id).Returns(Task.FromResult<Dish?>(null));

        //Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _dishService.DeleteAsync(dish.Id));
        Assert.Contains($"Сущность с Id = {dish.Id} не найдена.", exception.Message);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingDish_ReturnsDish()
    {
        //Arrange
        Dish dish = new Dish { Id = Ulid.NewUlid() };
        _dishRepositorySubstitute.GetByIdAsync(dish.Id, false).Returns(Task.FromResult<Dish?>(dish));

        //Act
        var result = await _dishService.GetByIdAsync(dish.Id);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(dish.Id, result.Id);
        await _dishRepositorySubstitute.Received(1).GetByIdAsync(dish.Id, false);
    }

    [Fact]
    public async Task GetByIdAsync_NotExistingDish_ThrowsNotFoundException()
    {
        //Arrange
        Dish dish = new Dish { Id = Ulid.NewUlid() };
        _dishRepositorySubstitute.GetByIdAsync(dish.Id, false).Returns(Task.FromResult<Dish?>(null));

        //Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _dishService.GetByIdAsync(dish.Id));
        Assert.Contains(typeof(Dish).Name, exception.Message);
        Assert.Contains(dish.Id.ToString(), exception.Message);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsDishList()
    {
        //Arrange
        var dishes = new List<Dish>
        {
            new Dish {Id = Ulid.NewUlid(), Name = "Pizza" },
            new Dish {Id = Ulid.NewUlid(), Name = "Pasta"}
        };

        _dishRepositorySubstitute.GetAllAsync().Returns(Task.FromResult<IEnumerable<Dish>>(dishes));

        //Act
        var result = await _dishService.GetAllAsync();

        //Assert
        Assert.NotNull(result);
        Assert.Equal(2, dishes.Count);
        Assert.Contains(result, d => d.Name == "Pizza");
        Assert.Contains(result, d => d.Name == "Pasta");
        Assert.All(result, dish => Assert.NotNull(dish.Name));
        await _dishRepositorySubstitute.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task GetAllAsync_WhenNoDishes_ReturnsEmptyList()
    {
        //Arrange
        _dishRepositorySubstitute.GetAllAsync().Returns(Task.FromResult(Enumerable.Empty<Dish>()));

        //Act
        var result = await _dishService.GetAllAsync();

        //Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task UpdateAsync_NotExistingDish_ThrowsKeyNotFoundException()
    {
        //Arrange
        Dish dish = new Dish { Id = Ulid.NewUlid() };
        _dishRepositorySubstitute.GetByIdAsync(dish.Id, false).Returns(Task.FromResult<Dish?>(dish));

    }

    [Fact]
    public async Task ToggleDishState_ExistingDish_ChangesState()
    {
        //Arrange
        Dish dish = new Dish { Id = Ulid.NewUlid(), IsAvailable = true };
        _dishRepositorySubstitute.GetByIdAsync(dish.Id, false).Returns(Task.FromResult<Dish?>(dish));
        _dishRepositorySubstitute.ToggleState(dish).Returns(true);

        //Act
        var result = await _dishService.ToggleDishStateAsync(dish.Id);

        //Assert
        Assert.True(result);
        Assert.False(dish.IsAvailable);
        await _unitOfWorkSubstitute.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task ToggleDishState_NotExistingDish_ThrowsKeyNotFoundException()
    {
        //Arrange
        Dish dish = new Dish { Id = Ulid.NewUlid(), IsAvailable = true };
        _dishRepositorySubstitute.GetByIdAsync(dish.Id).Returns(Task.FromResult<Dish?>(null));

        //Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _dishService.ToggleDishStateAsync(dish.Id));
        Assert.Contains($"Блюдо с Id {dish.Id} не найдено.", exception.Message);
    }

    [Fact]
    public void CheckMenuCategoryExists_ReturnsTrue_WhenCategoryExists()
    {
        //Arrange
        var categoryId = Ulid.NewUlid();
        _dishRepositorySubstitute.CheckMenuCategoryExists(categoryId).Returns(true);

        //Act
        var result = _dishService.CheckMenuCategoryExists(categoryId);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void CheckMenuCategoryExists_ReturnsFalse_WhenCategoryNotExists()
    {
        //Arrange
        var categoryId = Ulid.NewUlid();
        _dishRepositorySubstitute.CheckMenuCategoryExists(categoryId).Returns(false);

        //Act
        var result = _dishService.CheckMenuCategoryExists(categoryId);

        //Assert
        Assert.NotEmpty(categoryId.ToString());
        Assert.False(result);
    }

    [Fact]
    public void CheckMenuCategoryExists_ThrowsArgumentException_WhenCategoryIdIsEmpty()
    {
        //Arrange
        var categoryId = Ulid.Empty;

        //Act & Assert
        Assert.Throws<ArgumentException>(() => _dishService.CheckMenuCategoryExists(categoryId));
        Assert.Equal(Ulid.Empty, categoryId);
    }
}