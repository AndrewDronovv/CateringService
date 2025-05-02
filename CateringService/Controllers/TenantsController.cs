using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Tenants;
using Microsoft.AspNetCore.Mvc;
using static CateringService.ApiEndPoints;

namespace CateringService.Controllers;

[ApiController]
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
}