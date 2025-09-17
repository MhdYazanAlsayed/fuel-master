using FuelMaster.HeadOffice.Core.Constants;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Contracts.Services;
using FuelMaster.HeadOffice.Core.Enums;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Hubs;
using FuelMaster.HeadOffice.Core.Models.Dtos.Transactions;
using FuelMaster.HeadOffice.Core.Models.Requests.Deliveries;
using FuelMaster.HeadOffice.Core.Models.Requests.PTS;
using FuelMaster.HeadOffice.Core.Models.Responses.PTS;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.ApplicationService.Services
{
    public class PTSControllerService : IPTSController
    {
        private readonly ILogger<PTSControllerService> _logger;
        private readonly FuelMasterDbContext _context;
        private readonly ITransactionService _transactionService;
        private readonly IDeliveryService _deliveryService;
        private readonly IHubContext<RealTimeHub> _hubContext;

        public PTSControllerService(ILogger<PTSControllerService> logger,
            ITransactionService transactionService,
            IDeliveryService deliveryService ,
            IContextFactory<FuelMasterDbContext> contextFactory,
            IHubContext<RealTimeHub> hubContext)
        {
            _logger = logger;
            _context = contextFactory.CurrentContext;
            _transactionService = transactionService;
            _deliveryService = deliveryService;
            _hubContext = hubContext;
        }

        #region Tank Measurement
        public async Task<PTSResponse> UploadTankMeasurementAsync (UploadTankMeasurementRequest request)
        {
            //var response = new PTSResponse();

            //if (request == null || request.Packets == null)
            //{
            //    response.Packets.Add(
            //              new PTSErrorResponse
            //              {
            //                  Id = 123,
            //                  Type = PTSConsts.RequestsType.UploadTankMeasurement,
            //                  Error = true,
            //                  Message = PTSConsts.MessagesType.JsonPtsErrorNotFound
            //              });
            //    return response;
            //}

            //foreach (var packet in request.Packets)
            //{
            //    var tank = await _unitOfWork.Tanks.Table
            //        .SingleOrDefaultAsync(x => x.Id == packet.Data!.Tank);
            //    if (tank is null)
            //    {
            //        response.Packets.Add(new PTSResponsePacket()
            //        {
            //            Id = packet.Id,
            //            Type = PTSConsts.RequestsType.UploadTankMeasurement,
            //            Message = PTSConsts.MessagesType.JsonPtsErrorNotFound
            //        });

            //        continue;
            //    }
            //}

            throw new NotImplementedException();
        }
        #endregion

        #region Tank Delivery
        public async Task<PTSResponse> UploadTankDeliveryAsync (UploadTankDeliveryRequest request)
        {
            _logger.LogInformation($"Start UploadInTankDeliveryAsync");

            var response = new PTSResponse();

            if (request == null || request.Packets == null)
            {
                _logger.LogInformation($"if (model == null || model.Packets == null)");
                response.Packets.Add(new PTSErrorResponse
                {
                    Id = 123,
                    Type = PTSConsts.RequestsType.UploadInTankDelivery,
                    Error = true,
                    Message = PTSConsts.MessagesType.JsonPtsErrorNotFound
                });

                return response;
            }

            foreach (var packet in request.Packets)
            {
                var tank = await _context.Tanks.SingleOrDefaultAsync(x => x.Id == packet.Data.Tank);

                if (tank is null)
                {
                    response.Packets.Add(new PTSResponsePacket
                    {
                        Id = packet.Id,
                        Type = PTSConsts.RequestsType.UploadInTankDelivery,
                        Message = PTSConsts.MessagesType.JsonPtsErrorNotFound
                    });

                    continue;
                }

                await _deliveryService.CreateAsync(new DeliveryDto()
                {
                    InvoiceNumber = "Automatic",
                    Transport = "Automatic",
                    PaidVolume = packet.Data.EndValues!.ProductVolume - packet.Data.StartValues!.ProductVolume,
                    ReceivedVolume = packet.Data.EndValues.ProductVolume ,
                    TankId = tank.Id ,
                });

                response.Packets.Add(new PTSResponsePacket()
                {
                    Id = packet.Id,
                    Type = PTSConsts.RequestsType.UploadInTankDelivery,
                    Message = PTSConsts.MessagesType.Ok
                });
            }

            _logger.LogInformation($"TankDeliveryAsync performed successfully .");


            return response;
        }

        #endregion

        #region Pump Transactiodn

        public async Task<PTSResponse> PumpTransactionAsync(PumpTransactionRequest request)
        {
            _logger.LogInformation($"Start PumpTransactionAsync");

            var response = new PTSResponse();

            var firstPacket = request.Packets?.FirstOrDefault();
            if (firstPacket is null || firstPacket.Type != PTSConsts.RequestsType.UploadPumpTransaction)
            {
                response.Packets.Add(new PTSErrorResponse()
                {
                    Code = 1,
                    Id = 1,
                    Error = true,
                    Message = PTSConsts.MessagesType.JsonPtsErrorNotFound,
                    Type = firstPacket?.Type ?? null!
                });

                _logger.LogError($"Some values are missing .");

                return response;
            }

            foreach (var packet in request.Packets!)
            {
                var nozzle = await _context.Nozzles
                    .SingleOrDefaultAsync(
                        x => x.Number == packet.Data!.Nozzle &&
                        x.TankId == packet.Data!.Tank);

                if (nozzle is null)
                {
                    _logger.LogError($"Nozzle {{{packet.Data!.Nozzle}}} is null .");

                    response.Packets.Add(new PTSResponsePacket()
                    {
                        Id = packet.Id,
                        Type = PTSConsts.RequestsType.UploadPumpTransaction,
                        Message = PTSConsts.MessagesType.JsonPtsErrorNotFound
                    });

                    continue;
                }

                var employee = await _context.Employees.
                    SingleOrDefaultAsync(x => x.PTSNumber == packet.Data!.UserId.ToString());
                if (employee is null)
                {
                    _logger.LogWarning($"Cannot find employee {{{packet.Data!.UserId}}}");

                    response.Packets.Add(new PTSResponsePacket()
                    {
                        Id = packet.Id,
                        Type = PTSConsts.RequestsType.UploadPumpTransaction,
                        Message = PTSConsts.MessagesType.JsonPtsErrorNotFound
                    });

                    continue;
                }

                var todayDate = DateTimeCulture.Today;
                var tomorrowDate = DateTimeCulture.Today.AddDays(1);

                var transactionFormat = packet.Data!.Pump.ToString() + packet.Data.DateTime.ToString("ddMMyyyyHHmmss");

                var isSavedBefore = await _context.Transactions
                    .AnyAsync(
                    x => x.DateTime >= todayDate &&
                    x.DateTime <= tomorrowDate &&
                    x.UId == transactionFormat);

                if (!isSavedBefore)
                {
                    var result = await _transactionService.CreateAsync(new CreateTransactionDto
                    {
                        Amount = (decimal)packet.Data!.Amount,
                        DateTime = packet.Data.DateTime,
                        EmployeeId = employee.Id ,
                        NozzleId = nozzle.Id , 
                        PaymentMethod = PaymentMethod.Undefined , 
                        Price = (decimal)packet.Data!.Price,
                        TankId = nozzle.TankId , 
                        TotalVolume = (decimal)packet.Data!.TotalVolume , 
                        UId = transactionFormat ,
                        Volume = (decimal)packet.Data!.Volume ,
                    });

                    response.Packets.Add(new PTSResponsePacket()
                    {
                        Id = packet.Id,
                        Type = PTSConsts.RequestsType.UploadPumpTransaction,
                        Message = PTSConsts.MessagesType.Ok
                    });
                }
                else
                {
                    _logger.LogError($"Transaction UId : {transactionFormat} was saved before .");

                    response.Packets.Add(new PTSResponsePacket()
                    {
                        Id = packet.Id,
                        Type = PTSConsts.RequestsType.UploadPumpTransaction,
                        Message = PTSConsts.MessagesType.Ok
                    });
                }
            }


            _logger.LogInformation("Transaction saved successfully .");
            return response;
        }

        #endregion

        #region Update Nozzle Status
        public async Task<UpdateStatusResponse> UploadStatusAsync(UpdateStatusRequest request)
        {
            var realTimeResponse = new RealTimeReportResponse();
            _logger.LogInformation("Start Processing 'UpdateStatusAsync' Method");

            var response = new UpdateStatusResponse();
            foreach (var packet in request.Packets)
            {
                response.Packets.Add(new PacketUpdateStatus()
                {
                    Id = Convert.ToInt32(packet.Id)
                });

                if (packet.Data is null) continue;

                // Offline nozzles
                await FetchNozzlesWithUpdateStatusAsync(
                    packet.Data?.Pumps?.OfflineStatus?.Ids, 
                    NozzleStatus.Off ,
                    realTimeResponse);

                // Online nozzles
                await FetchNozzlesWithUpdateStatusAsync(
                    packet.Data?.Pumps?.FillingStatus?.Ids,
                    NozzleStatus.On,
                    realTimeResponse,
                    packet.Data?.Pumps?.FillingStatus?.Volumes,
                    packet.Data?.Pumps?.FillingStatus?.Amounts);

                // Idle nozzles
                await FetchNozzlesWithUpdateStatusAsync(
                    packet.Data?.Pumps?.IdleStatus?.Ids,
                    NozzleStatus.Disabled,
                    realTimeResponse,
                    packet.Data?.Pumps?.FillingStatus?.Volumes,
                    packet.Data?.Pumps?.FillingStatus?.Amounts);
            }


            await _context.SaveChangesAsync();
            _logger.LogInformation("UpdateStatus performed successfully .");

            // Send signalR notify
            await _hubContext.Clients.All.SendAsync("realtime-report", realTimeResponse);

            return response;
        }

        private async Task FetchNozzlesWithUpdateStatusAsync(
            List<int>? ids,
            NozzleStatus status,
            RealTimeReportResponse realTimeResponse,
            List<double>? volumes = null,
            List<double>? amounts = null)
        {
            if (ids is null)
            {
                _logger.LogError("Some nozzle are not found .");
                throw new NullReferenceException();
            }

            foreach (var id in ids)
            {
                var nozzle = await _context.Nozzles.SingleOrDefaultAsync(x => x.Id == id);
                if (nozzle is null)
                {
                    _logger.LogError($"Cannot find nozzle by id : {{{id}}}");
                    throw new NullReferenceException("Nozzle is null .");
                }

                nozzle.Status = status;

                if (volumes is not null && amounts is not null)
                {
                    var index = ids.IndexOf(id);

                    nozzle.Volume = (decimal)volumes[index];
                    nozzle.Amount = (decimal)amounts[index];
                }
                // Add information to realtime response
                realTimeResponse.Nozzles.Add(new()
                {
                    Id = nozzle.Id,
                    Status = nozzle.Status , 
                    Volume = nozzle.Volume,
                    Amount = nozzle.Amount
                });

                _context.Update(nozzle);
            }
        }

        #endregion
    }
}
