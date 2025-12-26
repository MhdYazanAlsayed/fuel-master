using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions.DTOs;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Services.Transactions.Dto;
using FuelMaster.HeadOffice.Core.Interfaces.Services.Transactions.Results;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions;

/// <summary>
/// This interface is used to process transactions in the application.
/// Manual transactions & PTS transactions & Whatever types.
/// It's an abstract interface that will be implemented by the different transaction processors.
/// </summary>
public interface ITransactionProcessor : IScopedDependency
{
    Task<ResultDto<TransactionResult>> ProcessAsync (ProcessTransactionDto dto);
}