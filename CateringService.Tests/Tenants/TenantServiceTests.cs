using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.Services;
using CateringService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CateringService.Tests.Tenants;

public sealed class TenantServiceTests
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<TenantService> _logger;
    private readonly ITenantService _tenantService;

    public TenantServiceTests()
    {
        _tenantRepository = Substitute.For<ITenantRepository>();
        _unitOfWorkRepository = Substitute.For<IUnitOfWorkRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<TenantService>>();

        _tenantService = new TenantService(_tenantRepository, _unitOfWorkRepository, _mapper, _logger);
    }

    [Fact]
    public async Task Ctor_WhenTenantRepositoryNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new TenantService(null!, _unitOfWorkRepository, _mapper, _logger));
        Assert.Contains("tenantRepository", exception.Message);
    }
}
