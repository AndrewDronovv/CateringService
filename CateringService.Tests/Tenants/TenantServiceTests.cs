using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Requests;
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

    #region Тесты конструктора
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
    #endregion

    #region Тесты получения tenant по Id
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
    #endregion

    #region Тесты получения всех tenants
    [Fact]
    public async Task GetTenantsAsync_WhenTenantListIsEmpty_ShouldReturnEmptyList()
    {
        //Arrange
        _tenantRepositoryMock.GetAllAsync().Returns(Task.FromResult(Enumerable.Empty<Tenant>()));

        //Act
        var result = await _tenantService.GetTenantsAsync();

        //Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetTenantsAsync_WhenTenantListExists_ShouldReturnTenantList()
    {
        //Arrange
        var tenants = new List<Tenant>
        {
            new Tenant{Id = Ulid.NewUlid(), Name = "Test tenant one"},
            new Tenant{Id = Ulid.NewUlid(), Name = "Test tenant two"}
        };
        var viewModels = tenants
            .Select(t => new TenantViewModel { Id = t.Id, Name = t.Name })
            .ToList();

        _tenantRepositoryMock.GetAllAsync().Returns(tenants);
        _mapper.Map<List<TenantViewModel>>(tenants).Returns(viewModels);

        //Act
        var result = await _tenantService.GetTenantsAsync();

        //Assert
        Assert.NotNull(result);
        Assert.Equal(viewModels.Count, result.Count);
        Assert.Equal(viewModels.Select(v => v.Name), result.Select(tv => tv.Name));
        Assert.All(result, tenant => Assert.False(string.IsNullOrWhiteSpace(tenant.Name)));
        Assert.IsAssignableFrom<IEnumerable<TenantViewModel>>(result);
        _mapper.Received(1).Map<List<TenantViewModel>>(Arg.Is<IEnumerable<Tenant>>(x => x.Count() > 0));

        await _tenantRepositoryMock.Received(1).GetAllAsync();
    }
    #endregion

    #region Тесты создания tenant
    [Fact]
    public async Task CreateTenantAsync_WhenAddTenantRequestIsNull_ShouldThrowArgumentNullException()
    {
        //Arrange
        AddTenantRequest? request = null;

        //Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _tenantService.CreateTenantAsync(request));
        Assert.Contains(nameof(request), exception.Message);
    }

    [Fact]
    public async Task CreateTenantAsync_WhenCreatedTenantIsNull_ShouldThrowNotFoundException()
    {
        //Arrange
        Ulid tenantId = Ulid.NewUlid();
        AddTenantRequest request = new AddTenantRequest { Name = "Test Tenant" };
        Tenant tenant = new Tenant { Id = tenantId, Name = request.Name };
        TenantViewModel viewModel = new TenantViewModel { Id = tenantId, Name = request.Name };

        _mapper.Map<Tenant>(request).Returns(tenant);
        _tenantRepositoryMock.Add(tenant).Returns(tenant.Id);
        _tenantRepositoryMock.GetByIdAsync(tenant.Id).Returns(Task.FromResult<Tenant?>(null));

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _tenantService.CreateTenantAsync(request));

        //Assert
        Assert.Contains(nameof(Tenant), exception.Message);
        Assert.Contains(tenantId.ToString(), exception.Message);
    }

    [Fact]
    public async Task CreateTenantAsync_WhenNewTenant_ShouldReturnTenant()
    {
        //Arrange
        Ulid tenantId = Ulid.NewUlid();
        AddTenantRequest request = new AddTenantRequest { Name = "Test Tenant" };
        Tenant tenant = new Tenant { Id = tenantId, Name = request.Name };
        TenantViewModel viewModel = new TenantViewModel { Id = tenantId, Name = request.Name };

        _mapper.Map<Tenant>(request).Returns(tenant);
        _tenantRepositoryMock.Add(tenant).Returns(tenant.Id);
        _tenantRepositoryMock.GetByIdAsync(tenant.Id).Returns(Task.FromResult<Tenant?>(tenant));
        _mapper.Map<TenantViewModel>(tenant).Returns(viewModel);

        //Act
        var result = await _tenantService.CreateTenantAsync(request);

        //Assert
        Assert.NotNull(result);
        Assert.NotEqual(Ulid.Empty, result.Id);
        Assert.Equal(tenantId, result.Id);
        Assert.Equal("Test Tenant", result.Name);
        _tenantRepositoryMock.Received(1).Add(Arg.Any<Tenant>());
        await _unitOfWorkRepositoryMock.Received(1).SaveChangesAsync();
        _mapper.Received(1).Map<Tenant>(request);
        _mapper.Received(1).Map<TenantViewModel>(tenant);
    }

    [Fact]
    public async Task CreateTenantAsync_WhenMappingFails_ShouldThrowInvalidOperationException()
    {
        //Arrange
        var request = new AddTenantRequest { Name = "Test Tenant" };
        _mapper.Map<Tenant>(request).Returns((Tenant?)null);

        //Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _tenantService.CreateTenantAsync(request));
        Assert.Contains("Tenant mapping failed.", exception.Message);
    }
    #endregion
}