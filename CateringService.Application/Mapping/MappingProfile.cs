using AutoMapper;
using CateringService.Application.DataTransferObjects.Dish;
using CateringService.Application.DataTransferObjects.MenuCategory;
using CateringService.Domain.Entities;

namespace CateringService.Application.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DishCreateDto, Dish>();
        CreateMap<Dish, DishDto>();
        CreateMap<DishUpdateDto, Dish>();

        CreateMap<MenuCategory, MenuCategoryDto>();
        CreateMap<MenuCategoryCreateDto, MenuCategory>();
    }
}