using AutoMapper;
using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Nozzles;
using FuelMaster.HeadOffice.Core.Models.Responses.Nozzles;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.ApplicationService.Entities
{
    public class NozzleService : INozzleService
    {
        private readonly FuelMasterDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorization _authorization;
        public NozzleService(IContextFactory<FuelMasterDbContext> contextFactory, IMapper mapper, IAuthorization authorization)
        {
            _context = contextFactory.CurrentContext;
            _mapper = mapper;
            _authorization = authorization;
        }

        public async Task<IEnumerable<NozzleResponse>> GetAllAsync(GetNozzleDto dto)
        {
            dto.StationId = (await _authorization.TryToGetStationIdAsync()) ?? dto.StationId;
            
            var result = await _context.Nozzles
                .Include(x => x.Tank)
                .Where(x => !dto.StationId.HasValue ||
                (x.Tank != null && x.Tank.StationId == dto.StationId))
                .ToListAsync();

            return _mapper.Map<List<NozzleResponse>>(result);
        }

        public async Task<ResultDto<Nozzle>> CreateAsync(NozzleDto dto)
        {
            try
            {
                var price = await GetZonePriceAsync(dto.TankId);

                var nozzle = new Nozzle(
                    dto.TankId,
                    dto.PumpId,
                    dto.Number,
                    dto.Status,
                    dto.Amount,
                    dto.Volume,
                    dto.Totalizer,
                    price,
                    dto.ReaderNumber
                );

                await _context.Nozzles.AddAsync(nozzle);
                await _context.SaveChangesAsync();

                return Results.Success(nozzle);
            }
            catch
            {
                return Results.Failure<Nozzle>();
            }
        }

        public async Task<ResultDto<Nozzle>> EditAsync(int id, NozzleDto dto)
        {
            try
            {
                var nozzle = await _context.Nozzles.FindAsync(id);

                if (nozzle == null)
                {
                    return Results.Failure<Nozzle>(Resource.EntityNotFound);
                }

                nozzle.TankId = dto.TankId;
                nozzle.PumpId = dto.PumpId;
                nozzle.Number = dto.Number;
                nozzle.Status = dto.Status;
                nozzle.Amount = dto.Amount;
                nozzle.Volume = dto.Volume;
                nozzle.Totalizer = dto.Totalizer;
                nozzle.ReaderNumber = dto.ReaderNumber;

                _context.Nozzles.Update(nozzle);
                await _context.SaveChangesAsync();

                return Results.Success(nozzle);
            }
            catch (NullReferenceException)
            {
                return Results.Failure<Nozzle>(Resource.EntityNotFound);
            }
        }

        public async Task<NozzleResponse?> DetailsAsync(int id)
        {
            var data = await _context.Nozzles
                .Include(x => x.Pump)
                .Include(x => x.Pump!.Station)
                .Include(x => x.Tank)
                .SingleOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<NozzleResponse?>(data);
        }

        public async Task<ResultDto> DeleteAsync(int id)
        {
            var nozzle = await _context.Nozzles.FindAsync(id);

            if (nozzle == null)
                return Results.Failure(Resource.EntityNotFound);

            _context.Nozzles.Remove(nozzle);
            await _context.SaveChangesAsync();

            return Results.Success();
        }

        public async Task<PaginationDto<NozzleResponse>> GetPaginationAsync(GetNozzlePaginationDto dto)
        {
            dto.StationId = (await _authorization.TryToGetStationIdAsync()) ?? dto.StationId;
            
            var paginatedData = await _context.Nozzles
                .Include(x => x.Tank)
                .Include(x => x.Pump)
                .Include(x => x.Pump!.Station)
                .Where(x => !dto.StationId.HasValue || (x.Tank != null && x.Tank.StationId == dto.StationId))
                .ToPaginationAsync(dto.Page);

            var mappedData = _mapper.Map<List<NozzleResponse>>(paginatedData.Data);

            return new(mappedData, paginatedData.Pages);
        }

        // Local Methods 
        private async Task<decimal> GetZonePriceAsync(int tankId)
        {
            var tank = await _context.Tanks
                .Where(x => x.Id == tankId)
                .Include(x => x.Station)
                .ThenInclude(x => x!.Zone)
                .ThenInclude(x => x!.Prices)
                .SingleOrDefaultAsync();
            if (tank is null || tank.Station?.Zone?.Prices is null)
                throw new InvalidOperationException();

            var zonePrice = tank.Station!.Zone!.Prices!.Single(x => x.FuelType == tank.FuelType);

            return zonePrice.Price;
        }
    }
}
