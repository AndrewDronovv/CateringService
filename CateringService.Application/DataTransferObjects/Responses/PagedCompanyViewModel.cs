namespace CateringService.Application.DataTransferObjects.Responses;

public sealed class PagedCompanyViewModel
{
    public List<CompanyViewModel> Companies { get; set; } = new List<CompanyViewModel>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}