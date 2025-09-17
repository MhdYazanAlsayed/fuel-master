using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Permissions;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.ApplicationService.Entities.Groups___Permissions
{
    public class PermissionService : IFuelMasterPermission
    {
        private readonly FuelMasterDbContext _context;

        public PermissionService(IContextFactory<FuelMasterDbContext> contextFactory)
        {
            _context = contextFactory.CurrentContext;
        }

        public async Task<IEnumerable<Permission>> GetAsync(int groupId)
        {
            return await _context.Permission
                .Where(x => x.FuelMasterGroupId == groupId)
                .ToListAsync();
        }

        public async Task<ResultDto> UpdateAsync(int groupId, UpdatePermissionsRequest request)
        {
            var permissions = await GetPermissionsAsync(groupId);
            foreach (var item in request.Permissions)
            {
                var permission = permissions.SingleOrDefault(x => x.Key == item.Key);
                if (permission is null)
                {
                    await _context.Permission
                        .AddAsync(new(groupId, item.Key, item.Value));

                    continue;
                }

                permission.Value = item.Value;
                _context.Permission.Update(permission);
            }

            await _context.SaveChangesAsync();

            return Results.Success();
        }

        private async Task<IEnumerable<Permission>> GetPermissionsAsync(int groupId)
        {
            return await _context.Permission
                .Where(x => x.FuelMasterGroupId == groupId)
                .ToListAsync();
        }
    }
}
