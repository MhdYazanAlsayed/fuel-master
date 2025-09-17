namespace FuelMaster.HeadOffice.Core.Models.Requests.PTS
{
    public class PumpTransactionRequest
    {
        public string? Protocol { get; set; }
        public string? PtsId { get; set; }
        public List<PumpTransactionRequestPacket>? Packets { get; set; }
    }

    public class PumpTransactionRequestData
    {
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTime { get; set; }
        public int Pump { get; set; }
        public int Nozzle { get; set; }
        public int FuelGradeId { get; set; }
        public string? FuelGradeName { get; set; }
        public int Tank { get; set; }
        public int Transaction { get; set; }
        public double Volume { get; set; }
        public double TCVolume { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }
        public double TotalVolume { get; set; }
        public double TotalAmount { get; set; }
        public string? Tag { get; set; }
        public int UserId { get; set; }
        public string? ConfigurationId { get; set; }
    }

    public class PumpTransactionRequestPacket
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public PumpTransactionRequestData? Data { get; set; }
    }

}
