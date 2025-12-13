//using FuelMaster.HeadOffice.Core.Enums;

//namespace FuelMaster.HeadOffice.Core.Models.Responses.Reports
//{
//    public class StoredProcedureReportsResponse
//    {
//        public List<EmployeesReportResponse> EmployeesReport { get; set; } = new();
//        public List<PaymentTransactionsReportResponse> PaymentTransactionsReport { get; set; } = new();
//        public List<StationsReportResponse> StationsReport { get; set; } = new();
//        public List<NozzlesReportResponse> NozzlesReport { get; set; } = new();
//    }

//    public class EmployeesReportResponse
//    {
//        public string FullName { get; set; } = string.Empty;
//        public string StationArabicName { get; set; } = string.Empty;
//        public string StationEnglishName { get; set; } = string.Empty;
//        public decimal Volume { get; set; }
//        public decimal Amount { get; set; }
//    }

//    public class PaymentTransactionsReportResponse
//    {
//        public string ArabicName { get; set; } = string.Empty;
//        public string EnglishName { get; set; } = string.Empty;
//        public PaymentMethod PaymentMethod { get; set; }
//        public int TransactionCount { get; set; }
//        public decimal Volume { get; set; }
//        public decimal Amount { get; set; }
//    }

//    public class StationsReportResponse
//    {
//        public string ArabicName { get; set; } = string.Empty;
//        public string EnglishName { get; set; } = string.Empty;
//        public decimal Volume { get; set; }
//        public decimal Amount { get; set; }
//    }

//    public class NozzlesReportResponse
//    {
//        public string StationArabicName { get; set; } = string.Empty;
//        public string StationEnglishName { get; set; } = string.Empty;
//        public int Number { get; set; }
//        public string FuelType { get; set; } = string.Empty;
//        public decimal Volume { get; set; }
//        public decimal Amount { get; set; }
//    }
//}
