using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CateringService.Controllers;

[ApiController]
[TypeFilter<LoggingActionFilter>]
public class TenantsController : ControllerBase
{
    private readonly ITenantService _tenantService;

    public TenantsController(ITenantService tenantService)
    {
        _tenantService = tenantService ?? throw new ArgumentNullException(nameof(tenantService));
    }

    [HttpGet(ApiEndPoints.Tenants.GetAll)]
    [ProducesResponseType(typeof(List<TenantViewModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TenantViewModel>>> GetTenantsAsync()
    {
        var tenants = await _tenantService.GetTenantsAsync();

        return Ok(tenants);
    }

    [HttpGet(ApiEndPoints.Tenants.Get, Name = "GetTenantById")]
    [ProducesResponseType(typeof(TenantViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TenantViewModel>> GetTenantAsync(Ulid tenantId)
    {
        var tenant = await _tenantService.GetTenantByIdAsync(tenantId);

        return Ok(tenant);
    }

    [HttpPost(ApiEndPoints.Tenants.Create)]
    [ProducesResponseType(typeof(TenantViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TenantViewModel>> CreateTenantAsync([FromBody] AddTenantRequest request)
    {
        var createdTenant = await _tenantService.CreateTenantAsync(request);

        return Ok(createdTenant);
    }

    [HttpDelete(ApiEndPoints.Tenants.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTenantAsync(Ulid tenantId)
    {
        return NoContent();
    }

    [HttpPut(ApiEndPoints.Tenants.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTenantAsync(Ulid tenantId, UpdateTenantRequest input)
    {
        return Ok();
    }
    [HttpPut(ApiEndPoints.Tenants.Block)]
    public async Task<ActionResult<TenantViewModel>> BlockTenantAsync([FromBody] BlockTenantRequest input)
    {

        var blockedTenant = await _tenantService.BlockTenantAsync(input.Id, input.BlockReason ?? string.Empty);

        return Ok();
    }

    [HttpPut(ApiEndPoints.Tenants.Unblock)]
    public async Task<ActionResult<TenantViewModel>> UnblockTenansAsync([FromRoute] Ulid tenantId)
    {
        var unblockedTenant = await _tenantService.UnblockTenantAsync(tenantId);

        return Ok();
    }
}