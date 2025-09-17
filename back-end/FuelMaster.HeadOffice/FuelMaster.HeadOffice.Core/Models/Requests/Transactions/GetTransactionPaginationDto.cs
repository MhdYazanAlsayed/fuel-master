using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FuelMaster.HeadOffice.Core.Models.Requests.Transactions
{
    public class GetTransactionPaginationDto
    {
        [BindRequired]
        public int Page { get; set; }
        public int? EmployeeId { get; set; }
        public int? NozzleId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int? StationId { get; set; }
    }
}
