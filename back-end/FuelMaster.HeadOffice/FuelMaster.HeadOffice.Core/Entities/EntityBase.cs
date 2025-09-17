using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Helpers;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public abstract class EntityBase<T> : Base
    {
        public EntityBase()
        {
            CreatedAt = DateTimeCulture.Now;
        }

        public T Id { get; set; } = default(T)!;
    }

    public abstract class Base : IInformationTable
    {
        public DateTime CreatedAt { get; set; }
    }
}
