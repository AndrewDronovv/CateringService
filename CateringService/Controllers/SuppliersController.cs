using CateringService.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CateringService.Controllers;

[ApiController]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }
}