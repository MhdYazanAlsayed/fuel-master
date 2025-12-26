using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Transactions.Enums;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Transactions.Processors;

public interface ITransactionProcessorFactory : IScopedDependency
{
    ITransactionProcessor GetProcessor(TransactionType transactionType);
}