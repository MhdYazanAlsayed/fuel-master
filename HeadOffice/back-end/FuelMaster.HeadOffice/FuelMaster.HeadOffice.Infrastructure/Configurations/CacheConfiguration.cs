namespace FuelMaster.HeadOffice.Infrastructure.Configurations
{
    public class CacheConfiguration
    {
        public int DefaultDurationMinutes { get; set; } = 15;
        public int FuelMasterGroupsDurationMinutes { get; set; } = 20;
        public int PaginationDurationMinutes { get; set; } = 10;
        public int DetailsDurationMinutes { get; set; } = 30;
    }
}
