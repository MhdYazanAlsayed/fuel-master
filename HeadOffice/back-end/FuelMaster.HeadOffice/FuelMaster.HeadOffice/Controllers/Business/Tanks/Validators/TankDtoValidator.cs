using FluentValidation;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.TankService.DTOs;

namespace FuelMaster.HeadOffice.Controllers.Tanks.Validators
{
    public class TankDtoValidator : AbstractValidator<TankDto>
    {
        public TankDtoValidator()
        {
            RuleFor(x => x.StationId)
                .GreaterThan(0).WithMessage("Station ID must be greater than 0");

            RuleFor(x => x.FuelTypeId)
                .GreaterThan(0).WithMessage("Fuel Type ID must be greater than 0");

            RuleFor(x => x.Number)
                .GreaterThan(0).WithMessage("Number must be greater than 0");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Capacity must be greater than 0");

            RuleFor(x => x.MaxLimit)
                .GreaterThan(0).WithMessage("Max limit must be greater than 0")
                .LessThan(x => x.Capacity).WithMessage("Max limit must be less than capacity");

            RuleFor(x => x.MinLimit)
                .GreaterThan(0).WithMessage("Min limit must be greater than 0")
                .LessThan(x => x.MaxLimit).WithMessage("Min limit must be less than max limit");

            RuleFor(x => x.CurrentLevel)
                .GreaterThanOrEqualTo(0).WithMessage("Current level must be greater than 0 or equal to 0")
                .LessThanOrEqualTo(x => x.MaxLimit).WithMessage("Current level must be less than or equal to max limit");

            RuleFor(x => x.CurrentVolume)
                .GreaterThanOrEqualTo(0).WithMessage("Current volume must be greater than or equal to 0")
                .LessThanOrEqualTo(x => x.MaxLimit).WithMessage("Current volume must be less than or equal to max limit");
        }
    }
}

