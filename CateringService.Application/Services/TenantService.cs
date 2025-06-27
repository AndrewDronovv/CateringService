using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Domain.Entities;
using CateringService.Domain.Exceptions;
using CateringService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace CateringService.Application.Services;

public class TenantService : ITenantService
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<TenantService> _logger;

    public TenantService(ITenantRepository tenantRepository, IUnitOfWorkRepository unitOfWork, IMapper mapper, ILogger<TenantService> logger)
    {
        _tenantRepository = tenantRepository ?? throw new ArgumentNullException(nameof(tenantRepository));
        _unitOfWorkRepository = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TenantViewModel> BlockTenantAsync(Ulid tenantId, string blockReason)
    {
        if (tenantId == Ulid.Empty)
        {
            _logger.LogWarning("TenantId не должен быть пустым.");
            throw new ArgumentException(nameof(tenantId), "TenantId is empty.");
        }

        var tenant = await _tenantRepository.GetByIdAsync(tenantId);
        if (tenant is null)
        {
            _logger.LogWarning("Арендатор {TenantId} не найден.", tenantId);
            throw new NotFoundException(nameof(Tenant), tenantId.ToString());
        }

        if (!tenant.IsActive)
        {
            _logger.LogWarning("Арендатор {TenantId} уже заблокирован.", tenantId);
            throw new ArgumentException(nameof(tenant), $"Арендатор {tenant.Name} уже заблокирован.");
        }

        await _tenantRepository.BlockAsync(tenantId, blockReason);
        _logger.LogInformation("Арендатор {TenantId} успешно заблокирован по причине {BlockReason}.", tenantId, blockReason);

        return new TenantViewModel
        {
            Id = tenantId,
            Name = tenant.Name,
            IsActive = false,
            BlockReason = blockReason
        };
    }

    public async Task<TenantViewModel> UnblockTenantAsync(Ulid tenantId)
    {
        if (tenantId == Ulid.Empty)
        {
            _logger.LogWarning("TenantId не должен быть пустым.");
            throw new ArgumentException(nameof(tenantId), "TenantId is empty.");
        }

        var tenant = await _tenantRepository.GetByIdAsync(tenantId);
        if (tenant is null)
        {
            _logger.LogWarning("Арендатор {TenantId} не найден.", tenantId);
            throw new NotFoundException(nameof(Tenant), tenantId.ToString());
        }

        if (tenant.IsActive)
        {
            _logger.LogWarning("Арендатор {TenantId} не заблокирован.", tenantId);
            throw new ArgumentException(nameof(tenant), $"Арендатор {tenant.Name} не заблокирован.");
        }

        await _tenantRepository.UnblockAsync(tenantId);
        _logger.LogInformation("Арендатор {TenantId} успешно разблокирован.", tenantId);

        return new TenantViewModel
        {
            Id = tenantId,
            Name = tenant.Name,
            IsActive = true,
            BlockReason = string.Empty
        };
    }

    public async Task DeleteTenantAsync(Ulid tenantId)
    {
        if (tenantId == Ulid.Empty)
        {
            _logger.LogWarning("TenantId не должен быть пустым.");
            throw new ArgumentException(nameof(tenantId), "TenantId is empty.");
        }

        _logger.LogInformation("Получен запрос на удаление арендатора {TenantId}.", tenantId);

        var tenantExists = await _tenantRepository.CheckTenantExistsAsync(tenantId);
        if (!tenantExists)
        {
            _logger.LogWarning("Арендатор {TenantId} не найден.", tenantId);
            throw new NotFoundException(nameof(Tenant), tenantId.ToString());
        }

        await _tenantRepository.DeleteAsync(tenantId);
        await _unitOfWorkRepository.SaveChangesAsync();

        _logger.LogInformation("Арендатор {TenantId} успешно удален.", tenantId);
    }

    public async Task<TenantViewModel> GetTenantByIdAsync(Ulid tenantId)
    {
        if (tenantId == Ulid.Empty)
        {
            _logger.LogWarning("TenantId не должен быть пустым.");
            throw new ArgumentException(nameof(tenantId), "TenantId is empty.");
        }

        _logger.LogInformation("Получен запрос на арендатора {TenantId}.", tenantId);

        var tenant = await _tenantRepository.GetByIdAsync(tenantId);
        if (tenant is null)
        {
            _logger.LogWarning("Арендатор {TenantId} не найден.", tenantId);
            throw new NotFoundException(nameof(Tenant), tenantId.ToString());
        }

        _logger.LogInformation("Арендатор {Name} с {Id} успешно получен.", tenant.Name, tenant.Id);

        return _mapper.Map<TenantViewModel>(tenant) ?? throw new InvalidOperationException("TenantViewModel mapping failed.");
    }

    public async Task<List<TenantViewModel>> GetTenantsAsync()
    {
        var tenants = await _tenantRepository.GetAllAsync();
        if (!tenants.Any())
        {
            _logger.LogWarning("Список арендаторов пуст.");
            return new List<TenantViewModel>();
        }

        _logger.LogInformation("Получено {Count} арендаторов.", tenants.ToList().Count);
        return _mapper.Map<List<TenantViewModel>>(tenants);
    }

    public async Task<TenantViewModel?> UpdateTenantAsync(Ulid tenantId, UpdateTenantRequest request)
    {
        var oldTenant = await _tenantRepository.GetByIdAsync(tenantId);

        if (oldTenant == null)
        {
            throw new Exception($"Сущность с ключом {tenantId} не найдена");
        }

        _mapper.Map(request, oldTenant);

        var updatedTenant = await _tenantRepository.UpdateAsync(oldTenant);
        await _unitOfWorkRepository.SaveChangesAsync();

        return _mapper.Map<TenantViewModel>(updatedTenant);
    }

    public async Task<TenantViewModel?> CreateTenantAsync(AddTenantRequest request)
    {
        if (request is null)
        {
            _logger.LogWarning("Входные данные не указаны.");
            throw new ArgumentNullException(nameof(request), "Tenant request is null.");
        }

        _logger.LogInformation("Получен запрос на создание арендатора.");

        var tenant = _mapper.Map<Tenant>(request) ?? throw new InvalidOperationException("Tenant mapping failed.");

        var tenantId = _tenantRepository.Add(tenant);
        await _unitOfWorkRepository.SaveChangesAsync();

        var createdTenant = await _tenantRepository.GetByIdAsync(tenantId);
        if (createdTenant is null)
        {
            _logger.LogWarning("Ошибка получения арендатора {TenantId}.", tenantId);
            throw new NotFoundException(nameof(Tenant), tenantId.ToString());
        }

        _logger.LogInformation("Арендатор {CreatedTenant.Name} с Id {TenantId} успешно создан.", createdTenant.Name, tenantId);

        return _mapper.Map<TenantViewModel>(createdTenant);
    }
}