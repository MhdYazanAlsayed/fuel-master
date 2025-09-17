using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.ApplicationService.Authentication
{
    public class UserManagerFactoryService : IUserManagerFactory
    {
        private readonly IContextFactory<FuelMasterDbContext> _contextFactory;
        private readonly ILogger<UserManager<FuelMasterUser>> _logger;

        public UserManagerFactoryService(IContextFactory<FuelMasterDbContext> contextFactory , ILogger<UserManager<FuelMasterUser>> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public UserManager<FuelMasterUser> CreateUserManager(string tenantId)
        {
            var context = _contextFactory.CreateDbContext(tenantId);
            return new UserManager<FuelMasterUser>(
                    new UserStore<FuelMasterUser, IdentityRole, FuelMasterDbContext>(context),
                    null!,
                    new PasswordHasher<FuelMasterUser>(),
                    new IUserValidator<FuelMasterUser>[0],
                    new IPasswordValidator<FuelMasterUser>[0],
                    new UpperInvariantLookupNormalizer(),
                    new IdentityErrorDescriber(),
                    null!,
                    _logger);
        }

        public UserManager<FuelMasterUser> CreateUserManager()
        {
            var context = _contextFactory.CurrentContext;
            return new UserManager<FuelMasterUser>(
                    new UserStore<FuelMasterUser, IdentityRole, FuelMasterDbContext>(context),
                    null!,
                    new PasswordHasher<FuelMasterUser>(),
                    new IUserValidator<FuelMasterUser>[0],
                    new IPasswordValidator<FuelMasterUser>[0],
                    new UpperInvariantLookupNormalizer(),
                    new IdentityErrorDescriber(),
                    null!,
                    _logger);
        }

    }
}
