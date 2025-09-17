using FuelMaster.HeadOffice.Core.Models.Dtos;

namespace FuelMaster.HeadOffice.Core.Helpers
{
    public static class Results
    {
        public static ResultDto<T> Success<T>(T? entity = null) where T : class
        {
            return new ResultDto<T>(true, entity, null);
        }

        public static ResultDto<T> Failure<T>(string? message = null, T? entity = null) where T : class
        {
            return new ResultDto<T>(false, entity, message);
        }

        public static ResultDto Success()
        {
            return new ResultDto(true, null);
        }

        public static ResultDto Failure(string? message = null)
        {
            return new ResultDto(false, message);
        }
    }
}
