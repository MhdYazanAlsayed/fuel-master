namespace FuelMaster.HeadOffice.Core.Constants
{
    public class PTSConsts
    {
        public const string Protocol = "jsonPTS";

        public static class RequestsType
        {
            public const string UploadPumpTransaction = "UploadPumpTransaction";
            public const string UploadTankMeasurement = "UploadTankMeasurement";
            public const string UploadInTankDelivery = "UploadInTankDelivery";
            public const string UploadStatus = "UploadStatus";
        }

        public static class MessagesType
        {
            public static string Ok = "OK";
            public const string JsonPtsErrorNotFound = "JSONPTS_ERROR_NOT_FOUND";
        }
    }
}
