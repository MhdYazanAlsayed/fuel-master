using FuelMaster.HeadOffice.Infrastructure.Configurations;
using FuelMaster.HeadOffice.Infrastructure.Services.Implementations.External.Responses;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using System.Text.Json;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Implementations.External;

public class FuelMasterAPI : IFuelMasterAPI
{
    private readonly HttpClient _httpClient;
    public FuelMasterAPI(FuelMasterWebsiteConfiguration fuelMasterWebsiteConfiguration)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(fuelMasterWebsiteConfiguration.Url);
        _httpClient.DefaultRequestHeaders.Add("X-API-KEY", fuelMasterWebsiteConfiguration.ApiKey);
    }
    public async Task<List<TenantConfigResponse>> GetTenantsAsync()
    {
        var response = await _httpClient.GetAsync("/api/tenants");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to get tenants from FuelMasterAPI");
        }
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<TenantConfigResponse>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}