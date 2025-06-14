﻿using CateringService.Domain.Entities.Approved;

namespace CateringService.Domain.Repositories;

public interface IDishRepository : IGenericRepository<Dish, Ulid>
{
    bool ToggleState(Dish dish);
}