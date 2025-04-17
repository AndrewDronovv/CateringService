using AutoMapper;
using CateringService.Application.DataTransferObjects.Dish;
using CateringService.Domain.Entities;

namespace CateringService.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DishCreateDto, Dish>();
        CreateMap<Dish, DishDto>();
        CreateMap<DishUpdateDto, Dish>();
    }
}