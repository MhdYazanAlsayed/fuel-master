//using FuelMaster.HeadOffice.Core.Interfaces.Authentication;
//using FuelMaster.HeadOffice.Core.Interfaces.Database;
//using FuelMaster.HeadOffice.Core.Interfaces.Entities;
//using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Delivery.Dtos;
//using FuelMaster.HeadOffice.Core.Interfaces.Services;
//using FuelMaster.HeadOffice.Core.Entities;
//using FuelMaster.HeadOffice.Core.Helpers;
//using FuelMaster.HeadOffice.Core.Models.Dtos;
//using FuelMaster.HeadOffice.Core.Resources;
//using FuelMaster.HeadOffice.Infrastructure.Contexts;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;

//namespace FuelMaster.HeadOffice.ApplicationService.Entities
//{
//    public class DeliveryService : IDeliveryRepository
//    {
//        private readonly FuelMasterDbContext _context;
//        private readonly ISigninService _authorization;
//        private readonly ILogger<DeliveryService> _logger;
//        private readonly ICacheService _cacheService;

//        public DeliveryService(IContextFactory<FuelMasterDbContext> contextFactory, 
//            ISigninService authorization, 
//            ILogger<DeliveryService> logger, 
//            ICacheService cacheService)
//        {
//            _context = contextFactory.CurrentContext;
//            _authorization = authorization;
//            _logger = logger;
//            _cacheService = cacheService;
//        }

//        public async Task<ResultDto<Delivery>> CreateAsync(DeliveryDto dto)
//        {
//            _logger.LogInformation("Creating new delivery for tank ID: {TankId}, received volume: {ReceivedVolume}", 
//                dto.TankId, dto.ReceivedVolume);

//            using var transaction = await _context.Database.BeginTransactionAsync();
//            try
//            {
//                var tank = await _context.Tanks
//                    .SingleOrDefaultAsync(x => x.Id == dto.TankId);
//                if (tank is null)
//                {
//                    _logger.LogWarning("Tank with ID {TankId} not found", dto.TankId);
//                    return new(false, null, Resource.CannotFindTank);
//                }

//                if (tank.CurrentVolume + dto.ReceivedVolume > tank.MaxLimit)
//                {
//                    _logger.LogWarning("Tank {TankId} has no space. Current: {CurrentVolume}, Max: {MaxLimit}, Requested: {ReceivedVolume}", 
//                        dto.TankId, tank.CurrentVolume, tank.MaxLimit, dto.ReceivedVolume);
//                    return new(false, null, Resource.TankHasNoSpace);
//                }

//                var delivery = Delivery.Create(
//                    dto.Transport,
//                    dto.InvoiceNumber,
//                    dto.PaidVolume,
//                    dto.ReceivedVolume,
//                    dto.TankId,
//                    tank.CurrentLevel,
//                    tank.CurrentVolume
//                );
//                await _context.Deliveries.AddAsync(delivery);

//                // Update tank level and volume
//                tank.Update(tank.Capacity, tank.MaxLimit, tank.MinLimit, tank.CurrentLevel, tank.CurrentVolume + dto.ReceivedVolume, tank.HasSensor);

//                await _context.SaveChangesAsync();
//                await transaction.CommitAsync();

//                return Results.Success(delivery);
//            }
//            catch (Exception ex)
//            {
//                await transaction.RollbackAsync();
//                _logger.LogError(ex, "Error creating delivery for tank ID: {TankId}", dto.TankId);
//                return new(false, null, Resource.CannotFindTank);
//            }
//        }

//        // public void EditAsync(int id, DeliveryDto dto)
//        // {
//        //     _logger.LogInformation("EditAsync called for delivery ID: {Id} - method not implemented", id);
//        //     throw new NotImplementedException();
//        //     //var delivery = await _context.Deliveries.FindAsync(id);

//        //     //if (delivery != null)
//        //     //{
//        //     //    delivery.Transport = dto.Transport;
//        //     //    delivery.InvoiceNumber = dto.InvoiceNumber;
//        //     //    delivery.PaidVolume = dto.PaidVolume;
//        //     //    delivery.RecivedVolume = dto.RecivedVolume;
//        //     //    delivery.TankOldLevel = dto.TankOldLevel;
//        //     //    delivery.TankNewLevel = dto.TankNewLevel;
//        //     //    delivery.TankOldVolume = dto.TankOldVolume;
//        //     //    delivery.TankNewVolume = dto.TankNewVolume;
//        //     //    delivery.GL = dto.GL;
//        //     //    delivery.TankId = dto.TankId;

