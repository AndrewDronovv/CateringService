using Castle.Core.Logging;
using CateringService.Domain.Abstractions;
using NSubstitute;

namespace CateringService.Tests.Dishes;

public class DishesControllerTests
{
    [Fact]
    public async Task CreateDishAsync_ShouldReturn201Created_WhenDishCreated()
    {
        //Arrange
        IDishService dishAppService = Substitute.For<IDishService>();
        ILogger logger = Substitute.For<ILogger>();
    }
}