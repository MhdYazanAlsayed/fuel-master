//using AutoMapper;
//using FuelMaster.HeadOffice.Core.Contracts.Authentication;
//using FuelMaster.HeadOffice.Core.Contracts.Database;
//using FuelMaster.HeadOffice.Core.Contracts.Entities;
//using FuelMaster.HeadOffice.Core.Contracts.Services;
//using FuelMaster.HeadOffice.Core.Entities;
//using FuelMaster.HeadOffice.Core.Helpers;
//using FuelMaster.HeadOffice.Core.Models.Dtos;
//using FuelMaster.HeadOffice.Core.Models.Requests.Nozzles;
//using FuelMaster.HeadOffice.Core.Models.Responses.Nozzles;
//using FuelMaster.HeadOffice.Core.Resources;
//using FuelMaster.HeadOffice.Infrastructure.Contexts;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;

//namespace FuelMaster.HeadOffice.ApplicationService.Entities
//{
//    public class NozzleService : INozzleService
//    {
//        private readonly FuelMasterDbContext _context;
//        private readonly IMapper _mapper;
//        private readonly ISigninService _authorization;
//        private readonly ILogger<NozzleService> _logger;
//        private readonly ICacheService _cacheService;

//        public NozzleService(IContextFactory<FuelMasterDbContext> contextFactory, 
//            IMapper mapper, 
//            ISigninService authorization,
//            ILogger<NozzleService> logger,
//            ICacheService cacheService)
//        {
//            _context = contextFactory.CurrentContext;
//            _mapper = mapper;
//            _authorization = authorization;
//            _logger = logger;
//            _cacheService = cacheService;
//        }

//        public async Task<IEnumerable<NozzleResponse>> GetAllAsync(GetNozzleDto dto)
//        {
//            _logger.LogInformation("Getting all nozzles for station: {StationId}", dto.StationId);

//            dto.StationId = (await _authorization.TryToGetStationIdAsync()) ?? dto.StationId;
            
//            var cacheKey = $"Nozzles_Station_{dto.StationId}";
//            var cachedNozzles = await _cacheService.GetAsync<IEnumerable<NozzleResponse>>(cacheKey);
            
//            if (cachedNozzles != null)
//            {
//                _logger.LogInformation("Retrieved {Count} nozzles from cache for station: {StationId}", 
//                    cachedNozzles.Count(), dto.StationId);
//                return cachedNozzles;
//            }

//            _logger.LogInformation("Nozzles not in cache, fetching from database for station: {StationId}", dto.StationId);
            
//            var result = await _context.Nozzles
//                .Include(x => x.Tank)
//                .Where(x => !dto.StationId.HasValue ||
//                (x.Tank != null && x.Tank.StationId == dto.StationId))
//                .ToListAsync();

//            var mappedResult = _mapper.Map<List<NozzleResponse>>(result);
            
//            await _cacheService.SetAsync(cacheKey, mappedResult);
            
//            _logger.LogInformation("Cached {Count} nozzles for station: {StationId}", mappedResult.Count, dto.StationId);

//            return mappedResult;
//        }

//        public async Task<ResultDto<Nozzle>> CreateAsync(NozzleDto dto)
//        {
//            _logger.LogInformation("Creating new nozzle for tank: {TankId}, pump: {PumpId}, number: {Number}", 
//                dto.TankId, dto.PumpId, dto.Number);

//            try
//            {
//                var price = await GetZonePriceAsync(dto.TankId);

//                var nozzle = new Nozzle(
//                    dto.TankId,
//                    dto.PumpId,
//                    dto.Number,
//                    dto.Status,
//                    dto.Amount,
//                    dto.Volume,
//                    dto.Totalizer,
//                    price,
//                    dto.ReaderNumber
//                );

//                await _context.Nozzles.AddAsync(nozzle);
//                await _context.SaveChangesAsync();

//                // Invalidate cache after creating nozzle
//                await InvalidateNozzlesCacheAsync();
                
//                _logger.LogInformation("Successfully created nozzle with ID: {Id} for tank: {TankId}", 
//                    nozzle.Id, dto.TankId);

//                return Results.Success(nozzle);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error creating nozzle for tank: {TankId}, pump: {PumpId}", 
//                    dto.TankId, dto.PumpId);
//                return Results.Failure<Nozzle>();
//            }
//        }

//        public async Task<ResultDto<Nozzle>> EditAsync(int id, NozzleDto dto)
//        {
//            _logger.LogInformation("Editing nozzle with ID: {Id}, tank: {TankId}, pump: {PumpId}, number: {Number}", 
//                id, dto.TankId, dto.PumpId, dto.Number);

//            try
//            {
//                var nozzle = await _context.Nozzles.FindAsync(id);

//                if (nozzle == null)
//                {
//                    _logger.LogWarning("Nozzle with ID {Id} not found", id);
//                    return Results.Failure<Nozzle>(Resource.EntityNotFound);
//                }

//                nozzle.TankId = dto.TankId;
//                nozzle.PumpId = dto.PumpId;
//                nozzle.Number = dto.Number;
//                nozzle.Status = dto.Status;
//                nozzle.Amount = dto.Amount;
//                nozzle.Volume = dto.Volume;
//                nozzle.Totalizer = dto.Totalizer;
//                nozzle.ReaderNumber = dto.ReaderNumber;

