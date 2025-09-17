using AutoMapper;
using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Tanks;
using FuelMaster.HeadOffice.Core.Models.Responses.Tanks;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.ApplicationService.Entities
{
    public class TankService : ITankService
    {
        private readonly FuelMasterDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorization _authorization;

        public TankService(IContextFactory<FuelMasterDbContext> contextFactory, IMapper mapper, IAuthorization authorization)
        {
            _context = contextFactory.CurrentContext;
            _mapper = mapper;
            _authorization = authorization;
        }

        public async Task<IEnumerable<TankResponse>> GetAllAsync(GetTankRequest dto)
        {
            dto.StationId = (await _authorization.TryToGetStationIdAsync()) ?? dto.StationId;
            
            var result = await _context.Tanks
                .Where(x => !dto.StationId.HasValue || x.StationId == dto.StationId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TankResponse>>(result);
        }

        public async Task<ResultDto<TankResponse>> CreateAsync(TankRequest dto)
        {
            try
            {
                if (dto.Capacity <= 0 || dto.MaxLimit <= 0 || dto.MinLimit <= 0 || dto.CurrentLevel <= 0 || dto.CurrentVolume <= 0)
                    throw new Exception();

                if (dto.MaxLimit >= dto.Capacity)
                    throw new Exception();

                if (dto.MinLimit >= dto.MaxLimit)
                    throw new Exception();

                if (dto.CurrentVolume > dto.MaxLimit)
                    throw new Exception();
                
                if (dto.CurrentVolume < dto.MinLimit)
                    throw new Exception();

                    
                var tank = new Tank(
                    dto.StationId,
                    dto.FuelType,
                    dto.Number,
                    dto.Capacity,
                    dto.MaxLimit,
                    dto.MinLimit,
                    dto.CurrentLevel,
                    dto.CurrentVolume,
                    dto.HasSensor
                );
                await _context.Tanks.AddAsync(tank);
                await _context.SaveChangesAsync();

                var tankResponse = _mapper.Map<TankResponse>(tank);
                return Results.Success(tankResponse);
            }
            catch (NullReferenceException)
            {
                return Results.Failure<TankResponse>(Resource.EntityNotFound);
            }
        }

        public async Task<ResultDto<TankResponse>> EditAsync(int id, TankRequest dto)
        {
            try
            {
                if (dto.Capacity <= 0 || dto.MaxLimit <= 0 || dto.MinLimit <= 0 || dto.CurrentLevel <= 0 || dto.CurrentVolume <= 0)
                    throw new Exception();

                if (dto.MaxLimit >= dto.Capacity)
                    throw new Exception();

                if (dto.MinLimit >= dto.MaxLimit)
                    throw new Exception();

                if (dto.CurrentVolume > dto.MaxLimit)
                    throw new Exception();
                
                if (dto.CurrentVolume < dto.MinLimit)
                    throw new Exception();

                var tank = await _context.Tanks.FindAsync(id);

                if (tank == null)
                {
                    return Results.Failure<TankResponse>(Resource.EntityNotFound);
                }

                tank.StationId = dto.StationId;
                tank.FuelType = dto.FuelType;
                tank.Number = dto.Number;
                tank.Capacity = dto.Capacity;
                tank.MaxLimit = dto.MaxLimit;
                tank.MinLimit = dto.MinLimit;
                tank.CurrentLevel = dto.CurrentLevel;
                tank.CurrentVolume = dto.CurrentVolume;
                tank.HasSensor = dto.HasSensor;

                _context.Tanks.Update(tank);
                await _context.SaveChangesAsync();

                var tankResponse = _mapper.Map<TankResponse>(tank);
                return Results.Success(tankResponse);
            }
            catch (NullReferenceException)
            {
                return Results.Failure<TankResponse>(Resource.EntityNotFound);
            }
        }

        public async Task<TankResponse?> DetailsAsync(int id)
        {
            var result = await _context.Tanks.Include(x => x.Station).SingleOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<TankResponse>(result);
        }

        public async Task<ResultDto> DeleteAsync(int id)
        {
            var tank = await _context.Tanks.FindAsync(id);

            if (tank == null)
            {
                return Results.Failure(Resource.EntityNotFound);
            }

            _context.Tanks.Remove(tank);
            await _context.SaveChangesAsync();

            return Results.Success();
        }

        public async Task<PaginationDto<TankResponse>> GetPaginationAsync(int currentPage, GetTankRequest dto)
        {
            dto.StationId = (await _authorization.TryToGetStationIdAsync()) ?? dto.StationId;
            
            var result = await _context.Tanks
                .Include(x => x.Station)
                .Where(x => !dto.StationId.HasValue || x.StationId == dto.StationId)
                .ToPaginationAsync(currentPage);

            var mappedData = _mapper.Map<List<TankResponse>>(result.Data);
            return new PaginationDto<TankResponse>(mappedData, result.Pages);
        }
    }
}
