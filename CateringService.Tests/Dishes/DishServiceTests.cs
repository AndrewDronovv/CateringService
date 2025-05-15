using CateringService.Application.Services;
using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;
using NSubstitute;

namespace CateringService.Tests.Dishes;

public class DishServiceTests
{
    private readonly IDishRepository _dishRepository;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;

    public DishServiceTests()
    {
        _dishRepository = Substitute.For<IDishRepository>();
        _unitOfWorkRepository = Substitute.For<IUnitOfWorkRepository>();
    }

    [Fact]
    public void Ctor_DishRepositoryNull_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new DishService(null, _unitOfWorkRepository));
        Assert.Contains("repository", exception.Message);
    }

    [Fact]
    public void Ctor_UnitOfWorkNull_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new DishService(_dishRepository, null));
        Assert.Contains("unitOfWork", exception.Message);
    }

    [Fact]
    public void Ctor_AllParameters_CreatesNewInstance()
    {
        var dishService = new DishService(_dishRepository, _unitOfWorkRepository);
        Assert.NotNull(dishService);
    }

    [Fact]
    public async Task AddAsync_NewDish_ReturnsDish()
    {
        Dish dish = new Dish 
        {
            Id = Ulid.NewUlid()
        };

        //Arrange
        _dishRepository.Add(dish).Returns<Ulid>(dish.Id);
        _dishRepository.GetByIdAsync(dish.Id).Returns<Task<Dish?>>(Task.FromResult(dish));
        var dishService = new DishService(_dishRepository, _unitOfWorkRepository);

        //Act
        var result = await dishService.AddAsync(dish);

        //Assert
        Assert.NotNull(result);
        Assert.Equal<Ulid>(dish.Id, result.Id);
        _dishRepository.Received(1).Add(dish);
    }
}