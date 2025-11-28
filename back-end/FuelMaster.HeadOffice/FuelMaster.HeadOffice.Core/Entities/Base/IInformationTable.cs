namespace FuelMaster.HeadOffice.Core.Entities
{
    public interface IInformationTable
    {
        DateTime CreatedAt { get; }
        DateTime? UpdatedAt { get; }
    }
}
