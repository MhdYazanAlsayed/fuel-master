using System;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Pricing.DTOs;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Pricing;


public interface IPricingService : IScopedDependency
{
    Task ChangePricesAsync(int zoneId, List<ChangePricesDto> dto);
}