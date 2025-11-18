//using AutoMapper;
//using FuelMaster.HeadOffice.Core.Contracts.Authentication;
//using FuelMaster.HeadOffice.Core.Contracts.Database;
//using FuelMaster.HeadOffice.Core.Contracts.Entities;
//using FuelMaster.HeadOffice.Core.Contracts.Services;
//using FuelMaster.HeadOffice.Core.Entities;
//using FuelMaster.HeadOffice.Core.Helpers;
//using FuelMaster.HeadOffice.Core.Models.Dtos;
//using FuelMaster.HeadOffice.Core.Models.Requests.Pumps;
//using FuelMaster.HeadOffice.Core.Models.Responses.Pumps;
//using FuelMaster.HeadOffice.Core.Resources;
//using FuelMaster.HeadOffice.Infrastructure.Contexts;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;

//namespace FuelMaster.HeadOffice.ApplicationService.Entities
//{
//    public class PumpService : IPumpService
//    {
//        private readonly FuelMasterDbContext _context;
//        private readonly IMapper _mapper;
//        private readonly ISigninService _authorization;
//        private readonly ILogger<PumpService> _logger;
//        private readonly ICacheService _cacheService;

//        public PumpService(IContextFactory<FuelMasterDbContext> contextFactory, 
//            IMapper mapper, 
//            ISigninService authorization,
//            ILogger<PumpService> logger,
//            ICacheService cacheService)
//        {
//            _context = contextFactory.CurrentContext;
//            _mapper = mapper;
//            _authorization = authorization;
//            _logger = logger;
//            _cacheService = cacheService;
//        }

//        public async Task<PaginationDto<PumpResponse>> GetPaginationAsync(int page , int? stationId)
//        {
//            _logger.LogInformation("Getting paginated pumps for page {Page}, station: {StationId}", page, stationId);

//            stationId = (await _authorization.TryToGetStationIdAsync()) ?? stationId;
            
//            var cacheKey = $"Pumps_Page_{page}_Station_{stationId}";
//            var cachedPagination = await _cacheService.GetAsync<PaginationDto<PumpResponse>>(cacheKey);
            
//            if (cachedPagination != null)
//            {
//                _logger.LogInformation("Retrieved paginated pumps from cache for page {Page}", page);
//                return cachedPagination;
//            }

//            _logger.LogInformation("Paginated pumps not in cache, fetching from database for page {Page}", page);
            
//            var paginatedResult = await _context.Pumps
//                .Include(x => x.Nozzles)
//                .Include(x => x.Station)
//                .Where(x => !stationId.HasValue || x.StationId == stationId)
//                .ToPaginationAsync(page);

//            var mappedData = _mapper.Map<List<PumpResponse>>(paginatedResult.Data);
//            var result = new PaginationDto<PumpResponse>(mappedData, paginatedResult.Pages);
            
//            await _cacheService.SetAsync(cacheKey, result);
            
//            _logger.LogInformation("Cached paginated pumps for page {Page}", page);

//            return result;
//        }

//        public async Task<IEnumerable<PumpResponse>> GetAllAsync(GetPumpRequest request)
//        {
//            _logger.LogInformation("Getting all pumps for station: {StationId}", request.StationId);

//            request.StationId = (await _authorization.TryToGetStationIdAsync()) ?? request.StationId;
            
//            var cacheKey = $"Pumps_Station_{request.StationId}";
//            var cachedPumps = await _cacheService.GetAsync<IEnumerable<PumpResponse>>(cacheKey);
            
//            if (cachedPumps != null)
//            {
//                _logger.LogInformation("Retrieved {Count} pumps from cache for station: {StationId}", 
//                    cachedPumps.Count(), request.StationId);
//                return cachedPumps;
//            }

//            _logger.LogInformation("Pumps not in cache, fetching from database for station: {StationId}", request.StationId);
            
//            var data = await _context.Pumps
//                .Where(x => !request.StationId.HasValue || x.StationId == request.StationId)
//                .ToListAsync();

//            var mappedData = _mapper.Map<IEnumerable<PumpResponse>>(data);
            
//            await _cacheService.SetAsync(cacheKey, mappedData);
            
//            _logger.LogInformation("Cached {Count} pumps for station: {StationId}", mappedData.Count(), request.StationId);

//            return mappedData;
//        }

