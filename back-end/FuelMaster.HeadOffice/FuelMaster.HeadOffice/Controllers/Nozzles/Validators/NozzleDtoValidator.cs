using FluentValidation;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.NozzleService.DTOs;
using FuelMaster.HeadOffice.Core.Enums;

namespace FuelMaster.HeadOffice.Controllers.Nozzles.Validators
{
    public class NozzleDtoValidator : AbstractValidator<NozzleDto>
    {
        public NozzleDtoValidator()
        {
            RuleFor(x => x.TankId)
                .GreaterThan(0).WithMessage("Tank ID must be greater than 0");

            RuleFor(x => x.PumpId)
                .GreaterThan(0).WithMessage("Pump ID must be greater than 0");

            RuleFor(x => x.Number)
                .GreaterThan(0).WithMessage("Number must be greater than 0");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Status must be a valid NozzleStatus value");

            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0).WithMessage("Amount must be greater than or equal to 0");

            RuleFor(x => x.Volume)
                .GreaterThanOrEqualTo(0).WithMessage("Volume must be greater than or equal to 0");

            RuleFor(x => x.Totalizer)
                .GreaterThanOrEqualTo(0).WithMessage("Totalizer must be greater than or equal to 0");
        }
    }
}

