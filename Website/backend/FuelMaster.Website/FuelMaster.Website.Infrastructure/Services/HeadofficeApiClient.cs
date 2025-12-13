using FuelMaster.Website.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace FuelMaster.Website.Infrastructure.Services;

public class HeadofficeApiClient : IHeadofficeApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<HeadofficeApiClient> _logger;

    public HeadofficeApiClient(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<HeadofficeApiClient> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("Headoffice");

        var baseUrl = _configuration["Headoffice:ApiBaseUrl"];
        if (!string.IsNullOrEmpty(baseUrl))
        {
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        var apiKey = _configuration["Headoffice:ApiKey"];
        if (!string.IsNullOrEmpty(apiKey))
        {
            _httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
        }

        var timeoutSeconds = _configuration.GetValue<int>("Headoffice:TimeoutSeconds", 30);
        _httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
    }

    public async Task<CreateTenantDatabaseResponse> CreateTenantDatabaseAsync(CreateTenantDatabaseRequest request)
    {
        try
        {
            _logger.LogInformation("Creating tenant database for {TenantName}", request.TenantName);

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            

            var response = await _httpClient.PostAsync("/api/tenants", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<CreateTenantDatabaseResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result != null && result.Success)
                {
                    _logger.LogInformation("Successfully created tenant database for {TenantName}", request.TenantName);
                    return result;
                }
            }

            _logger.LogWarning("Failed to create tenant database for {TenantName}. Status: {StatusCode}, Response: {Response}",
                request.TenantName, response.StatusCode, responseContent);

            return new CreateTenantDatabaseResponse
            {
                Success = false,
                ErrorMessage = $"Failed to create tenant database. Status: {response.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tenant database for {TenantName}", request.TenantName);
            return new CreateTenantDatabaseResponse
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<TenantInfoResponse?> GetTenantInfoAsync(string tenantName)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/tenants/{tenantName}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TenantInfoResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            _logger.LogWarning("Failed to get tenant info for {TenantName}. Status: {StatusCode}", tenantName, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting tenant info for {TenantName}", tenantName);
            return null;
        }
    }

    public async Task<BackupResponse> CreateBackupAsync(string tenantName, CreateBackupRequest request)
    {
        try
        {
            _logger.LogInformation("Creating backup for tenant {TenantName}", tenantName);

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"/api/tenants/{tenantName}/backup", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<BackupResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result != null && result.Success)
                {
                    _logger.LogInformation("Successfully created backup for tenant {TenantName}", tenantName);
                    return result;
                }
            }

            _logger.LogWarning("Failed to create backup for {TenantName}. Status: {StatusCode}, Response: {Response}",
                tenantName, response.StatusCode, responseContent);

            return new BackupResponse
            {
                Success = false,
                ErrorMessage = $"Failed to create backup. Status: {response.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating backup for {TenantName}", tenantName);
            return new BackupResponse
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<RestoreResponse> RestoreBackupAsync(string tenantName, RestoreBackupRequest request)
    {
        try
        {
            _logger.LogInformation("Restoring backup for tenant {TenantName}", tenantName);

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"/api/tenants/{tenantName}/restore", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestoreResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result != null && result.Success)
                {
                    _logger.LogInformation("Successfully restored backup for tenant {TenantName}", tenantName);
                    return result;
                }
            }

            _logger.LogWarning("Failed to restore backup for {TenantName}. Status: {StatusCode}, Response: {Response}",
                tenantName, response.StatusCode, responseContent);

            return new RestoreResponse
            {
                Success = false,
                ErrorMessage = $"Failed to restore backup. Status: {response.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error restoring backup for {TenantName}", tenantName);
            return new RestoreResponse
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }
}

