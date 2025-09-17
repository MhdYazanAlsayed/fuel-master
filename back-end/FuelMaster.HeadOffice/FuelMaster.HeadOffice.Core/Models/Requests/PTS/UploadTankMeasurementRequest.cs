namespace FuelMaster.HeadOffice.Core.Models.Requests.PTS
{
    public class UploadTankMeasurementRequest
    {
        public string Protocol { get; set; } = null!;
        public string PtsId { get; set; } = null!;
        public List<UploadTankMeasurementPacket>? Packets { get; set; }
    }

    public class UploadTankMeasurementData
    {
        public DateTime DateTime { get; set; }
        public int Tank { get; set; }
        public string? Status { get; set; }
        public List<string>? Alarms { get; set; }
        public double ProductHeight { get; set; }
        public double WaterHeight { get; set; }
        public double Temperature { get; set; }
        public int ProductVolume { get; set; }
        public int WaterVolume { get; set; }
        public int ProductUllage { get; set; }
        public int ProductTCVolume { get; set; }
        public double ProductDensity { get; set; }
        public int ProductMass { get; set; }
        public string? ConfigurationId { get; set; }
    }

    public class UploadTankMeasurementPacket
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public UploadTankDeliveryData? Data { get; set; }
    }



}
