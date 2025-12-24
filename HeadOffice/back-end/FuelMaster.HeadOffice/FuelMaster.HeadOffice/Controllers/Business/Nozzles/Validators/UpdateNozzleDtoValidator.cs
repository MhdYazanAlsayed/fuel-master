using FluentValidation;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.NozzleService.DTOs;

namespace FuelMaster.HeadOffice.Controllers.Nozzles.Validators
{
    public class UpdateNozzleDtoValidator : AbstractValidator<UpdateNozzleDto>
    {
        public UpdateNozzleDtoValidator()
        {
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
