using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.TankTransactions;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.ApplicationService.Entities
{
    public class TankTransactionService : ITankTransactionService
    {
        private readonly FuelMasterDbContext _context;

        public TankTransactionService(IContextFactory<FuelMasterDbContext> contextFactory)
        {
            _context = contextFactory.CurrentContext;
        }
        public async Task<ResultDto<TankTransaction>> CreateAsync(CreateTankTransactionDto dto)
        {
            var tankTransaction =
                new TankTransaction(dto.TankId, dto.CurrentVolume, dto.CurrentLevel);

            await _context.TankTransactions.AddAsync(tankTransaction);
            await _context.SaveChangesAsync();

            return Results.Success(tankTransaction);
        }

        public async Task<PaginationDto<TankTransaction>> GetPaginationAsync(GetTankTransactionPaginationDto dto)
        {
            return await _context.TankTransactions
                .Include(x => x.Tank)
                .Include(x => x.Tank!.Station)
                .Where(x =>
                    (!dto.StationId.HasValue ||
                        (x.Tank != null && x.Tank.StationId == dto.StationId)) ||
                    (!dto.TankId.HasValue || x.TankId == dto.TankId))
                .ToPaginationAsync(dto.Page);
        }
    }
}
