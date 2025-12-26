using AutoMapper;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.NozzleService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.NozzleService.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.TankService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Employees.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions.Queries.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Zones.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Core.Zones;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Enums;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Interfaces;
using FuelMaster.HeadOffice.Core.Interfaces.Services.Transactions.Dto;
using FuelMaster.HeadOffice.Core.Interfaces.Services.Transactions.Results;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.Extensions.Logging;


namespace FuelMaster.HeadOffice.ApplicationService.Entities;
public class TransactionService : ITransactionQuery
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository, 
    ILogger<TransactionService> logger)
    {
        _transactionRepository = transactionRepository;
    }

    public Task<TransactionResult?> DetailsAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<PaginationDto<TransactionResult>> GetPaginationAsync(int page, TransactionPaginationDto dto)
    {
        throw new NotImplementedException();
    }
}
