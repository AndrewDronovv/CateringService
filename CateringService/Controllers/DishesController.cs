using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace CateringService.Controllers;

[Route("api/dishes")]
[ApiController]
public class DishesController : ControllerBase
{
    private readonly IDishAppService _dishAppService;

    public DishesController(IDishAppService dishAppService)
    {
        _dishAppService = dishAppService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDishesAsync()
    {
        var dishes = await _dishAppService.GetDishesAsync();
        return Ok(dishes);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDishAsync(DishCreateDto entity)
    {
        await _dishAppService.CreateDishAsync(entity);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDishAsync(int id)
    {
        await _dishAppService.DeleteDishAsync(id);
        return Ok();
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDishAsync(int id)
    {
        var dish = await _dishAppService.GetDishByIdAsync(id);
        return Ok(dish);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDishAsync(int id, DishUpdateDto entity)
    {
        await _dishAppService.UpdateDishAsync(id, entity);
        return Ok();
    }
}
