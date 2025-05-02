using CateringService.Application.Abstractions;
using CateringService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CateringService.Controllers;

[ApiController]
public class TenantsController : ControllerBase
{
    private readonly ITenantService _tenantService;

    public TenantsController(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    [HttpGet(ApiEndPoints.Tenants.GetAll)]
    public async Task<ActionResult<IEnumerable<Tenant>>> GetTenantsAsync()
    {
        var tenants = await _tenantService.GetTenantsAsync();
        return Ok(tenants);
    }
}