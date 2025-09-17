using FuelMaster.HeadOffice.Core.Configurations;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Database.Repositories;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;

namespace FuelMaster.HeadOffice.Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private IRepository<Station>? _stations;
        private IRepository<City>? _cities;
        private IRepository<Delivery>? _deliveries;
        private IRepository<Employee>? _employees;
        private IRepository<Nozzle>? _nozzles;
        private IRepository<Pump>? _pumps;
        private IRepository<Tank>? _tanks;
        private IRepository<TankTransaction>? _tankTransactions;
        private IRepository<Transaction>? _transactions;
        private IRepository<Zone>? _zones;
        private IRepository<ZonePrice>? _zonePrices;
        private IRepository<ZonePriceHistory>? _histories;
        private IRepository<FuelMasterGroup>? _fuelMasterGroups;
        private IRepository<Permission>? _permissions;

        private FuelMasterDbContext _context;

        public UnitOfWork(
            IContextFactory<FuelMasterDbContext> contextFactory)
        {
            _context = contextFactory.CurrentContext;
        }


        #region Repositories Properties
        public IRepository<Station> Stations => (_stations ?? (_stations = new Repository<Station>(_context)));

        public IRepository<City> Cities => GetRepository(ref _cities);

        public IRepository<Delivery> Deliveries => GetRepository(ref _deliveries);

        public IRepository<Employee> Employees => GetRepository(ref _employees);

        public IRepository<Nozzle> Nozzles => GetRepository(ref _nozzles);

        public IRepository<Pump> Pumps => GetRepository(ref _pumps);
        public IRepository<Tank> Tanks => GetRepository(ref _tanks);

        public IRepository<TankTransaction> TankTransactions => GetRepository(ref _tankTransactions);
        public IRepository<Transaction> Transactions => GetRepository(ref _transactions);

        public IRepository<Zone> Zones => GetRepository(ref _zones);

        public IRepository<ZonePrice> ZonePrices => GetRepository(ref _zonePrices);

        public IRepository<ZonePriceHistory> ZonePriceHistory => GetRepository(ref _histories);

        public IRepository<FuelMasterGroup> FuelMasterGroups => GetRepository(ref _fuelMasterGroups);

        public IRepository<Permission> Permissions => GetRepository(ref _permissions);

        #endregion

        public async Task<IDbContextTransaction> BeginTransactionAsync ()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        private IRepository<T> GetRepository<T>(ref IRepository<T>? repository) where T : Base
        {
            return repository ??= new Repository<T>(_context);
        }

    }
}
