using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Dish;
using CateringService.Application.DataTransferObjects.Tenants;
using CateringService.Domain.Entities;
using CateringService.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CateringService.Controllers;

[ApiController]
[TypeFilter<LoggingActionFilter>]
public class TenantsController : ControllerBase
{
    private readonly ITenantService _tenantService;
    private readonly IMapper _mapper;
    private readonly ILogger<DishesController> _logger;

    public TenantsController(ITenantService tenantService, IMapper mapper, ILogger<DishesController> logger)
    {
        _tenantService = tenantService ?? throw new ArgumentNullException(nameof(tenantService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet(ApiEndPoints.Tenants.GetAll)]
    [ProducesResponseType(typeof(IEnumerable<TenantDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TenantDto>>> GetTenantsAsync()
    {
        _logger.LogInformation("Получен запрос на список арендаторов.");
        var tenants = await _tenantService.GetTenantsAsync();

        if (tenants is null || !tenants.Any())
        {
            _logger.LogInformation("Список арендаторов пуст.");
            return Ok(Enumerable.Empty<TenantDto>());
        }

        var tenantsDto = _mapper.Map<IEnumerable<TenantDto>>(tenants);
        _logger.LogInformation($"Запрос списка арендаторов выполнен успешно. Найдено {tenants.Count()} арендатора.");
        return Ok(tenantsDto);
    }

    [HttpGet(ApiEndPoints.Tenants.Get, Name = "GetTenantById")]
    [ProducesResponseType(typeof(DishDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TenantDto>> GetTenantAsync(Ulid tenantId)
    {
        if (tenantId == Ulid.Empty)
        {
            _logger.LogWarning("Id не должен быть пустым.");
            return BadRequest(new { Error = "Id не должен быть пустым." });
        }

        var tenant = await _tenantService.GetTenantByIdAsync(tenantId);
        if (tenant is null)
        {
            _logger.LogWarning($"Арендатор с Id = {tenantId} не найден.");
            return NotFound(new { Error = $"Арендатор с Id = {tenantId} не найден." });
        }

        var tenantDto = _mapper.Map<TenantDto>(tenant);
        _logger.LogInformation($"Арендатор {tenantDto.Name} с Id = {tenantId} успешно получен.");
        return Ok(tenantDto);
    }

    [HttpPost(ApiEndPoints.Tenants.Create)]
    [ProducesResponseType(typeof(TenantDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TenantDto>> CreateTenantAsync([FromBody] TenantCreateDto input)
    {
        if (input is null)
        {
            _logger.LogWarning("Входные данные не указаны. Операция создания арендатора не может быть выполнена.");
            return BadRequest(new { Error = "Входные параметры отсутствуют. Пожалуйста, предоставьте данные для создания арендатора." });
        }

        var tenant = _mapper.Map<Tenant>(input);

        var createdTenant = await _tenantService.AddTenantAsync(tenant);
        if (createdTenant is null)
        {
            _logger.LogWarning("Арендатор не был создан.");
            return BadRequest("Арендатор не был создан.");
        }

        var tenantDto = _mapper.Map<TenantDto>(createdTenant);
        _logger.LogInformation($"Арендатор {tenantDto.Name} с Id = {tenantDto.Id} создан в {tenantDto.CreatedAt}");
        return CreatedAtRoute("GetTenantById",
        new
        {
            tenantId = createdTenant.Id,
        },
        _mapper.Map<TenantDto>(createdTenant));
    }

    [HttpDelete(ApiEndPoints.Tenants.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTenantAsync(Ulid tenantId)
    {
        if (tenantId == Ulid.Empty)
        {
            _logger.LogWarning("Id не должен быть пустым.");
            return BadRequest(new { Error = "Id не должен быть пустым." });
        }

        await _tenantService.DeleteAsync(tenantId);
        _logger.LogInformation($"Арендатор с Id = {tenantId} удален.");
        return NoContent();
    }

    [HttpPut(ApiEndPoints.Tenants.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTenantAsync(Ulid tenantId, TenantUpdateDto input)
    {
        if (tenantId == Ulid.Empty)
        {
            _logger.LogWarning($"Id не должен быть пустым.");
            return BadRequest(new { Error = "Id не должен быть пустым." });
        }

        if (input is null)
        {
            _logger.LogWarning("Входные данные не указаны. Операция обновления арендатора не может быть выполнена.");
            return BadRequest(new { Error = "Входные параметры отсутствуют. Пожалуйста, предоставьте данные для создания арендатора." });
        }

        var updateRequest = _mapper.Map<Tenant>(input);

        await _tenantService.UpdateTenantAsync(tenantId, updateRequest);

        _logger.LogInformation($"Арендатор с Id = {tenantId} успешно обновлен.");
        return Ok(updateRequest);
    }
    [HttpPut(ApiEndPoints.Tenants.Block)]
    public async Task<ActionResult<TenantDto>> BlockTenantAsync([FromRoute] Ulid tenantId, [FromQuery] string blockReason)
    {
        if (tenantId == Ulid.Empty)
        {
            _logger.LogWarning("Id не должен быть пустым.");
            return BadRequest(new { Error = "Id не должен быть пустым." });
        }
        var tenant = await _tenantService.BlockTenantAsync(tenantId, blockReason);
        if (tenant is null)
        {
            _logger.LogWarning($"Арендатор с Id = {tenantId} не найден.");
            return NotFound(new { Error = $"Арендатор с Id = {tenantId} не найден." });
        }
        var tenantDto = _mapper.Map<TenantDto>(tenant);
        _logger.LogInformation($"Арендатор {tenantDto.Name} с Id = {tenantId} успешно заблокирован.");
        return Ok(tenantDto);
    }
}