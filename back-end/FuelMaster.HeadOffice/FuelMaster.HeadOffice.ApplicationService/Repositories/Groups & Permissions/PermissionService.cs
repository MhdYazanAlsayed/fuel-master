//using FuelMaster.HeadOffice.Core.Contracts.Database;
//using FuelMaster.HeadOffice.Core.Contracts.Entities;
//using FuelMaster.HeadOffice.Core.Contracts.Services;
//using FuelMaster.HeadOffice.Core.Entities;
//using FuelMaster.HeadOffice.Core.Helpers;
//using FuelMaster.HeadOffice.Core.Models.Dtos;
//using FuelMaster.HeadOffice.Core.Models.Requests.Permissions;
//using FuelMaster.HeadOffice.Infrastructure.Contexts;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;

//namespace FuelMaster.HeadOffice.ApplicationService.Entities.Groups___Permissions
//{
//    public class PermissionService : IFuelMasterPermission
//    {
//        private readonly FuelMasterDbContext _context;
//        private readonly ICacheService _cacheService;
//        private readonly ILogger<PermissionService> _logger;

//        public PermissionService(IContextFactory<FuelMasterDbContext> contextFactory, 
//            ICacheService cacheService, 
//            ILogger<PermissionService> logger)
//        {
//            _context = contextFactory.CurrentContext;
//            _cacheService = cacheService;
//            _logger = logger;
//        }

//        public async Task<IEnumerable<Permission>> GetAsync(int groupId)
//        {
//            _logger.LogInformation("Getting permissions for group ID: {GroupId}", groupId);

//            var cacheKey = $"Permissions_Group_{groupId}";
//            var cachedPermissions = await _cacheService.GetAsync<IEnumerable<Permission>>(cacheKey);
            
//            if (cachedPermissions != null)
//            {
//                _logger.LogInformation("Retrieved {Count} permissions from cache for group ID: {GroupId}", 
//                    cachedPermissions.Count(), groupId);
//                return cachedPermissions;
//            }

//            _logger.LogInformation("Permissions not in cache, fetching from database for group ID: {GroupId}", groupId);

//            var permissions = await _context.Permission
//                .Where(x => x.FuelMasterGroupId == groupId)
//                .ToListAsync();

//            // Cache the permissions for 15 minutes (using default duration)
//            await _cacheService.SetAsync(cacheKey, permissions);
            
//            _logger.LogInformation("Cached {Count} permissions for group ID: {GroupId}", 
//                permissions.Count, groupId);

//            return permissions;
//        }

//        public async Task<ResultDto> UpdateAsync(int groupId, UpdatePermissionsRequest request)
//        {
//            _logger.LogInformation("Updating permissions for group ID: {GroupId}", groupId);

//            var permissions = await GetPermissionsAsync(groupId);
//            foreach (var item in request.Permissions)
//            {
//                var permission = permissions.SingleOrDefault(x => x.Key == item.Key);
//                if (permission is null)
//                {
//                    await _context.Permission
//                        .AddAsync(new(groupId, item.Key, item.Value));

//                    continue;
//                }

//                permission.Value = item.Value;
//                _context.Permission.Update(permission);
//            }

//            await _context.SaveChangesAsync();

//            // Invalidate cache after updating permissions
//            await InvalidatePermissionsCacheAsync(groupId);
            
//            _logger.LogInformation("Successfully updated permissions for group ID: {GroupId}", groupId);

//            return Results.Success();
//        }

//        private async Task<IEnumerable<Permission>> GetPermissionsAsync(int groupId)
//        {
//            var cacheKey = $"Permissions_Group_{groupId}";
//            var cachedPermissions = await _cacheService.GetAsync<IEnumerable<Permission>>(cacheKey);
            
//            if (cachedPermissions != null)
//            {
//                return cachedPermissions;
//            }

//            var permissions = await _context.Permission
//                .Where(x => x.FuelMasterGroupId == groupId)
//                .ToListAsync();

//            // Cache the permissions
//            await _cacheService.SetAsync(cacheKey, permissions);

//            return permissions;
//        }

//        private async Task InvalidatePermissionsCacheAsync(int groupId)
//        {
//            var cacheKey = $"Permissions_Group_{groupId}";
//            await _cacheService.RemoveAsync(cacheKey);
            
//            _logger.LogInformation("Invalidated permissions cache for group ID: {GroupId}", groupId);
//        }
//    }
//}
