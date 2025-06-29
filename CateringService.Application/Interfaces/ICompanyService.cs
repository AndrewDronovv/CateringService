using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;

namespace CateringService.Application.Interfaces;

public interface ICompanyService
{
    Task<CompanyViewModel> CreateCompanyAsync(AddCompanyRequest request, Ulid userId);
    Task<CompanyViewModel?> GetCompanyByIdAsync(Ulid companyId, Ulid userId);
    Task<CompanyViewModel?> GetCompanyByTaxNumberAsync(string taxNumber, Ulid userId);
    Task<IEnumerable<CompanyViewModel>> SearchCompaniesByNameAsync(Ulid? tenantId, string query);
    Task<PagedCompanyViewModel> GetCompaniesAsync(GetCompaniesRequest request, Ulid userId);
    Task<CompanyViewModel> UpdateCompanyAsync(UpdateCompanyRequest request, Ulid userId);
}