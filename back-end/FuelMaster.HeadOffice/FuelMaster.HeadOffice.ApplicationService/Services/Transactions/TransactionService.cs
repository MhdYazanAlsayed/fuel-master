using FuelMaster.HeadOffice.Core.Interfaces.Database;
using FuelMaster.HeadOffice.Core.Interfaces.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Services.Transactions.Dto;
using FuelMaster.HeadOffice.Core.Interfaces.Services.Transactions.Results;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Enums;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Dtos.Transactions;
using FuelMaster.HeadOffice.Core.Models.Requests.Transactions;
using FuelMaster.HeadOffice.Core.Models.Responses.Transactions;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.ApplicationService.Entities
{
   public class TransactionService : ITransactionService
   {
       private readonly FuelMasterDbContext _context;

       public TransactionService(IContextFactory<FuelMasterDbContext> contextFactory)
       {
           _context = contextFactory.CurrentContext;
       }

    //    public async Task<ResultDto<Transaction>> CreateAsync(CreateTransactionDto dto)
    //    {
    //        var tank = await GetTankAsync(dto.TankId);
    //        if (tank is null)
    //            return Results.Failure<Transaction>();

    //        using var transaction = _context.Database.BeginTransaction();

    //        try 
    //        {
    //            var _transaction = CreateTransactionEntity(dto, tank.StationId);
    //            await _context.Transactions.AddAsync(_transaction);
    //            await _context.SaveChangesAsync();

    //            await UpdateNozzleAsync(dto.NozzleId, dto.Volume, dto.Amount);

    //            await transaction.CommitAsync();
    //            return Results.Success(_transaction);
    //        }
    //        catch 
    //        {
    //            await transaction.RollbackAsync();
    //            return Results.Failure<Transaction>();
    //        }
    //    }

       public async Task<CreateManuallyResponse> CreateManuallyAsync(CreateManuallyTransactionDto dto)
       {
           // Validate nozzle
           var nozzleValidation = await ValidateNozzleAsync(dto);
           if (!nozzleValidation.IsValid)
           {
               return CreateErrorResponse(nozzleValidation.Errors);
           }

           // Validate zone prices
           var zonePriceValidation = ValidateZonePrices(nozzleValidation.Nozzle!);
           if (!zonePriceValidation.IsValid)
           {
               return CreateErrorResponse(zonePriceValidation.Errors);
           }

           // Validate employee
           var employeeValidation = await ValidateEmployeeAsync(dto, nozzleValidation.Nozzle!);
           if (!employeeValidation.IsValid)
           {
               return CreateErrorResponse(employeeValidation.Errors);
           }

           // Validate tank volume
           var tankValidation = ValidateTankVolume(nozzleValidation.Nozzle!, dto.Volume);
           if (!tankValidation.IsValid)
           {
               return CreateErrorResponse(tankValidation.Errors);
           }

           return await ProcessManualTransactionAsync(dto, nozzleValidation.Nozzle!, zonePriceValidation.ZonePrice!, employeeValidation.Employee!);
       }

       public async Task<PaginationDto<Transaction>> GetPaginationAsync(GetTransactionPaginationDto dto, bool includePump = false)
       {
           var from = dto.From ?? DateTimeCulture.Now.AddDays(-1);
           var to = dto.To ?? DateTimeCulture.Now;

           IQueryable<Transaction> query = BuildTransactionQuery(includePump);

           return await query
               .Where(x => !dto.StationId.HasValue || x.StationId == dto.StationId)
               .Where(x => !dto.NozzleId.HasValue || x.NozzleId == dto.NozzleId)
               .Where(x => !dto.EmployeeId.HasValue || x.EmployeeId == dto.EmployeeId)
               .Where(x => x.CreatedAt >= from && x.CreatedAt <= to)
               .ToPaginationAsync(dto.Page);
       }

       #region Private Methods

       private async Task<Tank?> GetTankAsync(int tankId)
       {
           return await _context.Tanks.SingleOrDefaultAsync(x => x.Id == tankId);
       }

       private Transaction CreateTransactionEntity(CreateTransactionDto dto, int stationId)
       {
           return new Transaction(
               dto.UId,
               dto.PaymentMethod,
               dto.Price,
               dto.NozzleId,
               dto.Amount,
               dto.Volume,
               dto.EmployeeId,
               dto.TotalVolume,
               dto.TotalVolume + dto.Volume,
               stationId,
               dto.DateTime
           );
       }

       private async Task UpdateNozzleAsync(int nozzleId, decimal volume, decimal amount)
       {
           var nozzle = await _context.Nozzles.SingleAsync(x => x.Id == nozzleId);
           nozzle.Volume += volume;
           nozzle.Amount += amount;
       }

       private async Task<NozzleValidationResult> ValidateNozzleAsync(CreateManuallyTransactionDto dto)
       {
           var nozzle = await _context.Nozzles
               .Include(x => x.Tank)
               .ThenInclude(x => x!.Station)
               .ThenInclude(x => x!.Zone)
               .ThenInclude(x => x!.Prices)
               .SingleOrDefaultAsync(x => x.Number == dto.NozzleNumber && x.Tank!.StationId == dto.StationId);

           if (nozzle is null)
           {
               return new NozzleValidationResult
               {
                   IsValid = false,
                   Errors = new List<string> { Resource.NozzleNotFound }
               };
           }

           if (nozzle.Status == NozzleStatus.Disabled)
           {
               return new NozzleValidationResult
               {
                   IsValid = false,
                   Errors = new List<string> { Resource.NozzleDisabled }
               };
           }

           return new NozzleValidationResult
           {
               IsValid = true,
               Nozzle = nozzle
           };
       }

       private ZonePriceValidationResult ValidateZonePrices(Nozzle nozzle)
       {
           if (nozzle.Tank?.Station?.Zone?.Prices is null)
           {
               return new ZonePriceValidationResult
               {
                   IsValid = false,
                   Errors = new List<string> { Resource.ZonePricesNotFound }
               };
           }

           var zonePrice = nozzle.Tank.Station.Zone.Prices
               .SingleOrDefault(x => x.FuelType == nozzle.Tank.FuelType);

           if (zonePrice is null)
           {
               return new ZonePriceValidationResult
               {
                   IsValid = false,
                   Errors = new List<string> { Resource.ZonePricesNotFound }
               };
           }

           return new ZonePriceValidationResult
           {
               IsValid = true,
               ZonePrice = zonePrice
           };
       }

       private async Task<EmployeeValidationResult> ValidateEmployeeAsync(CreateManuallyTransactionDto dto, Nozzle nozzle)
       {
           var employee = await _context.Employees
               .Include(x => x.User)
               .SingleOrDefaultAsync(x => x.CardNumber == dto.EmployeeCardNumber);

           if (employee is null || employee.StationId is null || employee.User is null || !employee.User!.IsActive)
           {
               return new EmployeeValidationResult
               {
                   IsValid = false,
                   Errors = new List<string> { Resource.EmployeeNotFound }
               };
           }

           if (employee.StationId != nozzle.Tank!.StationId)
           {
               return new EmployeeValidationResult
               {
                   IsValid = false,
                   Errors = new List<string> { Resource.EmployeeNotInStation }
               };
           }

           return new EmployeeValidationResult
           {
               IsValid = true,
               Employee = employee
           };
       }

       private TankVolumeValidationResult ValidateTankVolume(Nozzle nozzle, decimal volume)
       {
           if (nozzle.Tank!.CurrentVolume - volume < nozzle.Tank.MinLimit)
           {
               return new TankVolumeValidationResult
               {
                   IsValid = false,
                   Errors = new List<string> { Resource.TankHasNoFuelEnough }
               };
           }

           return new TankVolumeValidationResult
           {
               IsValid = true
           };
       }

       private CreateManuallyResponse CreateErrorResponse(List<string> errors)
       {
           return new CreateManuallyResponse
           {
               Succeeded = false,
               Messages = errors
           };
       }

       private async Task<CreateManuallyResponse> ProcessManualTransactionAsync(
           CreateManuallyTransactionDto dto, 
           Nozzle nozzle, 
           ZonePrice zonePrice, 
           Employee employee)
       {
           using var transaction = _context.Database.BeginTransaction();

           try 
           {
               var _transaction = CreateManualTransactionEntity(dto, nozzle, zonePrice, employee);
               await _context.Transactions.AddAsync(_transaction);

               await UpdateNozzleAndTankAsync(nozzle, dto.Volume, zonePrice.Price);

               await _context.SaveChangesAsync();
               await transaction.CommitAsync();

               return new CreateManuallyResponse
               {
                   Succeeded = true,
                   Messages = new List<string>(),
                   Entity = _transaction
               };
           }
           catch 
           {
               await transaction.RollbackAsync();
               return new CreateManuallyResponse
               {
                   Succeeded = false,
                   Messages = new List<string>() { Resource.SthWentWrong }
               };
           }
       }

       private Transaction CreateManualTransactionEntity(
           CreateManuallyTransactionDto dto, 
           Nozzle nozzle, 
           ZonePrice zonePrice, 
           Employee employee)
       {
           return new Transaction(
               Guid.NewGuid().ToString(),
               dto.PaymentMethod,
               zonePrice.Price,
               nozzle.Id,
               zonePrice.Price * dto.Volume,
               dto.Volume,
               employee.Id,
               nozzle.Totalizer,
               nozzle.Totalizer + dto.Volume,
               nozzle.Tank!.StationId,
               DateTimeCulture.Now
           );
       }

       private async Task UpdateNozzleAndTankAsync(Nozzle nozzle, decimal volume, decimal price)
       {
           var today = DateTimeCulture.Now.Date;
           var tomorrow = today.AddDays(1).Date;
           var nozzleHistory = await _context.NozzleHistories
           .Where(x => x.CreatedAt >= today && x.CreatedAt < tomorrow)
           .SingleOrDefaultAsync();

           if (nozzleHistory is null)
           {
               nozzleHistory = new NozzleHistory(nozzle.Id, nozzle.Volume + volume, nozzle.Amount + price * volume, nozzle.Tank!.StationId, nozzle.Tank!.Id);
               await _context.NozzleHistories.AddAsync(nozzleHistory);
           }
           else 
           {
               nozzleHistory.Volume += volume;
               nozzleHistory.Amount += price * volume;
               _context.Update(nozzleHistory);
           }

           nozzle.Volume += volume;
           nozzle.Amount += price * volume;
           nozzle.Totalizer += volume;
           nozzle.Tank!.CurrentVolume -= volume;

           _context.Update(nozzle);
       }

       private IQueryable<Transaction> BuildTransactionQuery(bool includePump)
       {
           IQueryable<Transaction> query = _context.Transactions;

           query = query
               .Include(x => x.Nozzle)
               .ThenInclude(x => x!.Tank)
               .Include(x => x.Employee)
               .Include(x => x.Station);

           if (includePump)
               query = query.Include(x => x.Nozzle!.Pump);

           return query;
       }

        public Task<PaginationDto<Transaction>> GetPaginationAsync(GetTransactionPaginationDto dto, bool includePump = false)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDto<Transaction>> CreateAsync(CreateTransactionDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<CreateManuallyResponse> CreateManuallyAsync(CreateManuallyTransactionDto dto)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Validation Result Classes

        private class NozzleValidationResult
       {
           public bool IsValid { get; set; }
           public List<string> Errors { get; set; } = new();
           public Nozzle? Nozzle { get; set; }
       }

       private class ZonePriceValidationResult
       {
           public bool IsValid { get; set; }
           public List<string> Errors { get; set; } = new();
           public ZonePrice? ZonePrice { get; set; }
       }

       private class EmployeeValidationResult
       {
           public bool IsValid { get; set; }
           public List<string> Errors { get; set; } = new();
           public Employee? Employee { get; set; }
       }

       private class TankVolumeValidationResult
       {
           public bool IsValid { get; set; }
           public List<string> Errors { get; set; } = new();
       }

       #endregion
   }
}
