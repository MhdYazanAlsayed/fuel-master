using FuelMaster.Website.Core.Entities;
using FuelMaster.Website.Core.Enums;
using FuelMaster.Website.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FuelMaster.Website.Core.Services;

public class TenantService : ITenantService
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUserSubscriptionRepository _subscriptionRepository;
    private readonly IHeadofficeApiClient _headofficeApiClient;
    // private readonly IEncryptionService _encryptionService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<TenantService> _logger;

    public TenantService(
        ITenantRepository tenantRepository,
        IUserSubscriptionRepository subscriptionRepository,
        IHeadofficeApiClient headofficeApiClient,
        // IEncryptionService encryptionService,
        UserManager<ApplicationUser> userManager,
        ILogger<TenantService> logger)
    {
        _tenantRepository = tenantRepository;
        _subscriptionRepository = subscriptionRepository;
        _headofficeApiClient = headofficeApiClient;
        // _encryptionService = encryptionService;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Tenant> CreateTenantAsync(string userId, CreateTenantRequest request)
    {
        // Validate tenant name
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Tenant name is required", nameof(request));
        }

        // Validate tenant name format (alphanumeric and hyphens only)
        if (!System.Text.RegularExpressions.Regex.IsMatch(request.Name, @"^[a-z0-9-]+$"))
        {
            throw new ArgumentException("Tenant name can only contain lowercase letters, numbers, and hyphens", nameof(request));
        }

        // Check if tenant name already exists
        if (await _tenantRepository.TenantNameExistsAsync(request.Name))
        {
            throw new InvalidOperationException($"Tenant name '{request.Name}' already exists");
        }

        // Get user's active subscription
        var subscription = await _subscriptionRepository.GetActiveSubscriptionByUserIdAsync(userId);
        if (subscription == null)
        {
            throw new InvalidOperationException("User must have an active subscription to create a tenant");
        }
      
        // Create tenant database on Headoffice server
        var createRequest = new CreateTenantDatabaseRequest
        {
            TenantName = request.Name,
        };

        var createResponse = await _headofficeApiClient.CreateTenantDatabaseAsync(createRequest);

        if (!createResponse.Success || string.IsNullOrEmpty(createResponse.DatabaseName))
        {
            _logger.LogError("Failed to create tenant database for {TenantName}: {Error}",
                request.Name, createResponse.ErrorMessage);
            throw new InvalidOperationException(
                $"Failed to create tenant database: {createResponse.ErrorMessage ?? "Unknown error"}");
        }

        // Create tenant entity
        var tenant = new Tenant
        {
            Name = request.Name,
            UserId = userId,
            DatabaseName = createResponse.DatabaseName,
            Status = TenantStatus.Active,
            PlanId = subscription.PlanId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdTenant = await _tenantRepository.AddAsync(tenant);

        _logger.LogInformation("Successfully created tenant {TenantName} for user {UserId}", request.Name, userId);

        return createdTenant;
    }


    public async Task<IEnumerable<Tenant>> GetAllTenantsAsync(bool isActive = true)
    {
        var tenants = await _tenantRepository.GetAllAsync();
        if (isActive)
        {
            tenants = tenants.Where(t => t.Status == TenantStatus.Active);
        }
        
        return tenants;
    }

    public async Task<Tenant?> GetTenantByIdAsync(Guid tenantId)
    {
        return await _tenantRepository.GetByIdAsync(tenantId);
    }

    public async Task<Tenant?> GetTenantByNameAsync(string tenantName)
    {
        return await _tenantRepository.GetByNameAsync(tenantName);
    }

    public async Task<Tenant?> GetTenantByUserIdAsync(string userId)
    {
        return await _tenantRepository.GetByUserIdAsync(userId);
    }

    public async Task<IEnumerable<Tenant>> GetUserTenantsAsync(string userId)
    {
        return await _tenantRepository.GetTenantsByUserIdAsync(userId);
    }

    public async Task<Tenant> UpdateTenantAsync(Guid tenantId, UpdateTenantRequest request)
    {
        var tenant = await _tenantRepository.GetByIdAsync(tenantId);
        if (tenant == null)
        {
            throw new ArgumentException("Tenant not found", nameof(tenantId));
        }

        if (request.Name != null && request.Name != tenant.Name)
        {
            // Check if new name already exists
            if (await _tenantRepository.TenantNameExistsAsync(request.Name))
            {
                throw new InvalidOperationException($"Tenant name '{request.Name}' already exists");
            }
            tenant.Name = request.Name;
        }

        if (request.Status.HasValue)
        {
            tenant.Status = request.Status.Value;
        }

        tenant.UpdatedAt = DateTime.UtcNow;
        await _tenantRepository.UpdateAsync(tenant);

        return tenant;
    }

    public async Task<bool> DeleteTenantAsync(Guid tenantId, string userId)
    {
        var tenant = await _tenantRepository.GetByIdAsync(tenantId);
        if (tenant == null || tenant.UserId != userId)
        {
            return false;
        }

        // Soft delete
        tenant.Status = TenantStatus.Deleted;
        tenant.DeletedAt = DateTime.UtcNow;
        tenant.UpdatedAt = DateTime.UtcNow;
        await _tenantRepository.UpdateAsync(tenant);

        _logger.LogInformation("Tenant {TenantName} deleted by user {UserId}", tenant.Name, userId);

        return true;
    }

    public async Task<string> GetTenantConnectionStringAsync(string tenantName)
    {
        throw new NotImplementedException();
        // var tenant = await _tenantRepository.GetByNameAsync(tenantName);
        // if (tenant == null)
        // {
        //     throw new ArgumentException("Tenant not found", nameof(tenantName));
        // }

        // return _encryptionService.Decrypt(tenant.ConnectionString);
    }
}

