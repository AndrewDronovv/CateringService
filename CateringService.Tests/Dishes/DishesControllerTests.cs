using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Controllers;
using CateringService.Domain.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace CateringService.Tests.Dishes;

public sealed class DishesControllerTests
{
    private readonly IDishService _dishService;
    public DishesControllerTests()
    {
        _dishService = Substitute.For<IDishService>();
    }
    [Fact]
    public void Ctor_WhenAllParameters_ShouldCreateDishController()
    {
        var dishservice = new DishesController(_dishService);
        Assert.NotNull(dishservice);
    }

    [Fact]
    public void Ctor_WhenDishServiceIsNull_ShouldThrowArgumentNullException()
    {
        //Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new DishesController(null!));

        Assert.Equal("dishService", exception.ParamName);
    }

    [Fact]
    public async Task GetDishByIdAsyncV1_WhenDishExists_ShouldReturnOkResult()
    {
        //Arrange
        var dishId = Ulid.NewUlid();
        var expectedDish = new DishViewModel { Id = dishId, Name = "TestDish" };
        _dishService.GetByIdAsync(dishId).Returns(expectedDish);

        var controller = new DishesController(_dishService);

        //Act
        var result = await controller.GetDishByIdAsyncV1(dishId);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualDish = Assert.IsType<DishViewModel>(okResult.Value);
        Assert.Equal(expectedDish.Id, actualDish.Id);
        Assert.Equal(expectedDish.Name, actualDish.Name);
    }

    [Fact]
    public async Task GetDishByIdAsyncV2_WhenDishExists_ShouldReturnOkResult()
    {
        //Arrange
        var dishId = Ulid.NewUlid();
        var expectedDish = new DishViewModel { Id = dishId, Name = "TestDish" };
        _dishService.GetByIdAsync(dishId).Returns(expectedDish);

        var controller = new DishesController(_dishService);

        //Act
        var result = await controller.GetDishByIdAsyncV2(dishId);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualDish = Assert.IsType<DishViewModel>(okResult.Value);
        Assert.Equal(expectedDish.Id, actualDish.Id);
        Assert.Equal(expectedDish.Name, actualDish.Name);
    }

    [Fact]
    public async Task CreateDishAsync_WhenNewDish_ShouldReturnCreatedAtRouteResult()
    {
        //Arrange
        var supplierId = Ulid.NewUlid();
        var request = new AddDishRequest { Name = "TestDish" };
        var dishViewModel = new DishViewModel { Id = Ulid.NewUlid(), Name = request.Name };

        _dishService.CreateDishAsync(supplierId, request).Returns(dishViewModel);
        var controller = new DishesController(_dishService);

        //Act
        var result = await controller.CreateDishAsync(request, supplierId);

        //Assert
        var createdAtRoute = Assert.IsType<CreatedAtRouteResult>(result.Result);
        Assert.Equal("GetDishByIdV1", createdAtRoute.RouteName);
        Assert.Equal(dishViewModel.Id, createdAtRoute.RouteValues!["dishId"]);

        var actual = Assert.IsType<DishViewModel>(createdAtRoute.Value);
        Assert.Equal(request.Name, actual.Name);
        Assert.Equal(dishViewModel.Id, actual.Id);

        await _dishService.Received(1).CreateDishAsync(supplierId, request);
    }

    [Fact]
    public async Task GetDishBySlugAsync_WhenDishExists_ShouldReturnOkResult()
    {
        //Arrange
        var slug = "Test";
        var normalizedSlug = slug.ToLower();
        var expectedDish = new DishViewModel { Id = Ulid.NewUlid(), Name = "TestDish" };

        _dishService.GetBySlugAsync(normalizedSlug).Returns(expectedDish);
        var controller = new DishesController(_dishService);

        //Act
        var result = await controller.GetDishBySlugAsync(normalizedSlug);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualDish = Assert.IsType<DishViewModel>(okResult.Value);
        Assert.Equal(expectedDish.Id, actualDish.Id);
        Assert.Equal(expectedDish.Name, actualDish.Name);

        await _dishService.Received(1).GetBySlugAsync(normalizedSlug);
    }

    [Fact]
    public async Task ToggleDishStateAsync_WhenDishExists_ShouldReturnNoContent()
    {
        //Arrange
        var dishId = Ulid.NewUlid();
        var dish = new DishViewModel { Id = dishId, Name = "TestDish" };
        _dishService.GetByIdAsync(dishId).Returns(dish);

        var controller = new DishesController(_dishService);

        //Act
        var result = await controller.ToggleDishStateAsync(dishId);

        //Assert
        var noContent = Assert.IsType<NoContentResult>(result);
        Assert.Equal(StatusCodes.Status204NoContent, noContent.StatusCode);
    }
}