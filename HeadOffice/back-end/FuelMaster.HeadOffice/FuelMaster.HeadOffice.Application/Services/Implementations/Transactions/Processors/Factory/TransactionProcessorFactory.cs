using AutoMapper;
using FuelMaster.HeadOffice.Application.Services.Implementations.Transactions.ManualProcessor;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Transactions.Enums;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Transactions.Processors;
using FuelMaster.HeadOffice.Core.Interfaces;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.Transactions.Processors.Factory;

public class TransactionProcessorFactory : ITransactionProcessorFactory
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITankService _tankService;
    private readonly INozzleService _nozzleService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<TransactionManualProcessor> _logger;

    public TransactionProcessorFactory(
        ITransactionRepository transactionRepository,
        ITankService tankService,
        INozzleService nozzleService,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<TransactionManualProcessor> logger
    )
    {
        _transactionRepository = transactionRepository;
        _tankService = tankService;
        _nozzleService = nozzleService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public ITransactionProcessor GetProcessor(TransactionType transactionType)
    {
        return transactionType switch
        {
            TransactionType.Manual => new TransactionManualProcessor(_transactionRepository, _tankService, _nozzleService, _unitOfWork, _mapper, _logger),
            TransactionType.PTS => throw new NotImplementedException("PTS processor is not implemented"),
            _ => throw new ArgumentException("Invalid transaction type")
        };
    }
}