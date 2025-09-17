using FuelMaster.HeadOffice.Core.Constants;

namespace FuelMaster.HeadOffice.Core.Models.Responses.PTS
{
    public class PacketUpdateStatus
    {
        public int Id { get; set; }
        public string Type { get; set; } = "UploadStatus";
        public string Message { get; set; } = "OK";
    }

    public class UpdateStatusResponse
    {
        public string Protocol { get; set; } = PTSConsts.Protocol;
        public List<PacketUpdateStatus> Packets { get; set; } = new List<PacketUpdateStatus>();
    }

}
