using AutoMapper;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Application.Interfaces;
using CateringService.Domain.Entities;
using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Exceptions;
using CateringService.Domain.Interfaces;
using CateringService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.ComponentModel.Design;

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
        _companyRepository = companyRepository;
        _unitOfWorkRepository = unitOfWorkRepository;
        _mapper = mapper;
        _logger = logger;
        _tenantRepository = tenantRepository;
        _addressRepository = addressRepository;
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

        var company = _mapper.Map<Company>(request) ?? throw new InvalidOperationException("Company mapping failed");

        var companyId = _companyRepository.Add(company);
        await _unitOfWorkRepository.SaveChangesAsync();

        var createdCompany = await _companyRepository.GetByIdAsync(companyId);
        if (createdCompany is null)
        {
            _logger.LogWarning("Ошибка получения компании {CompanyId}.", companyId);
            throw new NotFoundException(nameof(Company), companyId.ToString());
        }

        _logger.LogInformation("Компания c Id {CompanyId} для арендатора с Id {TenantId}, налоговым номером {TaxNumber} успешно создана в {CreatedAt}.", companyId, request.TenantId, request.TaxNumber, createdCompany.CreatedAt);

        return _mapper.Map<CompanyViewModel>(createdCompany);
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
        if (tenantId.HasValue)
        {
            if (!await _tenantRepository.CheckActiveTenantExistsAsync(tenantId.Value))
            {
                _logger.LogWarning("Арендатор {TenantId} не найден или деактивирован.", tenantId);
                throw new NotFoundException(nameof(Tenant), tenantId.ToString());
            }
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

        return _mapper.Map<IEnumerable<CompanyViewModel>>(company) ?? throw new InvalidOperationException("CompanyViewMode mapping failed.");
    }
}