//        public async Task<ResultDto<PumpResponse>> CreateAsync(PumpRequest dto)
//        {
//            _logger.LogInformation("Creating new pump with number: {Number}, station: {StationId}, manufacturer: {Manufacturer}", 
//                dto.Number, dto.StationId, dto.Manufacturer);

//            try
//            {
//                var pump = new Pump(
//                    dto.Number,
//                    dto.StationId,
//                    dto.Manufacturer
//                );

//                await _context.Pumps.AddAsync(pump);
//                await _context.SaveChangesAsync();

//                // Invalidate cache after creating pump
//                await InvalidatePumpsCacheAsync();
                
//                _logger.LogInformation("Successfully created pump with ID: {Id} for station: {StationId}", 
//                    pump.Id, dto.StationId);

//                return Results.Success(_mapper.Map<PumpResponse>(pump));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error creating pump with number: {Number}, station: {StationId}", 
//                    dto.Number, dto.StationId);
//                return Results.Failure<PumpResponse>(Resource.EntityNotFound);
//            }
//        }

//        public async Task<ResultDto<PumpResponse>> EditAsync(int id, PumpRequest dto)
//        {
//            _logger.LogInformation("Editing pump with ID: {Id}, number: {Number}, station: {StationId}, manufacturer: {Manufacturer}", 
//                id, dto.Number, dto.StationId, dto.Manufacturer);

//            try
//            {
//                var pump = await _context.Pumps.FindAsync(id);

//                if (pump == null)
//                {
//                    _logger.LogWarning("Pump with ID {Id} not found", id);
//                    return Results.Failure<PumpResponse>(Resource.EntityNotFound);
//                }

//                pump.Number = dto.Number;
//                pump.StationId = dto.StationId;
//                pump.Manufacturer = dto.Manufacturer;

//                _context.Pumps.Update(pump);
//                await _context.SaveChangesAsync();

//                // Invalidate cache after editing pump
//                await InvalidatePumpsCacheAsync();
                
//                _logger.LogInformation("Successfully updated pump with ID: {Id}", id);

//                return Results.Success(_mapper.Map<PumpResponse>(pump));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error editing pump with ID: {Id}", id);
//                return Results.Failure<PumpResponse>(Resource.EntityNotFound);
//            }
//        }

//        public async Task<PumpResponse?> DetailsAsync(int id)
//        {
//            _logger.LogInformation("Getting details for pump with ID: {Id}", id);

//            var cacheKey = $"Pump_Details_{id}";
//            var cachedPump = await _cacheService.GetAsync<PumpResponse>(cacheKey);
            
//            if (cachedPump != null)
//            {
//                _logger.LogInformation("Retrieved pump details from cache for ID: {Id}", id);
//                return cachedPump;
//            }

//            _logger.LogInformation("Pump details not in cache, fetching from database for ID: {Id}", id);

//            var data = await _context.Pumps
//                .Include(x => x.Nozzles)
//                .Include(x => x.Station)
//                .SingleOrDefaultAsync(x => x.Id == id);

//            var mappedData = _mapper.Map<PumpResponse>(data);
            
//            if (mappedData != null)
//            {
//                await _cacheService.SetAsync(cacheKey, mappedData);
//                _logger.LogInformation("Cached pump details for ID: {Id}", id);
//            }

//            return mappedData;
//        }

//        public async Task<ResultDto> DeleteAsync(int id)
//        {
//            _logger.LogInformation("Deleting pump with ID: {Id}", id);

//            try
//            {
//                var pump = await _context.Pumps.FindAsync(id);

//                if (pump == null)
//                {
//                    _logger.LogWarning("Pump with ID {Id} not found for deletion", id);
//                    return Results.Failure(Resource.EntityNotFound);
//                }

//                _context.Pumps.Remove(pump);
//                await _context.SaveChangesAsync();

//                // Invalidate cache after deleting pump
//                await InvalidatePumpsCacheAsync();
                
//                _logger.LogInformation("Successfully deleted pump with ID: {Id}", id);

//                return Results.Success();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error deleting pump with ID: {Id}", id);
//                return Results.Failure(Resource.EntityNotFound);
//            }
//        }

//        private async Task InvalidatePumpsCacheAsync()
//        {
//            await _cacheService.RemoveByPatternAsync("Pumps");
//            _logger.LogInformation("Invalidated all pumps cache entries");
//        }
//    }
//}
