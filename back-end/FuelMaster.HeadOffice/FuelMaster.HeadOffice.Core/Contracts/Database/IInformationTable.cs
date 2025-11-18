namespace FuelMaster.HeadOffice.Core.Contracts.Database
{
    public interface IInformationTable
    {
        DateTime CreatedAt { get; }
        DateTime? UpdatedAt { get; }
    }
}
