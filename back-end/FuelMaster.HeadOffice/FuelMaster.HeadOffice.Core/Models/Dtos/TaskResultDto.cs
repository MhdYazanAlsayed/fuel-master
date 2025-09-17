namespace FuelMaster.HeadOffice.Core.Models.Dtos
{
    public class TaskResultDto(bool succeeded ,string? message = null)
    {
        public string? Message { get; set; } = message;
        public bool Succeeded { get; set; } = succeeded;
    }
}
