using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.Services;
using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;
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
    public async Task CreateAddressAsync_WhenAddAddressRequestIsNull_ShouldThrowArgumentNullException()
    {
        //Arrange
        var tenantId = Ulid.NewUlid();
        AddAddressRequest? request = null;

        //Act && Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _addressService.CreateAddressAsync(tenantId, request));
        Assert.Contains(nameof(request), exception.Message);
    }

    [Fact]
    public async Task CreateAddressAsync_WhenTenantIdIsEmpty_ShouldThrowArgumentException()
    {
        //Arrange
        var tenantId = Ulid.Empty;
        AddAddressRequest request = new AddAddressRequest();

        //Act && Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _addressService.CreateAddressAsync(tenantId, request));
        Assert.Contains("TenantId is empty.", exception.Message);
    }

    [Fact]
    public async Task CreateAddressAsync_ShouldReturnNull_WhenTenantIsNull()
    {
        //Arrange
        var tenantId = Ulid.NewUlid();
        AddAddressRequest request = new AddAddressRequest();
        _tenantRepositoryMock.GetByIdAsync(request.TenantId).Returns(Task.FromResult<Tenant?>(null));

        //Act
        var result = await _addressService.CreateAddressAsync(tenantId, request);

        //Assert
        Assert.Null(result);
        await _tenantRepositoryMock.Received(1).GetByIdAsync(request.TenantId);
    }

    //[Fact]
    //public async Task CreateAddressAsync_ShouldReturnNull_WhenTenantIsNotActive()
    //{
    //    //Arrange
    //    AddAddressRequest request = new AddAddressRequest
    //    {
    //        TenantId = Ulid.NewUlid(),
    //        Country = "USA",
    //        City = "New York",
    //        Zip = "10001"
    //    };
    //    Tenant inActiveTenant = new Tenant { IsActive = false };
    //    _tenantRepositoryMock.GetByIdAsync(request.TenantId).Returns(Task.FromResult(inActiveTenant));

    //    //Act
    //    var result = await _addressService.CreateAddressAsync(request, request.TenantId);

    //    //Assert
    //    Assert.Null(result);
    //    await _tenantRepositoryMock.Received(1).GetByIdAsync(request.TenantId);
    //}

    //[Fact]
    //public async Task GetAddressAsync_ExistingAddress_ReturnsAddress()
    //{
    //    //Arrange
    //    Address address = new Address { Id = Ulid.NewUlid() };
    //    DishViewModel mappedAddress = new DishViewModel
    //    {
    //        Id = address.Id,
    //        Country = "Russia",
    //        City = "Moscow",
    //        Zip = "832823"

    //    };
    //    _addressRepositoryMock.GetByIdAsync(address.Id).Returns(Task.FromResult<Address?>(address));
    //    _mapper.Map<DishViewModel>(address).Returns(mappedAddress);

    //    //Act
    //    var result = await _addressService.GetByIdAsync(address.Id);

    //    //Assert
    //    Assert.NotNull(result);
    //    Assert.Equal(mappedAddress.Id, result.Id);
    //    Assert.Equal("Russia", result.Country);
    //    Assert.Equal("Moscow", result.City);
    //    Assert.Equal("832823", result.Zip);

    //    await _addressRepositoryMock.Received(1).GetByIdAsync(address.Id);
    //    _mapper.Received(1).Map<DishViewModel>(address);
    //}
}
