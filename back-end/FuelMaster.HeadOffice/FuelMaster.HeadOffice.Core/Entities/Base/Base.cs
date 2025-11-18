using System.ComponentModel.DataAnnotations.Schema;
using FuelMaster.HeadOffice.Core.Contracts.Database;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public abstract class Base : IInformationTable
    {
        public DateTime CreatedAt { get; protected set; }

        public DateTime? UpdatedAt { get; protected set; }
    }
}

