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

namespace CateringService.Tests.Addresses;

public sealed class AddressServiceTests
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
    public async Task Ctor_WhenAddressRepositoryNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new AddressService(null!, _unitOfWorkMock, _tenantRepositoryMock, _mapper, _logger));
        Assert.Contains("addressRepository", exception.Message);
    }

    [Fact]
    public async Task Ctor_WhenUnitOfWorkNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new AddressService(_addressRepositoryMock, null!, _tenantRepositoryMock, _mapper, _logger));
        Assert.Contains("unitOfWork", exception.Message);
    }

    [Fact]
    public async Task Ctor_WhenTenantRepositoryNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new AddressService(_addressRepositoryMock, _unitOfWorkMock, null!, _mapper, _logger));
        Assert.Contains("tenantRepository", exception.Message);
    }

    [Fact]
    public async Task Ctor_WhenMapperNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new AddressService(_addressRepositoryMock, _unitOfWorkMock, _tenantRepositoryMock, null!, _logger));
        Assert.Contains("mapper", exception.Message);
    }

    [Fact]
    public async Task Ctor_WhenLoggerNull_ShouldThrowArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new AddressService(_addressRepositoryMock, _unitOfWorkMock, _tenantRepositoryMock, _mapper, null!));
        Assert.Contains("logger", exception.Message);
    }

    [Fact]
    public async Task Ctor_WhenAllParameters_ShouldCreateNewInstance()
    {
        Assert.NotNull(_addressService);
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
        var request = new AddAddressRequest();

        //Act && Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _addressService.CreateAddressAsync(tenantId, request));
        Assert.Contains("TenantId is empty.", exception.Message);
    }

    [Fact]
    public async Task CreateAddressAsync_WhenTenantNotFound_ShouldThrowNotFoundException()
    {
        //Arrange
        var tenantId = Ulid.NewUlid();
        var request = new AddAddressRequest { TenantId = tenantId };

        _tenantRepositoryMock.GetByIdAsync(tenantId).Returns(Task.FromResult<Tenant?>(null));

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _addressService.CreateAddressAsync(tenantId, request));
        Assert.Contains(nameof(Tenant), exception.Message);
        Assert.Equal(tenantId.ToString(), exception.Id);
        await _tenantRepositoryMock.Received(1).GetByIdAsync(tenantId);
    }

    [Fact]
    public async Task CreateAddressAsync_WhenTenantIsNotActive_ShouldThrowNotFoundException()
    {
        //Arrange
        var tenantId = Ulid.NewUlid();
        var request = new AddAddressRequest { TenantId = tenantId };
        var inactiveTenant = new Tenant { Id = tenantId, IsActive = false };

        _tenantRepositoryMock.GetByIdAsync(tenantId).Returns(Task.FromResult<Tenant?>(inactiveTenant));

        //Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _addressService.CreateAddressAsync(tenantId, request));
        Assert.Contains(nameof(Tenant), exception.Message);
        Assert.Equal(tenantId.ToString(), exception.Id);
        await _tenantRepositoryMock.Received(1).GetByIdAsync(tenantId);
    }

    [Fact]
    public async Task CreateAddressAsync_WhenNewAddress_ShouldReturnAddress()
    {
        //Arrange
        var tenantId = Ulid.NewUlid();
        var addressId = Ulid.NewUlid();
        var request = new AddAddressRequest
        {
            TenantId = tenantId,
            Country = "Test Country"
        };
        var tenant = new Tenant { Id = tenantId, IsActive = true };
        var address = new Address { Id = addressId, TenantId = tenantId, Country = "Test Country" };
        var viewModel = new AddressViewModel { Id = addressId, Country = "Test Country" };

        _tenantRepositoryMock.GetByIdAsync(tenantId).Returns(tenant);
        _mapper.Map<Address>(request).Returns(address);
        _addressRepositoryMock.Add(address).Returns(addressId);
        _addressRepositoryMock.GetByIdAsync(addressId).Returns(address);
        _mapper.Map<AddressViewModel>(address).Returns(viewModel);

        //Act
        var result = await _addressService.CreateAddressAsync(tenantId, request);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<AddressViewModel>(result);
        Assert.Equal(addressId, result.Id);
        Assert.Equal("Test Country", result.Country);

        _tenantRepositoryMock.Received(1).GetByIdAsync(tenantId);
        _addressRepositoryMock.Received(1).Add(Arg.Any<Address>());
        _addressRepositoryMock.Received(1).GetByIdAsync(addressId);
        await _unitOfWorkMock.Received(1).SaveChangesAsync();
        _mapper.Received(1).Map<AddressViewModel>(address);
    }

    [Fact]
    public async Task CreateAddressAsync_WhenCreatedAddressIsNull_ShouldThrowNotFoundException()
    {
        //Arrange
        var tenantId = Ulid.NewUlid();
        var addressId = Ulid.NewUlid();
        var request = new AddAddressRequest
        {
            TenantId = tenantId,
            Country = "Test Country"
        };
        var tenant = new Tenant { Id = tenantId, IsActive = true };
        var address = new Address { Id = addressId, TenantId = tenantId, Country = request.Country };

        _tenantRepositoryMock.GetByIdAsync(tenantId).Returns(tenant);
        _mapper.Map<Address>(request).Returns(address);
        _addressRepositoryMock.Add(address).Returns(addressId);

        _addressRepositoryMock.GetByIdAsync(addressId).Returns(Task.FromResult<Address?>(null));

        //Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _addressService.CreateAddressAsync(tenantId, request));
        Assert.Contains(nameof(Address), exception.Message);
        _addressRepositoryMock.Received(1).GetByIdAsync(addressId);
        await _unitOfWorkMock.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task GetByIdAsync_WhenDishExists_ShouldReturnAddress()
    {
        //Arrange
        var addressId = Ulid.NewUlid();
        var address = new Address
        {
            Id = addressId,
            Country = "Test Country",
            City = "Test City",
            Zip = "832823"
        };
        var viewModel = new AddressViewModel
        {
            Id = addressId,
            Country = address.Country,
            City = address.City,
            Zip = address.Zip
        };

        _addressRepositoryMock.GetByIdAsync(addressId).Returns(address);
        _mapper.Map<AddressViewModel>(address).Returns(viewModel);

        //Act
        var result = await _addressService.GetByIdAsync(addressId);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<AddressViewModel>(result);
        Assert.Equal(addressId, result.Id);
        Assert.Equal("Test Country", result.Country);
        Assert.Equal("Test City", result.City);
        Assert.Equal("832823", result.Zip);

        await _addressRepositoryMock.Received(1).GetByIdAsync(addressId);
        _mapper.Received(1).Map<AddressViewModel>(address);
    }

    [Fact]
    public async Task GetByIdAsync_WhenDishDoesNotExist_ShouldThrowNotFoundException()
    {
        //Arrange
        Address address = new Address { Id = Ulid.NewUlid() };
        _addressRepositoryMock.GetByIdAsync(address.Id).Returns(Task.FromResult<Address?>(null));

        //Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _addressService.GetByIdAsync(address.Id));
        Assert.Contains(nameof(Address), exception.Message);
        Assert.Contains(address.Id.ToString(), exception.Message);
    }

    [Fact]
    public async Task GetByIdAsync_WhenAddressIdIsEmpty_ShouldThrowArgumentException()
    {
        //Arrange
        var addressId = Ulid.Empty;

        //Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _addressService.GetByIdAsync(addressId));
        Assert.Contains(nameof(addressId), exception.Message);
    }
}