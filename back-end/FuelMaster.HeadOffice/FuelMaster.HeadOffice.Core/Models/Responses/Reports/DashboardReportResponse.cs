using FuelMaster.HeadOffice.Core.Entities;

namespace FuelMaster.HeadOffice.Core.Models.Responses.Reports
{
    public class DashboardReportResponse
    {
        public List<Tank> Tanks { get; set; } = new();
        public List<StationsReportResponse> StationsReport { get; set; } = new();
        public List<NozzlesReportResponse> NozzlesReport { get; set; } = new();
        public List<EmployeesReportResponse> EmployeesReport { get; set; } = new();
        public List<PaymentTransactionsReportResponse> PaymentTransactionsReport { get; set; } = new();
    }
}
