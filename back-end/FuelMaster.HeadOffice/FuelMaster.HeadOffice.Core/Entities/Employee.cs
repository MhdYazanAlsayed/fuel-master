namespace FuelMaster.HeadOffice.Core.Entities
{
    public class Employee : EntityBase<int>
    {
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public int? Age { get; set; }
        public string? IdentificationNumber { get; set; }
        public string CardNumber { get; set; }
        public string? Address { get; set; }
        public FuelMasterUser? User { get; set; }
        public Station? Station { get; set; }
        public int? StationId { get; set; }
        public string? PTSNumber { get; set; }

        public Employee(string fullName, string cardNumber, int? stationId = null, string? phoneNumber = null, string? emailAddress = null, int? age = null, string? identificationNumber = null, string? address = null, string? pTSNumber = null)
        {
            FullName = fullName;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            Age = age;
            IdentificationNumber = identificationNumber;
            CardNumber = cardNumber;
            Address = address;
            StationId = stationId;
            PTSNumber = pTSNumber;
        }
    }
}
