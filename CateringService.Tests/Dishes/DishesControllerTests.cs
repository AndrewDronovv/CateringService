using Castle.Core.Logging;
using CateringService.Application.Abstractions;
using NSubstitute;

namespace CateringService.Tests.Dishes;

public class DishesControllerTests
{
    [Fact]
    public async Task CreateDishAsync_ShouldReturn201Created_WhenDishCreated()
    {
        //Arrange
        IDishAppService dishAppService = Substitute.For<IDishAppService>();
        ILogger logger = Substitute.For<ILogger>();
    }
}
