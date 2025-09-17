using AutoMapper;
using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Exceptions.Stations;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Stations;
using FuelMaster.HeadOffice.Core.Models.Responses.Station;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.ApplicationService.Entities
{
    public class StationService : IStationService
    {
        private readonly FuelMasterDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorization _authorization;
        public StationService(IContextFactory<FuelMasterDbContext> contextFactory, IMapper mapper, IAuthorization authorization)
        {
            _context = contextFactory.CurrentContext;
            _mapper = mapper;
            _authorization = authorization;
        }

        public async Task<IEnumerable<StationResponse>> GetAllAsync()
        {
            int? stationId = (await _authorization.TryToGetStationIdAsync()) ?? null;
            
            var result = await _context.Stations
            .Where(x => !stationId.HasValue || x.Id == stationId)
            .ToListAsync();
            return _mapper.Map<List<StationResponse>>(result);
        }

        public async Task<PaginationDto<StationResponse>> GetPaginationAsync(int currentPage)
        {
            int? stationId = (await _authorization.TryToGetStationIdAsync()) ?? null;
            
            var data = await _context.Stations
                .Include(x => x.City)
                .Include(x => x.Zone)
                .Where(x => !stationId.HasValue || x.Id == stationId)
                .ToPaginationAsync(currentPage);

            var mappedData = _mapper.Map<List<StationResponse>>(data.Data);
            return new PaginationDto<StationResponse>(mappedData, data.Pages);
        }

        public async Task<ResultDto<StationResponse>> CreateAsync(StationRequest dto)
        {
            try
            {
                var station = new Station(dto.EnglishName, dto.ArabicName, dto.CityId, dto.ZoneId);
                await _context.Stations.AddAsync(station);
                await _context.SaveChangesAsync();

                var stationResponse = _mapper.Map<StationResponse>(station);
                return Results.Success(stationResponse);
            }
            catch (NullReferenceException)
            {
                return Results.Failure<StationResponse>(Resource.EntityNotFound);
            }
        }

        public async Task<ResultDto<StationResponse>> EditAsync(int id, StationRequest dto)
        {
            var station = await _context.Stations.FindAsync(id);

            if (station == null)
            {
                return Results.Failure<StationResponse>(Resource.EntityNotFound);
            }

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                // Update nozzles prices
                var zone = await _context.Zones
                .Include(x => x.Prices)
                .SingleAsync(x => x.Id == dto.ZoneId);

                var nozzles = await _context.Nozzles
                .Include(x => x.Tank)
                .Where(x => x.Tank!.StationId == id)
                .ToListAsync();

                foreach (var nozzle in nozzles)
                {
                    var zonePrice = zone.Prices!.SingleOrDefault(x => x.FuelType == nozzle.Tank!.FuelType);
                    if (zonePrice == null || zonePrice.Price == 0)
                    {
                        throw new AssignStationToInitialZoneException(Resource.AssignStationToInitialZoneException);
                    }

                    nozzle.Price = zonePrice.Price;
                    _context.Nozzles.Update(nozzle);
                }

                station.EnglishName = dto.EnglishName;
                station.ArabicName = dto.ArabicName;
                station.CityId = dto.CityId;
                station.ZoneId = dto.ZoneId;

                _context.Stations.Update(station);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var stationResponse = _mapper.Map<StationResponse>(station);
                return Results.Success(stationResponse);
            }
            catch(AssignStationToInitialZoneException)
            {
                await transaction.RollbackAsync();
                return Results.Failure<StationResponse>(Resource.AssignStationToInitialZoneException);
            }
            catch
            {
                await transaction.RollbackAsync();
                return Results.Failure<StationResponse>(Resource.SthWentWrong);
            }
        }

        public async Task<StationResponse?> DetailsAsync(int id)
        {
            var result = await _context.Stations
                .Include(x => x.City)
                .Include(x => x.Zone)
                .SingleOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<StationResponse>(result);
        }

        public async Task<ResultDto> DeleteAsync(int id)
        {
            var station = await _context.Stations.FindAsync(id);

            if (station == null)
            {
                return Results.Failure(Resource.EntityNotFound);
            }

            _context.Stations.Remove(station);
            await _context.SaveChangesAsync();

            return Results.Success();
        }
    }
}
