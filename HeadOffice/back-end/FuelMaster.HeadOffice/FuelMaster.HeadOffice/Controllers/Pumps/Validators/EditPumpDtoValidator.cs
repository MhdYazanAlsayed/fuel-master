using FluentValidation;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.PumpService.DTOs;

namespace FuelMaster.HeadOffice.Controllers.Pumps.Validators
{
    public class EditPumpDtoValidator : AbstractValidator<PumpDto>
    {
        public EditPumpDtoValidator()
        {
            RuleFor(x => x.Number)
                .GreaterThan(0).WithMessage("Number must be greater than 0");

            RuleFor(x => x.StationId)
                .GreaterThan(0).WithMessage("Station ID must be greater than 0");
        }
    }
}

