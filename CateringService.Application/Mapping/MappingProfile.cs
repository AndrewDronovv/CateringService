using AutoMapper;
using CateringService.Application.DataTransferObjects.Dish;
using CateringService.Application.DataTransferObjects.MenuCategory;
using CateringService.Application.DataTransferObjects.Tenants;
using CateringService.Domain.Entities;

namespace CateringService.Application.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Dish, DishDto>().ReverseMap();
        CreateMap<DishCreateDto, Dish>().ReverseMap();
        CreateMap<DishUpdateDto, Dish>().ReverseMap();

        CreateMap<MenuCategory, MenuCategoryDto>().ReverseMap();
        CreateMap<MenuCategoryCreateDto, MenuCategory>().ReverseMap();
        CreateMap<MenuCategoryUpdateDto, MenuCategory>().ReverseMap();

        CreateMap<TenantDto, Tenant>().ReverseMap();
    }
}