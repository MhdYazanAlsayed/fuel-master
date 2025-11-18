using FuelMaster.HeadOffice.Core.Helpers;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public class FuelMasterGroup(string arabicName , string englishName) : AggregateRoot<int>
    {
        public string ArabicName { get; private set; } = arabicName;
        public string EnglishName { get; private set; } = englishName;
        public IReadOnlyList<Permission> Permissions => _permissions.AsReadOnly();
        protected List<Permission> _permissions = new List<Permission>();

        public void AddPermission( string key, bool value)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("Key is required");

            _permissions.Add(new Permission(Id, key, value));
        }

        public void UpdatePermission(int permissionId, string key, bool value)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("Key is required");
            if (permissionId <= 0) throw new ArgumentException("PermissionId must be greater than 0");

            var permission = _permissions.SingleOrDefault(x => x.Id == permissionId);
            if (permission is null) throw new ArgumentException("Permission not found");

            permission.Update(value);
            UpdatedAt = DateTimeCulture.Now;
        }

        public void RemovePermission(int permissionId)
        {
            if (permissionId <= 0) throw new ArgumentException("PermissionId must be greater than 0");

            var permission = _permissions.SingleOrDefault(x => x.Id == permissionId);
            if (permission is null) throw new ArgumentException("Permission not found");

            _permissions.Remove(permission);
        }
    }
}
