namespace FuelMaster.HeadOffice.Application.DTOs;

public class ResultDto<T>(bool succeeded , T? entity = null, string? message = null) where T : class
{
    public bool Succeeded { get; set; } = succeeded;
    public string? Message { get; set; } = message;
    public T? Entity { get; set; } = entity;
}

public class ResultDto(bool succeeded ,string? message = null)
{
    public bool Succeeded { get; set; } = succeeded;
    public string? Message { get; set; } = message;
}