using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.FuelMasterGroups;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.ApplicationService.Entities
{
    public class FuelMasterGroupService : IFuelMasterGroupService
    {
        private readonly FuelMasterDbContext _context;

        public FuelMasterGroupService(IContextFactory<FuelMasterDbContext> contextFactory)
        {
            _context = contextFactory.CurrentContext;
        }

        public async Task<IEnumerable<FuelMasterGroup>> GetAllAsync()
        {
            return await _context.FuelMasterGroup.ToListAsync();
        }

        public async Task<PaginationDto<FuelMasterGroup>> GetPaginationAsync(int currentPage)
        {
            return await _context.FuelMasterGroup.ToPaginationAsync(currentPage);
        }

        public async Task<ResultDto<FuelMasterGroup>> CreateAsync(FuelMasterGroupDto dto)
        {
            try
            {
                var fuelMasterGroup = new FuelMasterGroup(dto.ArabicName, dto.EnglishName);
                await _context.FuelMasterGroup.AddAsync(fuelMasterGroup);
                await _context.SaveChangesAsync();

                return Results.Success(fuelMasterGroup);
            }
            catch (NullReferenceException)
            {
                return Results.Failure<FuelMasterGroup>(Resource.EntityNotFound);
            }
        }

        public async Task<ResultDto<FuelMasterGroup>> EditAsync(int id, FuelMasterGroupDto dto)
        {
            try
            {
                var fuelMasterGroup = await _context.FuelMasterGroup.FindAsync(id);

                if (fuelMasterGroup == null)
                {
                    return Results.Failure<FuelMasterGroup>(Resource.EntityNotFound);
                }

                fuelMasterGroup.ArabicName = dto.ArabicName;
                fuelMasterGroup.EnglishName = dto.EnglishName;

                _context.FuelMasterGroup.Update(fuelMasterGroup);
                await _context.SaveChangesAsync();

                return Results.Success(fuelMasterGroup);
            }
            catch (NullReferenceException)
            {
                return Results.Failure<FuelMasterGroup>(Resource.EntityNotFound);
            }
        }

        public async Task<ResultDto> DeleteAsync(int id)
        {
            var fuelMasterGroup = await _context.FuelMasterGroup.FindAsync(id);

            if (fuelMasterGroup == null)
            {
                return Results.Failure(Resource.EntityNotFound);
            }

            _context.FuelMasterGroup.Remove(fuelMasterGroup);
            await _context.SaveChangesAsync();

            return Results.Success();
        }

        public async Task<FuelMasterGroup?> DetailsAsync(int id)
        {
            return await _context.FuelMasterGroup.SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
