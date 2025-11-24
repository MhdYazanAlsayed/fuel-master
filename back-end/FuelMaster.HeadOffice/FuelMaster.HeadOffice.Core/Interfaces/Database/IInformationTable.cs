namespace FuelMaster.HeadOffice.Core.Interfaces.Database
{
    public interface IInformationTable
    {
        DateTime CreatedAt { get; }
        DateTime? UpdatedAt { get; }
    }
}
