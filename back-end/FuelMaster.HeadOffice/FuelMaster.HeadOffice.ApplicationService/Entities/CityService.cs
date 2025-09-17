using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Cities;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.ApplicationService.Entities
{
    public class CityService : ICityService
    {
        private readonly FuelMasterDbContext _context;

        public CityService(IContextFactory<FuelMasterDbContext> contextFactory)
        {
            _context = contextFactory.CurrentContext;
        }

        public async Task<PaginationDto<City>> GetPaginationAsync(int currentPage)
        {
            return await _context.Cities.ToPaginationAsync(currentPage);
        }

        public async Task<IEnumerable<City>> GetAllAsync()
        {
            return await _context.Cities.ToListAsync();
        }

        public async Task<ResultDto<City>> CreateAsync(CityDto dto)
        {
            var city = new City(dto.ArabicName, dto.EnglishName);
            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();

            return Results.Success(city);
        }

        public async Task<ResultDto<City>> EditAsync(int id, CityDto dto)
        {
            var city = await _context.Cities.FindAsync(id);

            if (city is null)
                return Results.Failure(Resource.EntityNotFound, city);

            city.ArabicName = dto.ArabicName;
            city.EnglishName = dto.EnglishName;

            _context.Cities.Update(city);
            await _context.SaveChangesAsync();

            return Results.Success(city);
        }

        public async Task<ResultDto> DeleteAsync(int id)
        {
            var city = await _context.Cities.FindAsync(id);

            if (city is null)
                return Results.Failure(Resource.EntityNotFound);

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return Results.Success();
        }

        public async Task<City?> DetailsAsync(int id)
        {
            return await _context.Cities.SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
