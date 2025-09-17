namespace FuelMaster.HeadOffice.Core.Entities
{
    public class Permission(int fuelMasterGroupId , string key , bool value) : EntityBase<int>
    {
        public int FuelMasterGroupId { get; set; } = fuelMasterGroupId;
        public FuelMasterGroup? FuelMasterGroup { get; set; }
        public string Key { get; set; } = key;
        public bool Value { get; set; } = value;
    }
}
