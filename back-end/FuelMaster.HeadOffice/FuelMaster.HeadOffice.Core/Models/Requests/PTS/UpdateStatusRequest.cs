using FuelMaster.HeadOffice.Core.Constants;

namespace FuelMaster.HeadOffice.Core.Models.Requests.PTS
{

    public class UpdateStatusRequest
    {
        public string Protocol { get; set; } = PTSConsts.Protocol;
        public string PtsId { get; set; } = null!;
        public List<PacketUpdateStatusRequest> Packets { get; set; } = null!;
    }

    public class UpdateStatusRequestData
    {
        public string? ConfigurationId { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime FirmwareDateTime { get; set; }
        public int PtsStartupSeconds { get; set; }
        public int BatteryVoltage { get; set; }
        public int CpuTemperature { get; set; }
        public bool PowerDownDetected { get; set; }
        public PumpsUpdateStatusRequest? Pumps { get; set; }
        public ProbesUpdateStatusRequest? Probes { get; set; }
        public PriceBoardsUpdateStatusRequest? PriceBoards { get; set; }
        public ReadersUpdateStatusRequest? Readers { get; set; }
        public object? Gps { get; set; }
        public List<FuelGradeUpdateStatusRequest>? FuelGrades { get; set; }
    }

    public class FillingStatusUpdateStatusRequest
    {
        public List<int> Ids { get; set; }
        public List<int> Nozzles { get; set; }
        public List<int> Transactions { get; set; }
        public List<double> Volumes { get; set; }
        public List<double> Amounts { get; set; }
        public List<double> Prices { get; set; }
    }

    public class FuelGradeUpdateStatusRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public double ExpansionCoefficient { get; set; }
    }
    public class IdleStatusUpdateStatusRequest
    {
        public List<int>? Ids { get; set; }
        public List<int>? NozzlesUp { get; set; }
        public List<int>? LastNozzles { get; set; }
        public List<int>? LastTransactions { get; set; }
        public List<double>? LastVolumes { get; set; }
        public List<double>? LastAmounts { get; set; }
        public List<double>? LastPrices { get; set; }
        public List<string>? Requests { get; set; }
    }

    public class OfflineStatusUpdateStatusRequest
    {
        public List<int>? Ids { get; set; }
        public List<string>? Tags { get; set; }
    }

    public class OnlineStatusUpdateStatusRequest
    {
        public List<int>? Ids { get; set; }
        public List<int>? Errors { get; set; }
        public List<object>? CriticalHighProductAlarms { get; set; }
        public List<object>? HighProductAlarms { get; set; }
        public List<object>? LowProductAlarms { get; set; }
        public List<object>? CriticalLowProductAlarms { get; set; }
        public List<object>? HighWaterAlarms { get; set; }
        public List<object>? TankLeakageAlarms { get; set; }
        public List<List<double?>>? Measurements { get; set; }
    }

    public class PacketUpdateStatusRequest
    {
        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public UpdateStatusRequestData Data { get; set; } = null!;
    }

    public class PriceBoardsUpdateStatusRequest
    {
        public OnlineStatusUpdateStatusRequest? OnlineStatus { get; set; } 
        public OfflineStatusUpdateStatusRequest? OfflineStatus { get; set; } 
    }

    public class ProbesUpdateStatusRequest
    {
        public OnlineStatusUpdateStatusRequest? OnlineStatus { get; set; }
        public OfflineStatusUpdateStatusRequest? OfflineStatus { get; set; }
    }

    public class PumpsUpdateStatusRequest
    {
        public IdleStatusUpdateStatusRequest? IdleStatus { get; set; }
        public FillingStatusUpdateStatusRequest? FillingStatus { get; set; }
        public object? EndOfTransactionStatus { get; set; }
        public OfflineStatusUpdateStatusRequest? OfflineStatus { get; set; }
        public List<string>? Users { get; set; }
    }

    public class ReadersUpdateStatusRequest
    {
        public OnlineStatusUpdateStatusRequest? OnlineStatus { get; set; }
        public OfflineStatusUpdateStatusRequest? OfflineStatus { get; set; }
    }



}
