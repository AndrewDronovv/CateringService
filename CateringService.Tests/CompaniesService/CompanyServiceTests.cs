using AutoMapper;
using CateringService.Application.Interfaces;
using CateringService.Application.Services;
using CateringService.Domain.Interfaces;
using CateringService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CateringService.Tests.CompaniesService;

public sealed class CompanyServiceTests
{
    private readonly ICompanyRepository _companyRepositoryMock;
    private readonly IUnitOfWorkRepository _unitOfWorkMock;
    private readonly IMapper _mapper;
    private readonly ILogger<CompanyService> _logger;
    private readonly ITenantRepository _tenantRepositoryMock;
    private readonly IAddressRepository _addressRepositoryMock;
    private readonly ICompanyService _companyService;

    public CompanyServiceTests()
    {
        _companyRepositoryMock = Substitute.For<ICompanyRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWorkRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CompanyService>>();
        _tenantRepositoryMock = Substitute.For<ITenantRepository>();
        _addressRepositoryMock = Substitute.For<IAddressRepository>();

        _companyService = new CompanyService(_companyRepositoryMock, _unitOfWorkMock, _mapper, _logger, _tenantRepositoryMock, _addressRepositoryMock);
    }

    #region Тесты конструктора
    [Fact]
    public async Task Ctor_WhenCompanyRepositoryNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new CompanyService(null!, _unitOfWorkMock, _mapper, _logger, _tenantRepositoryMock, _addressRepositoryMock));
        Assert.Equal("companyRepository", exception.ParamName);
    }

    [Fact]
    public async Task Ctor_WhenUnitOfWorkNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new CompanyService(_companyRepositoryMock, null!, _mapper, _logger, _tenantRepositoryMock, _addressRepositoryMock));
        Assert.Equal("unitOfWorkRepository", exception.ParamName);
    }

    [Fact]
    public async Task Ctor_WhenMapperNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new CompanyService(_companyRepositoryMock, _unitOfWorkMock, null!, _logger, _tenantRepositoryMock, _addressRepositoryMock));
        Assert.Equal("mapper", exception.ParamName);
    }

    [Fact]
    public async Task Ctor_WhenLoggerNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new CompanyService(_companyRepositoryMock, _unitOfWorkMock, _mapper, null!, _tenantRepositoryMock, _addressRepositoryMock));
        Assert.Equal("logger", exception.ParamName);
    }

    [Fact]
    public async Task Ctor_WhenTenantRepositoryNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new CompanyService(_companyRepositoryMock, _unitOfWorkMock, _mapper, _logger, null!, _addressRepositoryMock));
        Assert.Equal("tenantRepository", exception.ParamName);
    }

    [Fact]
    public async Task Ctor_WhenAddressRepositoryNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new CompanyService(_companyRepositoryMock, _unitOfWorkMock, _mapper, _logger, _tenantRepositoryMock, null!));
        Assert.Equal("addressRepository", exception.ParamName);
    }
    #endregion
}