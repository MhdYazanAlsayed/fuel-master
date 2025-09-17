using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FuelMaster.HeadOffice.Core.Models.Requests.Deliveries
{
    public class GetDeliveriesPaginationDto
    {
        [BindRequired]
        public int Page { get; set; }

        public int? StationId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
