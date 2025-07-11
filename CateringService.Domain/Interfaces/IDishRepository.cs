﻿using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface IDishRepository : IGenericRepository<Dish, Ulid>
{
    bool ToggleState(Dish dish);
    Task<Dish?> GetBySlugAsync(string slug);
    Task<IEnumerable<Dish>> GetDishesBySupplierIdAsync(Ulid supplierId);
}