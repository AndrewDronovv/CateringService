using AutoMapper;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Domain.Entities;
using CateringService.Domain.Entities.Approved;

namespace CateringService.Application.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Dish, DishViewModel>().ReverseMap();
        CreateMap<AddDishRequest, Dish>().ReverseMap();
        CreateMap<UpdateDishRequest, Dish>().ReverseMap();

        CreateMap<MenuCategory, MenuCategoryViewModel>().ReverseMap();
        CreateMap<AddMenuCategoryRequest, MenuCategory>().ReverseMap();
        CreateMap<UpdateMenuCategoryRequest, MenuCategory>().ReverseMap();

        CreateMap<TenantViewModel, Tenant>().ReverseMap();
        CreateMap<AddTenantRequest, Tenant>().ReverseMap();
        CreateMap<UpdateTenantRequest, Tenant>().ReverseMap();

        CreateMap<AddAddressRequest, Address>().ReverseMap();
        CreateMap<Address, AddressViewModel>().ReverseMap();
        CreateMap<UpdateAddressRequest, Address>().ReverseMap();

        CreateMap<User, AddAddressRequest>().ReverseMap();
        CreateMap<User, UserViewModel>().ReverseMap();

        CreateMap<AddUserRequest, Customer>().ReverseMap();
        CreateMap<AddUserRequest, Supplier>().ReverseMap();
        CreateMap<AddUserRequest, Broker>().ReverseMap();
    }
}