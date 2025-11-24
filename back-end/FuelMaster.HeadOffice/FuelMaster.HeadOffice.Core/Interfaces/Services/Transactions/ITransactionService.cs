using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Services.Transactions.Dto;
using FuelMaster.HeadOffice.Core.Interfaces.Services.Transactions.Results;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Models.Dtos;


namespace FuelMaster.HeadOffice.Core.Interfaces.Entities
{
   public interface ITransactionService : IScopedDependency
   {
       Task<PaginationDto<Transaction>> GetPaginationAsync(GetTransactionPaginationDto dto, bool includePump = false);
       Task<ResultDto<Transaction>> CreateAsync(CreateTransactionDto dto);
       Task<CreateManuallyResponse> CreateManuallyAsync (CreateManuallyTransactionDto dto);
   }
}
