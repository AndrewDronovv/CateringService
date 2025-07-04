﻿using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;

namespace CateringService.Domain.Interfaces;

public interface ICompanyRepository : IGenericRepository<Company, Ulid>
{
    Task<Ulid> AddAsync(Company company);
    Task<Company?> GetByIdAsync(Ulid companyId);
    Task<Company?> GetByTaxNumberAsync(string taxNumber);
    Task<IEnumerable<Company>> SearchByNameAsync(Ulid? tenantId, string query);
    Task<(IEnumerable<Company>, int totalCount)> GetListAsync(Ulid? tenantId, int page, int pageSize);
}