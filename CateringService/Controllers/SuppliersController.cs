using CateringService.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CateringService.Controllers;

[Route("api/suppliers")]
[ApiController]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveSuppliers([FromQuery] int workingHours)
    {
        var suppliers = await _supplierService.GetFilteredSuppliersAsync(workingHours);
        return Ok(suppliers);
    }
}