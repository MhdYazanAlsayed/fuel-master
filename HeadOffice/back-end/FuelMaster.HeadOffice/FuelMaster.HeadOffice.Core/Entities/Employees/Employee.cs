using FuelMaster.HeadOffice.Core.Entities.Configs.Area;
using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions;
using FuelMaster.HeadOffice.Core.Helpers;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public class Employee : Entity<int>
    {
        protected Employee() {}
        public string FullName { get; private set; }
        public string? PhoneNumber { get; private set; }
        public string? EmailAddress { get; private set; }
        public int? Age { get; private set; }
        public string? IdentificationNumber { get; private set; }
        public string CardNumber { get; private set; }
        public string? Address { get; private set; }
        public FuelMasterUser? User { get; private set; }

        public Station? Station { get; private set; }
        public int? StationId { get; private set; }

        public Area? Area { get; private set; }
        public int? AreaId { get; private set; }

        public City? City { get; private set; }
        public int? CityId { get; private set; }

        // This specifies the permissions and how to use the system.
        public FuelMasterRole? Role { get; private set; }
        public int? RoleId { get; private set; }

        // This specifies the scope of the employee, what he'll see in the system.
        public Scope Scope { get; private set; }

        public string? PTSNumber { get; private set; }

        public Employee(string fullName, string cardNumber, Scope scope, int roleId, int? stationId = null, int? areaId = null, int? cityId = null, string? phoneNumber = null, string? emailAddress = null, int? age = null, string? identificationNumber = null, string? address = null, string? pTSNumber = null)
        {
            if (string.IsNullOrEmpty(fullName)) throw new ArgumentException("FullName is required");
            if (string.IsNullOrEmpty(cardNumber)) throw new ArgumentException("CardNumber is required");
            if (age is not null &&age < 18) throw new ArgumentException("Age must be greater than 18");
            if (roleId <= 0) throw new ArgumentException("RoleId is required");
            if (scope == Scope.Area && areaId is null) throw new ArgumentException("AreaId is required");
            if (scope == Scope.City && cityId is null) throw new ArgumentException("CityId is required");
            if (scope == Scope.Station && stationId is null) throw new ArgumentException("StationId is required");
            if (scope == Scope.Self && stationId is not null) throw new ArgumentException("StationId cannot be provided for self scope");

            FullName = fullName;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            Age = age;
            IdentificationNumber = identificationNumber;
            CardNumber = cardNumber;
            Address = address;
            PTSNumber = pTSNumber;
            Scope = scope;
            RoleId = roleId;
            StationId = scope == Scope.Station ? stationId : null;
            AreaId = scope == Scope.Area ? areaId : null;
            CityId = scope == Scope.City ? cityId : null;
        }
    
        public void Update(string fullName, string cardNumber, Scope scope, int roleId, int? stationId = null, int? areaId = null, int? cityId = null, string? phoneNumber = null, string? emailAddress = null, int? age = null, string? identificationNumber = null, string? address = null, string? pTSNumber = null)
        {
            if (string.IsNullOrEmpty(fullName)) throw new ArgumentException("FullName is required");
            if (string.IsNullOrEmpty(cardNumber)) throw new ArgumentException("CardNumber is required");
            if (age is not null &&age < 18) throw new ArgumentException("Age must be greater than 18");
            if (roleId <= 0) throw new ArgumentException("RoleId is required");
            if (scope == Scope.Area && areaId is null) throw new ArgumentException("AreaId is required");
            if (scope == Scope.City && cityId is null) throw new ArgumentException("CityId is required");
            if (scope == Scope.Station && stationId is null) throw new ArgumentException("StationId is required");
            if (scope == Scope.Self && stationId is not null) throw new ArgumentException("StationId cannot be provided for self scope");

            FullName = fullName;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            Age = age;
            IdentificationNumber = identificationNumber;
            CardNumber = cardNumber;
            Address = address;
            PTSNumber = pTSNumber;
            UpdatedAt = DateTimeCulture.Now;
            RoleId = roleId;
            Scope = scope;
            StationId = scope == Scope.Station ? stationId : null;
            AreaId = scope == Scope.Area ? areaId : null;
            CityId = scope == Scope.City ? cityId : null;
        }
    }
}
