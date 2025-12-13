using FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Implementations.Authentication
{
    public class UserManagerFactoryService : IUserManagerFactory
    {
        private readonly IContextFactory<FuelMasterDbContext> _contextFactory;
        private readonly ILogger<UserManager<FuelMasterUser>> _logger;
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly IPasswordHasher<FuelMasterUser> _passwordHasher;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly IdentityErrorDescriber _errorDescriber;
        private readonly IUserValidator<FuelMasterUser>[] _userValidators;
        private readonly IPasswordValidator<FuelMasterUser>[] _passwordValidators;

        public UserManagerFactoryService(
            IContextFactory<FuelMasterDbContext> contextFactory,
            ILogger<UserManager<FuelMasterUser>> logger,
            IOptions<IdentityOptions> identityOptions,
            IPasswordHasher<FuelMasterUser> passwordHasher,
            ILookupNormalizer lookupNormalizer,
            IdentityErrorDescriber errorDescriber,
            IEnumerable<IUserValidator<FuelMasterUser>> userValidators,
            IEnumerable<IPasswordValidator<FuelMasterUser>> passwordValidators)
        {
            _contextFactory = contextFactory;
            _logger = logger;
            _identityOptions = identityOptions;
            _passwordHasher = passwordHasher;
            _lookupNormalizer = lookupNormalizer;
            _errorDescriber = errorDescriber;
            _userValidators = userValidators.ToArray();
            _passwordValidators = passwordValidators.ToArray();
        }

        public async Task<UserManager<FuelMasterUser>> CreateUserManagerAsync(Guid tenantId)
        {
            var context = await _contextFactory.CreateDbContextForTenantAsync(tenantId);
            return CreateUserManagerByContext(context);
        }

        public UserManager<FuelMasterUser> CreateUserManager()
        {
            var context = _contextFactory.CurrentContext;
            return CreateUserManagerByContext(context);
        }

        public UserManager<FuelMasterUser> CreateUserManagerByContext<TContext>(TContext context) where TContext : DbContext
        {
            return new UserManager<FuelMasterUser>(
                new UserStore<FuelMasterUser, IdentityRole, TContext>(context),
                _identityOptions,
                _passwordHasher,
                _userValidators,
                _passwordValidators,
                _lookupNormalizer,
                _errorDescriber,
                null!, // IOptions<IdentityOptions> - using the injected one above
                _logger);
        }

        private UserManager<FuelMasterUser> CreateUserManagerByContext(FuelMasterDbContext context)
        {
            return new UserManager<FuelMasterUser>(
                new UserStore<FuelMasterUser, IdentityRole, FuelMasterDbContext>(context),
                _identityOptions,
                _passwordHasher,
                _userValidators,
                _passwordValidators,
                _lookupNormalizer,
                _errorDescriber,
                null!, // IOptions<IdentityOptions> - using the injected one above
                _logger);
        }
    }
}
