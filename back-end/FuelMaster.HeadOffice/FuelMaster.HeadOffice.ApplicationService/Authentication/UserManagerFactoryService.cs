using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FuelMaster.HeadOffice.ApplicationService.Authentication
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

        public UserManager<FuelMasterUser> CreateUserManager(string tenantId)
        {
            var context = _contextFactory.CreateDbContext(tenantId);
            return CreateUserManagerInternal(context);
        }

        public UserManager<FuelMasterUser> CreateUserManager()
        {
            var context = _contextFactory.CurrentContext;
            return CreateUserManagerInternal(context);
        }

        private UserManager<FuelMasterUser> CreateUserManagerInternal(FuelMasterDbContext context)
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
