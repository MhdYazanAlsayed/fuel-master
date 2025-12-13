using FuelMaster.Website.Core.Entities;

namespace FuelMaster.Website.Core.Interfaces;

public interface ITokenService
{
    Task<(string Token, DateTime ExpiresAt)> GenerateTokenAsync(ApplicationUser user);
}