//        //     //    _context.Deliveries.Update(delivery);
//        //     //    await _context.SaveChangesAsync();
//        //     //}
//        // }

//        public async Task<Delivery?> DetailsAsync(int id)
//        {
//            _logger.LogInformation("Getting details for delivery with ID: {Id}", id);

//            var delivery = await _context.Deliveries
//                .Include(x => x.Tank)
//                .Include(x => x.Tank!.Station)
//                .SingleOrDefaultAsync(x => x.Id == id);

//            return delivery;
//        }

//        public async Task<ResultDto> DeleteAsync(int id)
//        {
//            _logger.LogInformation("Deleting delivery with ID: {Id}", id);

//            using var transaction = await _context.Database.BeginTransactionAsync();
//            try
//            {
//                var delivery = await _context.Deliveries
//                    .SingleOrDefaultAsync(x => x.Id == id);
//                if (delivery is null)
//                {
//                    _logger.LogWarning("Delivery with ID {Id} not found for deletion", id);
//                    return Results.Failure();
//                }

//                var tank = await _context.Tanks
//                    .SingleOrDefaultAsync(x => x.Id == delivery.TankId);
//                if (tank is null)
//                {
//                    _logger.LogWarning("Tank with ID {TankId} not found for delivery {DeliveryId}", 
//                        delivery.TankId, id);
//                    return Results.Failure();
//                }

//                tank.Update(tank.Capacity, tank.MaxLimit, tank.MinLimit, tank.CurrentLevel, tank.CurrentVolume - delivery.RecivedVolume, tank.HasSensor);
//                _context.Deliveries.Remove(delivery);

//                await _context.SaveChangesAsync();
//                await transaction.CommitAsync();

//                _logger.LogInformation("Successfully deleted delivery with ID: {Id}", id);

//                return Results.Success();
//            }
//            catch (Exception ex)
//            {
//                await transaction.RollbackAsync();
//                _logger.LogError(ex, "Error deleting delivery with ID: {Id}", id);
//                return Results.Failure();
//            }
//        }

//        public async Task<PaginationDto<Delivery>> GetPaginationAsync(GetDeliveriesPaginationDto dto)
//        {
//            _logger.LogInformation("Getting paginated deliveries for page {Page}, station: {StationId}, from: {From}, to: {To}", 
//                dto.Page, dto.StationId, dto.From, dto.To);

//            if (dto.From is null) 
//            {
//                dto.From = DateTime.Now.AddDays(-10);
//            }
            
//            if (dto.To is null)
//            {
//                dto.To = DateTime.Now;
//            }

//            dto.StationId = (await _authorization.TryToGetStationIdAsync()) ?? dto.StationId;

//            // Create cache key based on parameters
//            var cacheKey = $"Deliveries_Page_{dto.Page}_Station_{dto.StationId}_From_{dto.From:yyyyMMdd}_To_{dto.To:yyyyMMdd}";
//            var cachedPagination = await _cacheService.GetAsync<PaginationDto<Delivery>>(cacheKey);
            
//            if (cachedPagination != null)
//            {
//                _logger.LogInformation("Retrieved paginated deliveries from cache for page {Page}", dto.Page);
//                return cachedPagination;
//            }

//            _logger.LogInformation("Paginated deliveries not in cache, fetching from database for page {Page}", dto.Page);

//            var pagination = await _context.Deliveries
//                .Include(x => x.Tank)
//                .Include(x => x.Tank!.Station)
//                .Where(x => !dto.StationId.HasValue ||
//                    (x.Tank != null && x.Tank.StationId == dto.StationId))
//                .Where(x => !dto.From.HasValue || x.CreatedAt >= dto.From)
//                .Where(x => !dto.To.HasValue || x.CreatedAt <= dto.To)
//                .OrderByDescending(x => x.CreatedAt)
//                .ToPaginationAsync(dto.Page);
            
//            await _cacheService.SetAsync(cacheKey, pagination);
            
//            _logger.LogInformation("Cached paginated deliveries for page {Page}", dto.Page);

//            return pagination;
//        }

//    }
//}
