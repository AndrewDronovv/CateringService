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

        return CreatedAtRoute("GetCompanyById", new { companyId = createdCompany.Id }, createdCompany);
    }

    [HttpGet("api/companies/{companyId}", Name = "GetCompanyById")]
    [ProducesResponseType(typeof(CompanyViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CompanyViewModel>> GetCompanyAsync(Ulid companyId, Ulid userId)
    {
        var company = await _companyService.GetCompanyByIdAsync(companyId, userId);

        return Ok(company);
    }

    //TODO: Доделать параметр userId.
    [HttpGet("api/companies/by-tax-number")]
    [ProducesResponseType(typeof(CompanyViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CompanyViewModel>> GetCompanyByTaxNumberAsync([FromQuery] string taxNumber, [FromQuery] Ulid userId)
    {
        var company = await _companyService.GetCompanyByTaxNumberAsync(taxNumber, Ulid.Parse("01HY5Q0RPNMXCA2W6JXDMVVZ7B"));

        return Ok(company);
    }
    //TODO: Доделать параметр userId.
    [HttpGet("api/companies/search-by-name")]
    [ProducesResponseType(typeof(IEnumerable<CompanyViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<CompanyViewModel>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CompanyViewModel>> SearchCompaniesByNameAsync(Ulid? tenantId, string query)
    {
        var companies = await _companyService.SearchCompaniesByNameAsync(tenantId, query);

        return Ok(companies);
    }
}