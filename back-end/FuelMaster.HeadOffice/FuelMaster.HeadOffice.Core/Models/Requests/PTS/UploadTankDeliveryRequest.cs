namespace FuelMaster.HeadOffice.Core.Models.Requests.PTS
{
    public class UploadTankDeliveryRequest
    {
        public string? Protocol { get; set; }
        public string? PtsId { get; set; }
        public List<UploadTankDeliveryPacket> Packets { get; set; } = null!;
    }

    public class AbsoluteValues
    {
        public double ProductDensity { get; set; }
        public double ProductMass { get; set; }
        public double PumpsDispensedVolume { get; set; }
    }

    public class UploadTankDeliveryData
    {
        public int Tank { get; set; }
        public StartValues? StartValues { get; set; }
        public EndValues? EndValues { get; set; }
        public AbsoluteValues? AbsoluteValues { get; set; }
        public string? ConfigurationId { get; set; }
    }

    public class EndValues
    {
        public DateTime DateTime { get; set; }
        public double ProductHeight { get; set; }
        public double WaterHeight { get; set; }
        public double Temperature { get; set; }
        public int ProductVolume { get; set; }
        public int ProductTCVolume { get; set; }
    }

    public class UploadTankDeliveryPacket
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public UploadTankDeliveryData Data { get; set; } = null!;
    }

    public class StartValues
    {
        public DateTime DateTime { get; set; }
        public double ProductHeight { get; set; }
        public double WaterHeight { get; set; }
        public double Temperature { get; set; }
        public int ProductVolume { get; set; }
        public int ProductTCVolume { get; set; }
    }


}
