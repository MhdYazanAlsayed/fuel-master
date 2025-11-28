using FuelMaster.HeadOffice.Core.Helpers;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public class Employee : Entity<int>
    {
        public string FullName { get; private set; }
        public string? PhoneNumber { get; private set; }
        public string? EmailAddress { get; private set; }
        public int? Age { get; private set; }
        public string? IdentificationNumber { get; private set; }
        public string CardNumber { get; private set; }
        public string? Address { get; private set; }
        public FuelMasterUser? User { get; private set; }

        // TODO : User may have multiple stations
        public Station? Station { get; private set; }
        public int? StationId { get; private set; }
        public string? PTSNumber { get; private set; }

        public Employee(string fullName, string cardNumber, int? stationId = null, string? phoneNumber = null, string? emailAddress = null, int? age = null, string? identificationNumber = null, string? address = null, string? pTSNumber = null)
        {
            if (string.IsNullOrEmpty(fullName)) throw new ArgumentException("FullName is required");
            if (string.IsNullOrEmpty(cardNumber)) throw new ArgumentException("CardNumber is required");
            if (stationId <= 0) throw new ArgumentException("StationId must be greater than 0");
            if (age is not null &&age < 18) throw new ArgumentException("Age must be greater than 18");

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
    
        public void Update(string fullName, string cardNumber, int? stationId = null, string? phoneNumber = null, string? emailAddress = null, int? age = null, string? identificationNumber = null, string? address = null, string? pTSNumber = null)
        {
            if (string.IsNullOrEmpty(fullName)) throw new ArgumentException("FullName is required");
            if (string.IsNullOrEmpty(cardNumber)) throw new ArgumentException("CardNumber is required");
            if (stationId <= 0) throw new ArgumentException("StationId must be greater than 0");
            if (age is not null &&age < 18) throw new ArgumentException("Age must be greater than 18");

            FullName = fullName;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            Age = age;
            IdentificationNumber = identificationNumber;
            CardNumber = cardNumber;
            Address = address;
            StationId = stationId;
            PTSNumber = pTSNumber;
            UpdatedAt = DateTimeCulture.Now;
        }
    }
}
