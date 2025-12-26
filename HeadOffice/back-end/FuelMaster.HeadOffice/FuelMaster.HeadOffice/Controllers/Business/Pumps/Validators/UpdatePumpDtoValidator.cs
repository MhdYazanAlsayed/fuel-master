using FluentValidation;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.PumpService.DTOs;

namespace FuelMaster.HeadOffice.Controllers.Pumps.Validators
{
    public class UpdatePumpDtoValidator : AbstractValidator<UpdatePumpDto>
    {
        public UpdatePumpDtoValidator()
        {
            RuleFor(x => x.Number)
                .GreaterThan(0).WithMessage("Number must be greater than 0");
        }
    }
}

