using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.Services;
using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CateringService.Tests.Addresses;

public class AddressServiceTests
{
    private readonly IAddressRepository _addressRepositoryMock;
    private readonly IUnitOfWorkRepository _unitOfWorkMock;
    private readonly ITenantRepository _tenantRepositoryMock;
    private readonly IMapper _mapper;
    private readonly IAddressService _addressService;
    private readonly ILogger<AddressService> _logger;

    public AddressServiceTests()
    {
        _addressRepositoryMock = Substitute.For<IAddressRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWorkRepository>();
        _tenantRepositoryMock = Substitute.For<ITenantRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<AddressService>>();

        _addressService = new AddressService(_addressRepositoryMock, _unitOfWorkMock, _tenantRepositoryMock, _mapper, _logger);
    }

    [Fact]
    public async Task CreateAddressAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        //Arrange
        var tenantId = Ulid.NewUlid();

        //Act && Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _addressService.CreateAddressAsync(null, tenantId));
        Assert.Contains("Address request is null.", exception.Message);
    }

    [Fact]
    public async Task CreateAddressAsync_ShouldThrowArgumentException_WhenTenantIdIsEmpty()
    {
        //Arrange
        AddAddressRequest request = new AddAddressRequest()
        {
            TenantId = Ulid.NewUlid(),
            Country = "USA",
            City = "New York",
            Zip = "10001"
        };

        //Act && Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _addressService.CreateAddressAsync(request, Ulid.Empty));
        Assert.Contains("TenantId is empty.", exception.Message);
    }

    [Fact]
    public async Task CreateAddressAsync_ShouldReturnNull_WhenTenantIsNull()
    {
        //Arrange
        AddAddressRequest request = new AddAddressRequest
        {
            TenantId = Ulid.NewUlid(),
            Country = "USA",
            City = "New York",
            Zip = "10001"
        };
        _tenantRepositoryMock.GetByIdAsync(request.TenantId).Returns(Task.FromResult<Tenant?>(null));

        //Act
        var result = await _addressService.CreateAddressAsync(request, request.TenantId);

        //Assert
        Assert.Null(result);
        _tenantRepositoryMock.Received(1).GetByIdAsync(request.TenantId);
    }

    [Fact]
    public async Task CreateAddressAsync_ShouldReturnNull_WhenTenantIsNotActive()
    {
        //Arrange
        AddAddressRequest request = new AddAddressRequest
        {
            TenantId = Ulid.NewUlid(),
            Country = "USA",
            City = "New York",
            Zip = "10001"
        };
        Tenant inActiveTenant = new Tenant { IsActive = false };
        _tenantRepositoryMock.GetByIdAsync(request.TenantId).Returns(Task.FromResult(inActiveTenant));

        //Act
        var result = await _addressService.CreateAddressAsync(request, request.TenantId);

        //Assert
        Assert.Null(result);
        await _tenantRepositoryMock.Received(1).GetByIdAsync(request.TenantId);
    }
}
