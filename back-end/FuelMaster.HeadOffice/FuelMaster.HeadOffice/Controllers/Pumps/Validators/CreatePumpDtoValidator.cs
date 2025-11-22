using FluentValidation;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Pumps.Dtos;
using FuelMaster.HeadOffice.Core.Resources;

namespace FuelMaster.HeadOffice.Controllers.Pumps.Validators
{
    public class CreatePumpDtoValidator : AbstractValidator<CreatePumpDto>
    {
        public CreatePumpDtoValidator()
        {
            RuleFor(x => x.Number)
                .GreaterThan(0).WithMessage("Number must be greater than 0");

            RuleFor(x => x.StationId)
                .GreaterThan(0).WithMessage("Station ID must be greater than 0");
        }
    }
}

