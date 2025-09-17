using FuelMaster.HeadOffice.Core.Contracts.Database.Repositories;
using FuelMaster.HeadOffice.Core.Contracts.Markers;
using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace FuelMaster.HeadOffice.Core.Contracts.Database
{
    public interface IUnitOfWork : IScopedDependency
    {
        public IRepository<Station> Stations { get; }
        public IRepository<City> Cities { get; }
        public IRepository<Delivery> Deliveries { get; }
        public IRepository<Employee> Employees { get; }
        public IRepository<Nozzle> Nozzles { get; }
        public IRepository<Pump> Pumps { get; }
        public IRepository<Tank> Tanks { get; }
        public IRepository<TankTransaction> TankTransactions { get; }
        public IRepository<Transaction> Transactions { get; }
        public IRepository<Zone> Zones { get; }
        public IRepository<ZonePrice> ZonePrices { get; }
        public IRepository<ZonePriceHistory> ZonePriceHistory { get; }
        public IRepository<Permission> Permissions { get; }
        public IRepository<FuelMasterGroup> FuelMasterGroups { get; }

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task SaveChangesAsync();
    }
}
