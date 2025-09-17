using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities.Zones;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Dtos.Zones;
using FuelMaster.HeadOffice.Infrastructure.Contexts;

namespace FuelMaster.HeadOffice.ApplicationService.Entities.Zones
{
    public class ZonePriceHistoryService : IZonePriceHistory
    {
        private readonly IUserService _userService;
        private readonly FuelMasterDbContext _context;

        public ZonePriceHistoryService(IUserService userService, IContextFactory<FuelMasterDbContext> contextFactory)
        {
            _userService = userService;
            _context = contextFactory.CurrentContext;
        }

        public async Task<ResultDto<ZonePriceHistory>> CreateAsync(CreateHistoryDto dto)
        {
            var userId = await _userService.GetLoggedUserIdAsync();
            if (userId is null) return Results.Failure<ZonePriceHistory>();

            var history = new ZonePriceHistory(dto.ZonePriceId, userId, dto.Price);
            await _context.ZonePriceHistory.AddAsync(history);
            await _context.SaveChangesAsync();

            return Results.Success(history);
        }
    }
}
