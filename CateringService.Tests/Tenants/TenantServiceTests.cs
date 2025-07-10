using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Application.Services;
using CateringService.Domain.Entities;
using CateringService.Domain.Exceptions;
using CateringService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CateringService.Tests.Tenants;

public sealed class TenantServiceTests
{
    private readonly ITenantRepository _tenantRepositoryMock;
    private readonly IUnitOfWorkRepository _unitOfWorkRepositoryMock;
    private readonly IMapper _mapper;
    private readonly ILogger<TenantService> _logger;
    private readonly ITenantService _tenantService;

    public TenantServiceTests()
    {
        _tenantRepositoryMock = Substitute.For<ITenantRepository>();
        _unitOfWorkRepositoryMock = Substitute.For<IUnitOfWorkRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<TenantService>>();

        _tenantService = new TenantService(_tenantRepositoryMock, _unitOfWorkRepositoryMock, _mapper, _logger);
    }

    [Fact]
    public async Task Ctor_WhenTenantRepositoryNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new TenantService(null!, _unitOfWorkRepositoryMock, _mapper, _logger));
        Assert.Contains("tenantRepository", exception.Message);
    }

    [Fact]
    public async Task Ctor_WhenUnitOfWorkNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new TenantService(_tenantRepositoryMock, null!, _mapper, _logger));
        Assert.Contains("unitOfWork", exception.Message);
    }

    [Fact]
    public async Task Ctor_WhenMapperNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new TenantService(_tenantRepositoryMock, _unitOfWorkRepositoryMock, null!, _logger));
        Assert.Contains("mapper", exception.Message);
    }

    [Fact]
    public async Task Ctor_WhenLoggerNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new TenantService(_tenantRepositoryMock, _unitOfWorkRepositoryMock, _mapper, null!));
        Assert.Contains("logger", exception.Message);
    }

    [Fact]
    public async Task Ctor_WhenAllParameters_ShouldCreateNewInstance()
    {
        Assert.NotNull(_tenantService);
        Assert.IsType<TenantService>(_tenantService);
    }

    [Fact]
    public async Task GetTenantByIdAsync_WhenTenantIsEmpty_ShouldThrowArgumentException()
    {
        //Arrange
        var tenantId = Ulid.Empty;

        //Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _tenantService.GetTenantByIdAsync(tenantId));
        Assert.Contains(nameof(tenantId), exception.Message);
    }

    [Fact]
    public async Task GetTenantByIdAsync_WhenTenantDoesNotExist_ShouldThrowNotFoundException()
    {
        //Arrange
        var tenant = new Tenant { Id = Ulid.NewUlid() };
        _tenantRepositoryMock.GetByIdAsync(tenant.Id).Returns(Task.FromResult<Tenant?>(null!));

        //Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _tenantService.GetTenantByIdAsync(tenant.Id));
        Assert.Contains(nameof(Tenant), exception.Message);
        Assert.Contains(tenant.Id.ToString(), exception.Message);
    }

    [Fact]
    public async Task GetTenantByIdAsync_WhenTenantExists_ShouldReturnTenant()
    {
        //Arrange
        var tenantId = Ulid.NewUlid();
        var tenant = new Tenant { Id = tenantId, Name = "Tenant Test" };
        var viewModel = new TenantViewModel { Id = tenant.Id, Name = tenant.Name };

        _tenantRepositoryMock.GetByIdAsync(tenant.Id).Returns(tenant);
        _mapper.Map<TenantViewModel>(tenant).Returns(viewModel);

        //Act
        var result = await _tenantService.GetTenantByIdAsync(tenant.Id);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<TenantViewModel>(result);
        Assert.Equal(tenantId, result.Id);
        Assert.Equal("Tenant Test", result.Name);

        await _tenantRepositoryMock.Received(1).GetByIdAsync(tenantId);
        _mapper.Received(1).Map<TenantViewModel>(tenant);
    }
}