//                _context.Nozzles.Update(nozzle);
//                await _context.SaveChangesAsync();

//                // Invalidate cache after editing nozzle
//                await InvalidateNozzlesCacheAsync();
                
//                _logger.LogInformation("Successfully updated nozzle with ID: {Id}", id);

//                return Results.Success(nozzle);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error editing nozzle with ID: {Id}", id);
//                return Results.Failure<Nozzle>(Resource.EntityNotFound);
//            }
//        }

//        public async Task<NozzleResponse?> DetailsAsync(int id)
//        {
//            _logger.LogInformation("Getting details for nozzle with ID: {Id}", id);

//            var cacheKey = $"Nozzle_Details_{id}";
//            var cachedNozzle = await _cacheService.GetAsync<NozzleResponse>(cacheKey);
            
//            if (cachedNozzle != null)
//            {
//                _logger.LogInformation("Retrieved nozzle details from cache for ID: {Id}", id);
//                return cachedNozzle;
//            }

//            _logger.LogInformation("Nozzle details not in cache, fetching from database for ID: {Id}", id);

//            var data = await _context.Nozzles
//                .Include(x => x.Pump)
//                .Include(x => x.Pump!.Station)
//                .Include(x => x.Tank)
//                .SingleOrDefaultAsync(x => x.Id == id);

//            var mappedData = _mapper.Map<NozzleResponse?>(data);
            
//            if (mappedData != null)
//            {
//                await _cacheService.SetAsync(cacheKey, mappedData);
//                _logger.LogInformation("Cached nozzle details for ID: {Id}", id);
//            }

//            return mappedData;
//        }

//        public async Task<ResultDto> DeleteAsync(int id)
//        {
//            _logger.LogInformation("Deleting nozzle with ID: {Id}", id);

//            try
//            {
//                var nozzle = await _context.Nozzles.FindAsync(id);

//                if (nozzle == null)
//                {
//                    _logger.LogWarning("Nozzle with ID {Id} not found for deletion", id);
//                    return Results.Failure(Resource.EntityNotFound);
//                }

//                _context.Nozzles.Remove(nozzle);
//                await _context.SaveChangesAsync();

//                // Invalidate cache after deleting nozzle
//                await InvalidateNozzlesCacheAsync();
                
//                _logger.LogInformation("Successfully deleted nozzle with ID: {Id}", id);

//                return Results.Success();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error deleting nozzle with ID: {Id}", id);
//                return Results.Failure(Resource.EntityNotFound);
//            }
//        }

//        public async Task<PaginationDto<NozzleResponse>> GetPaginationAsync(GetNozzlePaginationDto dto)
//        {
//            _logger.LogInformation("Getting paginated nozzles for page {Page}, station: {StationId}", 
//                dto.Page, dto.StationId);

//            dto.StationId = (await _authorization.TryToGetStationIdAsync()) ?? dto.StationId;
            
//            var cacheKey = $"Nozzles_Page_{dto.Page}_Station_{dto.StationId}";
//            var cachedPagination = await _cacheService.GetAsync<PaginationDto<NozzleResponse>>(cacheKey);
            
//            if (cachedPagination != null)
//            {
//                _logger.LogInformation("Retrieved paginated nozzles from cache for page {Page}", dto.Page);
//                return cachedPagination;
//            }

//            _logger.LogInformation("Paginated nozzles not in cache, fetching from database for page {Page}", dto.Page);
            
//            var paginatedData = await _context.Nozzles
//                .Include(x => x.Tank)
//                .Include(x => x.Pump)
//                .Include(x => x.Pump!.Station)
//                .Where(x => !dto.StationId.HasValue || (x.Tank != null && x.Tank.StationId == dto.StationId))
//                .ToPaginationAsync(dto.Page);

//            var mappedData = _mapper.Map<List<NozzleResponse>>(paginatedData.Data);
//            var result = new PaginationDto<NozzleResponse>(mappedData, paginatedData.Pages);
            
//            await _cacheService.SetAsync(cacheKey, result);
            
//            _logger.LogInformation("Cached paginated nozzles for page {Page}", dto.Page);

//            return result;
//        }

//        // Local Methods 
//        private async Task<decimal> GetZonePriceAsync(int tankId)
//        {
//            _logger.LogDebug("Getting zone price for tank ID: {TankId}", tankId);
            
//            var tank = await _context.Tanks
//                .Where(x => x.Id == tankId)
//                .Include(x => x.Station)
//                .ThenInclude(x => x!.Zone)
//                .ThenInclude(x => x!.Prices)
//                .SingleOrDefaultAsync();
//            if (tank is null || tank.Station?.Zone?.Prices is null)
//            {
//                _logger.LogError("Tank {TankId} or its zone/prices not found", tankId);
//                throw new InvalidOperationException();
//            }

//            var zonePrice = tank.Station!.Zone!.Prices!.Single(x => x.FuelType == tank.FuelType);
            
//            _logger.LogDebug("Retrieved zone price {Price} for tank {TankId} with fuel type {FuelType}", 
//                zonePrice.Price, tankId, tank.FuelType);

//            return zonePrice.Price;
//        }

//        private async Task InvalidateNozzlesCacheAsync()
//        {
//            await _cacheService.RemoveByPatternAsync("Nozzles");
//            _logger.LogInformation("Invalidated all nozzles cache entries");
//        }
//    }
//}
