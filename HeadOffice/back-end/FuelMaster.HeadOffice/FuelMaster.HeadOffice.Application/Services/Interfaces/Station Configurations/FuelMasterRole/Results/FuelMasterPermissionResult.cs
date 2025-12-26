namespace FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.FuelMasterRoleService.Results
{
    public class FuelMasterPermissionResult
    {
        public int Id { get; set; }
        public int AreaOfAccessId { get; set; }
        public string EnglishName { get; set; } = null!;
        public string ArabicName { get; set; } = null!;
        public string EnglishDescription { get; set; } = null!;
        public string ArabicDescription { get; set; } = null!;
    }
}
