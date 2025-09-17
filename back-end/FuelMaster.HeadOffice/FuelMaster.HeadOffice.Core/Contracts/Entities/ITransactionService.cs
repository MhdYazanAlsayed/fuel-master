using FuelMaster.HeadOffice.Core.Contracts.Markers;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Dtos.Transactions;
using FuelMaster.HeadOffice.Core.Models.Requests.Transactions;
using FuelMaster.HeadOffice.Core.Models.Responses.Transactions;

namespace FuelMaster.HeadOffice.Core.Contracts.Entities
{
    public interface ITransactionService : IScopedDependency
    {
        Task<PaginationDto<Transaction>> GetPaginationAsync(GetTransactionPaginationDto dto, bool includePump = false);
        Task<ResultDto<Transaction>> CreateAsync(CreateTransactionDto dto);
        Task<CreateManuallyResponse> CreateManuallyAsync (CreateManuallyTransactionDto dto);
    }
}
