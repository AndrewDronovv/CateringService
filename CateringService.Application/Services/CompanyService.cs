using AutoMapper;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Application.Interfaces;
using CateringService.Domain.Entities;
using CateringService.Domain.Exceptions;
using CateringService.Domain.Interfaces;
using CateringService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace CateringService.Application.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CompanyService> _logger;
    private readonly ITenantRepository _tenantRepository;
    private readonly IAddressRepository _addressRepository;

    public CompanyService(ICompanyRepository companyRepository, IUnitOfWorkRepository unitOfWorkRepository,
        IMapper mapper, ILogger<CompanyService> logger, ITenantRepository tenantRepository, IAddressRepository addressRepository)
    {
        _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        _unitOfWorkRepository = unitOfWorkRepository ?? throw new ArgumentNullException(nameof(unitOfWorkRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _tenantRepository = tenantRepository ?? throw new ArgumentNullException(nameof(tenantRepository));
        _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
    }

    public async Task<CompanyViewModel> BlockCompanyAsync(Ulid companyId, Ulid userId)
    {
        if (companyId == Ulid.Empty)
        {
            _logger.LogWarning("CompanyId не должен быть пустым.");
            throw new ArgumentException(nameof(companyId), "CompanyId is empty.");
        }

        _logger.LogInformation("Получен запрос на блокировку компании {CompanyId}.", companyId);

        var company = await _companyRepository.GetByIdAsync(companyId);
        if (company is null)
        {
            _logger.LogWarning("Компания {CompanyId} не найдена.", companyId);
            throw new NotFoundException(nameof(Company), companyId.ToString());
        }

        if (company.IsBlocked)
        {
            _logger.LogWarning("Компания {CompanyId} уже заблокирована.", companyId);
            throw new ArgumentException(nameof(company), $"Компания {company.Name} уже заблокирована.");
        }

        company.UpdatedAt = DateTime.UtcNow;
        company.IsBlocked = true;

        _companyRepository.Update(company);
        await _unitOfWorkRepository.SaveChangesAsync();

        _logger.LogInformation("Компания {CompanyId} успешно заблокирована в {UpdatedAt}.", companyId, company.UpdatedAt);

        return _mapper.Map<CompanyViewModel>(company) ?? throw new InvalidOperationException("CompanyViewModel mapping failed.");
    }

    //TODO: Проверки уникальности TaxNumber.
    public async Task<CompanyViewModel> CreateCompanyAsync(AddCompanyRequest request, Ulid userId)
    {
        if (request is null)
        {
            _logger.LogWarning("Входные данные не указаны.");
            throw new ArgumentNullException(nameof(request), "Company request is null.");
        }

        if (!await _tenantRepository.CheckActiveTenantExistsAsync(request.TenantId))
        {
            _logger.LogWarning("Арендатор {TenantId} не найден или деактивирован.", request.TenantId);
            throw new NotFoundException(nameof(Tenant), request.TenantId.ToString());
        }

        if (!await _addressRepository.CheckAddressExistsAsync(request.AddressId))
        {
            _logger.LogWarning("Адрес {AddressId} не найден.", request.AddressId);
            throw new NotFoundException(nameof(Address), request.AddressId.ToString());
        }

        _logger.LogInformation("Получен запрос на создание компании.");

        var company = _mapper.Map<Company>(request) ?? throw new InvalidOperationException("Company mapping failed.");

        var companyId = _companyRepository.Add(company);
        await _unitOfWorkRepository.SaveChangesAsync();

        var createdCompany = await _companyRepository.GetByIdAsync(companyId);
        if (createdCompany is null)
        {
            _logger.LogWarning("Ошибка получения компании {CompanyId}.", companyId);
            throw new NotFoundException(nameof(Company), companyId.ToString());
        }

        _logger.LogInformation("Компания c Id {CompanyId} для арендатора с Id {TenantId}, налоговым номером {TaxNumber} успешно создана в {CreatedAt}.", companyId, request.TenantId, request.TaxNumber, createdCompany.CreatedAt);

        return _mapper.Map<CompanyViewModel>(createdCompany) ?? throw new InvalidOperationException("CompanyViewModel mapping failed.");
    }

    public async Task<PagedCompanyViewModel> GetCompaniesAsync(GetCompaniesRequest request, Ulid userId)
    {
        if (request is null)
        {
            _logger.LogWarning("Входные данные не указаны.");
            throw new ArgumentNullException(nameof(request), "Companies request is null.");
        }

        if (request.TenantId.HasValue && !await _tenantRepository.CheckActiveTenantExistsAsync(request.TenantId.Value))
        {
            _logger.LogWarning("Арендатор {TenantId} не найден или деактивирован.", request.TenantId.Value);
            throw new NotFoundException(nameof(Tenant), request.TenantId.Value.ToString());
        }

        var (companies, totalCount) = await _companyRepository.GetListAsync(request.TenantId, request.Page, request.PageSize);

        var companyViewModels = _mapper.Map<IEnumerable<CompanyViewModel>>(companies) ?? throw new InvalidOperationException("CompanyViewModel mapping failed.");

        int totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        var response = new PagedCompanyViewModel
        {
            Companies = companyViewModels.ToList(),
            TotalCount = totalCount,
            TotalPages = totalPages,
            Page = request.Page,
            PageSize = request.PageSize
        };

        _logger.LogInformation("Получено {TotalCount} записей, страница {Page} с {PageSize} записями.", response.TotalCount, response.Page, response.PageSize);

        return response;
    }

    //TODO: Добавить логику с userId.
    public async Task<CompanyViewModel?> GetCompanyByIdAsync(Ulid companyId, Ulid userId)
    {
        if (companyId == Ulid.Empty)
        {
            _logger.LogWarning("CompanyId не должен быть пустым.");
            throw new ArgumentException(nameof(companyId), "CompanyId is empty.");
        }

        _logger.LogInformation("Получен запрос на компанию {CompanyId}.", companyId);

        var company = await _companyRepository.GetByIdAsync(companyId);
        if (company is null)
        {
            _logger.LogWarning("Компания {CompanyId} не найдена.", companyId);
            throw new NotFoundException(nameof(Company), companyId.ToString());
        }

        _logger.LogInformation("Компания {CompanyId} успешно получена.", companyId);

        return _mapper.Map<CompanyViewModel?>(company) ?? throw new InvalidOperationException("CompanyViewModel mapping failed.");
    }

    public async Task<CompanyViewModel?> GetCompanyByTaxNumberAsync(string taxNumber, Ulid userId)
    {
        if (string.IsNullOrWhiteSpace(taxNumber))
        {
            _logger.LogWarning("TaxNumber не должен быть пустым.");
            throw new ArgumentException(nameof(taxNumber), "TaxNumber is empty.");
        }

        _logger.LogInformation("Получен запрос на компанию с налоговым номером {TaxNumber}.", taxNumber);

        string normalizedTaxNumber = taxNumber.Trim();

        var company = await _companyRepository.GetByTaxNumberAsync(normalizedTaxNumber);

        if (company is null)
        {
            _logger.LogWarning("Компания {TaxNumber} не найдена.", normalizedTaxNumber);
            throw new NotFoundException(nameof(Company), normalizedTaxNumber);
        }

        return _mapper.Map<CompanyViewModel>(company) ?? throw new InvalidOperationException("CompanyViewModel mapping failed.");
    }

    public async Task<IEnumerable<CompanyViewModel>> SearchCompaniesByNameAsync(Ulid? tenantId, string query)
    {
        if (tenantId.HasValue && !await _tenantRepository.CheckActiveTenantExistsAsync(tenantId.Value))
        {
            _logger.LogWarning("Арендатор {TenantId} не найден или деактивирован.", tenantId);
            throw new NotFoundException(nameof(Tenant), tenantId.ToString());
        }

        if (string.IsNullOrWhiteSpace(query))
        {
            _logger.LogWarning("Название компаний не должно быть пустым.");
            throw new ArgumentException(nameof(query), "Query is empty.");
        }

        _logger.LogInformation("Получен запрос на компании с названием {Query}.", query);

        var company = await _companyRepository.SearchByNameAsync(tenantId, query);

        if (company is null)
        {
            _logger.LogWarning("Компании с названием {Query} не найдена.", query);
            throw new NotFoundException(nameof(IEnumerable<Company>), string.Empty);
        }

        return _mapper.Map<IEnumerable<CompanyViewModel>>(company) ?? throw new InvalidOperationException("CompanyViewModel mapping failed.");
    }

    public async Task<CompanyViewModel> UpdateCompanyAsync(UpdateCompanyRequest request, Ulid userId)
    {
        var companyCurrent = await _companyRepository.GetByIdAsync(request.Id);

        _mapper.Map(request, companyCurrent);

        _companyRepository.Update(companyCurrent, false);

        await _unitOfWorkRepository.SaveChangesAsync();

        return _mapper.Map<CompanyViewModel>(companyCurrent);
    }
}