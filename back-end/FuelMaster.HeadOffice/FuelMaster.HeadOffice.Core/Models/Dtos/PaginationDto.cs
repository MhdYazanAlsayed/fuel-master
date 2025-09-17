namespace FuelMaster.HeadOffice.Core.Models.Dtos
{
    public class PaginationDto<T>
    {
        public PaginationDto(List<T> data , int pages)
        {
            Data = data;
            Pages = pages;
        }
        public List<T> Data { get; set; }
        public int Pages { get; set; }
    }
}
