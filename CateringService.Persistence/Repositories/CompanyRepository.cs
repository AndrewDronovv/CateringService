using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Company?> GetByIdAsync(Ulid companyId)
    {
        return await _context.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == companyId);
    }

    public async Task<Company?> GetByTaxNumberAsync(string taxNumber)
    {
        return await _context.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.TaxNumber == taxNumber);
    }
}