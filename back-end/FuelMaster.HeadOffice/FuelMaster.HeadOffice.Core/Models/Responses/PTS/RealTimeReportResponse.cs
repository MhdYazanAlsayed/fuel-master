using FuelMaster.HeadOffice.Core.Enums;

namespace FuelMaster.HeadOffice.Core.Models.Responses.PTS
{
    public class RealTimeReportResponse
    {
        public List<RealTimeReportNozzleResponse> Nozzles { get; set; } = new List<RealTimeReportNozzleResponse>();
    }

    public class RealTimeReportNozzleResponse
    {
        public int Id { get; set; }
        public NozzleStatus Status { get; set; }
        public decimal Volume { get; set; }
        public decimal Amount { get; set; }
    }
}
