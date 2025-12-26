using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions.Queries.DTOs;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Services.Transactions.Results;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions;

public interface ITransactionQuery : IScopedDependency
{
    Task<List<TransactionResult>> GetAllAsync(TransactionsDto dto);
    Task<PaginationDto<TransactionResult>> GetPaginationAsync(int page, TransactionPaginationDto dto);
    Task<TransactionResult?> DetailsAsync(long id);
}
