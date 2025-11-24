using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Models.Requests.PTS;
using FuelMaster.HeadOffice.Core.Models.Responses.PTS;

namespace FuelMaster.HeadOffice.Core.Interfaces.Services
{
    public interface IPTSController : IScopedDependency
    {
        Task<UpdateStatusResponse> UploadStatusAsync(UpdateStatusRequest request);
        Task<PTSResponse> PumpTransactionAsync(PumpTransactionRequest request);
        Task<PTSResponse> UploadTankDeliveryAsync(UploadTankDeliveryRequest request);
        Task<PTSResponse> UploadTankMeasurementAsync(UploadTankMeasurementRequest request);

    }
}
