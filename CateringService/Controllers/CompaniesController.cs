using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Application.Interfaces;
using CateringService.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CateringService.Controllers;

[ApiController]
[TypeFilter<LoggingActionFilter>]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService ?? throw new ArgumentNullException(nameof(companyService));
    }

    [HttpPost("api/companies")]
    [ProducesResponseType(typeof(CompanyViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CompanyViewModel>> CreateCompanyAsync([FromBody] AddCompanyRequest request)
    {
        var createdCompany = await _companyService.CreateCompanyAsync(request, Ulid.Parse("01HY5Q0RPNMXCA2W6JXDMVVZ7B"));

        return Ok(createdCompany);
    }
}