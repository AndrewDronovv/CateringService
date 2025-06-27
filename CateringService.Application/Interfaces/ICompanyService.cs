using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;

namespace CateringService.Application.Interfaces;

public interface ICompanyService
{
    Task<CompanyViewModel> CreateCompanyAsync(AddCompanyRequest request, Ulid userId);
    Task<CompanyViewModel?> GetCompanyByIdAsync(Ulid companyId, Ulid userId);
}