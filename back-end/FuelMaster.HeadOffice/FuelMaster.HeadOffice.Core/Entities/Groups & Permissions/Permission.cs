using FuelMaster.HeadOffice.Core.Helpers;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public class Permission(int fuelMasterGroupId , string key , bool value) : Entity<int>
    {
        public int FuelMasterGroupId { get; private set; } = fuelMasterGroupId;
        public FuelMasterGroup? FuelMasterGroup { get; private set; }
        public string Key { get; private set; } = key;
        public bool Value { get; private set; } = value;

        public void Update(bool value)
        {
            Value = value;
            UpdatedAt = DateTimeCulture.Now;
        }
    }
}
