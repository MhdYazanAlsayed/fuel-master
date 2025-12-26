using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions.Command.DTOs;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Services.Transactions.Results;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions.Commands;

public interface ITransactionCommand : IScopedDependency
{
    Task<ResultDto<TransactionResult>> CreateAsync (CreateTransactionDto dto);
}