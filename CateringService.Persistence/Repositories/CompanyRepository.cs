using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Interfaces;

namespace CateringService.Persistence.Repositories;

public class CompanyRepository : GenericRepository<Company, Ulid>, ICompanyRepository
{
    public CompanyRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Ulid> AddAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
        return company.Id;
    }
}