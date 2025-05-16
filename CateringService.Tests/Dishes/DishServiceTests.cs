using CateringService.Application.Services;
using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;
using NSubstitute;

namespace CateringService.Tests.Dishes;

public class DishServiceTests
{
    private readonly IDishRepository _dishRepositorySubstitute;
    private readonly IUnitOfWorkRepository _unitOfWorkSubstitute;

    public DishServiceTests()
    {
        _dishRepositorySubstitute = Substitute.For<IDishRepository>();
        _unitOfWorkSubstitute = Substitute.For<IUnitOfWorkRepository>();
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
        Dish dish = CreateDish();

        _dishRepositorySubstitute.Add(dish).Returns(dish.Id);
        _dishRepositorySubstitute.GetByIdAsync(dish.Id).Returns(Task.FromResult<Dish?>(dish));
        var dishService = new DishService(_dishRepositorySubstitute, _unitOfWorkSubstitute);

        //Act
        var result = await dishService.AddAsync(dish);

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
        Dish dish = CreateDish();

        _dishRepositorySubstitute.GetByIdAsync(dish.Id).Returns(Task.FromResult<Dish?>(dish));
        var dishService = new DishService(_dishRepositorySubstitute, _unitOfWorkSubstitute);

        //Act
        await dishService.DeleteAsync(dish.Id);

        //Assert
        _dishRepositorySubstitute.Received(1).Delete(dish);
        await _unitOfWorkSubstitute.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteAsync_NotExistingDish_ThrowsKeyNotFoundException()
    {
        //Arrange
        Dish dish = CreateDish();

        _dishRepositorySubstitute.GetByIdAsync(dish.Id).Returns(Task.FromResult<Dish?>(null));
        var dishService = new DishService(_dishRepositorySubstitute, _unitOfWorkSubstitute);

        //Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => dishService.DeleteAsync(dish.Id));
        Assert.Contains($"Сущность с Id = {dish.Id} не найдена.", exception.Message);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingDish_ReturnsDish()
    {
        //Arrange
        Dish dish = CreateDish();

        _dishRepositorySubstitute.GetByIdAsync(dish.Id, false).Returns(Task.FromResult<Dish?>(dish));
        var dishService = new DishService(_dishRepositorySubstitute, _unitOfWorkSubstitute);

        //Act
        var result = await dishService.GetByIdAsync(dish.Id);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(dish.Id, result.Id);
    }

    private Dish CreateDish()
    {
        return new Dish();
    }
}