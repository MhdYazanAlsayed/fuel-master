using FluentValidation;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.StationService.DTOs;
using FuelMaster.HeadOffice.Core.Resources;

namespace FuelMaster.HeadOffice.Controllers.Stations.Validators
{
    public class EditStationDtoValidator : AbstractValidator<EditStationDto>
    {
        public EditStationDtoValidator()
        {
            RuleFor(x => x.ArabicName)
                .NotEmpty().WithMessage(Resource.ArabicNameRequired)
                .MaximumLength(100).WithMessage(Resource.ArabicNameMaxLength);

            RuleFor(x => x.EnglishName)
                .NotEmpty().WithMessage(Resource.EnglishNameMaxLength)
                .MaximumLength(100).WithMessage(Resource.EnglishNameMaxLength);

            RuleFor(x => x.ZoneId)
                .GreaterThan(0).WithMessage("Zone ID must be greater than 0");
        }
    }
}

