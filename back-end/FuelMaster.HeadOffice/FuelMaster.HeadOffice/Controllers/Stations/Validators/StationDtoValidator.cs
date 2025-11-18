using FluentValidation;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Station.Dtos;
using FuelMaster.HeadOffice.Core.Resources;

namespace FuelMaster.HeadOffice.Controllers.Stations.Validators
{
    public class StationDtoValidator : AbstractValidator<StationDto>
    {
        public StationDtoValidator()
        {
            RuleFor(x => x.ArabicName)
                .NotEmpty().WithMessage(Resource.ArabicNameRequired)
                .MaximumLength(100).WithMessage(Resource.ArabicNameMaxLength);

            RuleFor(x => x.EnglishName)
                .NotEmpty().WithMessage(Resource.EnglishNameMaxLength)
                .MaximumLength(100).WithMessage(Resource.EnglishNameMaxLength);

            RuleFor(x => x.CityId)
                .GreaterThan(0).WithMessage("City ID must be greater than 0");

            RuleFor(x => x.ZoneId)
                .GreaterThan(0).WithMessage("Zone ID must be greater than 0");
        }
    }
}

