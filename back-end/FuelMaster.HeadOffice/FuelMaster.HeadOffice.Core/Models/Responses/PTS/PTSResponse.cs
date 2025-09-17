using FuelMaster.HeadOffice.Core.Constants;

namespace FuelMaster.HeadOffice.Core.Models.Responses.PTS
{
    public class PTSResponse
    {
        public string Protocol { get; set; } = PTSConsts.Protocol;

        public List<PTSResponsePacket> Packets { get; set; } = new();
    }

    public class PTSResponsePacket
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? Message { get; set; }
    }

    public class PTSErrorResponse : PTSResponsePacket
    {
        public bool Error { get; set; }
        public int Code { get; set; }
    }
}
