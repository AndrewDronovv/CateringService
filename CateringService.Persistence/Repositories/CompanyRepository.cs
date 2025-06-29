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
            .FirstOrDefaultAsync(c => c.Id == companyId);
    }

    public async Task<Company?> GetByTaxNumberAsync(string taxNumber)
    {
        return await _context.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.TaxNumber == taxNumber);
    }

    public async Task<(IEnumerable<Company>, int totalCount)> GetListAsync(Ulid? tenantId, int page, int pageSize)
    {
        var query = _context.Companies
            .AsNoTracking();

        if (tenantId.HasValue)
            query = query.Where(c => c.TenantId == tenantId.Value);

        var totalCount = await query.CountAsync();

        var companies = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (companies, totalCount);
    }

    public async Task<IEnumerable<Company>> SearchByNameAsync(Ulid? tenantId, string query)
    {
        string normalizedQuery = query.Trim().ToUpper();

        IQueryable<Company> companies = _context.Companies.AsNoTracking();

        if (tenantId.HasValue)
            companies = companies.Where(c => c.TenantId == tenantId.Value);

        companies = companies
            .Where(c => c.Name.ToUpper().Contains(normalizedQuery))
            .OrderBy(c => c.Name);

        return await companies.ToListAsync();
    }
}