using AutoMapper;
using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Pumps;
using FuelMaster.HeadOffice.Core.Models.Responses.Pumps;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.ApplicationService.Entities
{
    public class PumpService : IPumpService
    {
        private readonly FuelMasterDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorization _authorization;
        public PumpService(IContextFactory<FuelMasterDbContext> contextFactory, IMapper mapper, IAuthorization authorization)
        {
            _context = contextFactory.CurrentContext;
            _mapper = mapper;
            _authorization = authorization;
        }

        public async Task<PaginationDto<PumpResponse>> GetPaginationAsync(int page , int? stationId)
        {
            stationId = (await _authorization.TryToGetStationIdAsync()) ?? stationId;
            var paginatedResult = await _context.Pumps
                .Include(x => x.Nozzles)
                .Include(x => x.Station)
                .Where(x => !stationId.HasValue || x.StationId == stationId)
                .ToPaginationAsync(page);

            var mappedData = _mapper.Map<List<PumpResponse>>(paginatedResult.Data);

            return new(mappedData, paginatedResult.Pages);
        }

        public async Task<IEnumerable<PumpResponse>> GetAllAsync(GetPumpRequest request)
        {
            request.StationId = (await _authorization.TryToGetStationIdAsync()) ?? request.StationId;
            var data = await _context.Pumps
                .Where(x => !request.StationId.HasValue || x.StationId == request.StationId)
                .ToListAsync();

            var mappedData = _mapper.Map<IEnumerable<PumpResponse>>(data);

            return mappedData;
        }

        public async Task<ResultDto<PumpResponse>> CreateAsync(PumpRequest dto)
        {
            try
            {
                var pump = new Pump(
                    dto.Number,
                    dto.StationId,
                    dto.Manufacturer
                );

                await _context.Pumps.AddAsync(pump);
                await _context.SaveChangesAsync();

                return Results.Success(_mapper.Map<PumpResponse>(pump));
            }
            catch (NullReferenceException)
            {
                return Results.Failure<PumpResponse>(Resource.EntityNotFound);
            }
        }

        public async Task<ResultDto<PumpResponse>> EditAsync(int id, PumpRequest dto)
        {
            try
            {
                var pump = await _context.Pumps.FindAsync(id);

                if (pump == null)
                {
                    return Results.Failure<PumpResponse>(Resource.EntityNotFound);
                }

                pump.Number = dto.Number;
                pump.StationId = dto.StationId;
                pump.Manufacturer = dto.Manufacturer;

                _context.Pumps.Update(pump);
                await _context.SaveChangesAsync();

                return Results.Success(_mapper.Map<PumpResponse>(pump));
            }
            catch (NullReferenceException)
            {
                return Results.Failure<PumpResponse>(Resource.EntityNotFound);
            }
        }

        public async Task<PumpResponse?> DetailsAsync(int id)
        {
            var data = await _context.Pumps
                .Include(x => x.Nozzles)
                .Include(x => x.Station)
                .SingleOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<PumpResponse>(data);
        }

        public async Task<ResultDto> DeleteAsync(int id)
        {
            var pump = await _context.Pumps.FindAsync(id);

            if (pump == null)
            {
                return Results.Failure(Resource.EntityNotFound);
            }

            _context.Pumps.Remove(pump);
            await _context.SaveChangesAsync();

            return Results.Success();
        }
    }
}
