using FluentValidation;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Zones.Dtos;
using FuelMaster.HeadOffice.Core.Resources;

namespace FuelMaster.HeadOffice.Controllers.Zones.Validators
{
    public class ZoneDtoValidator : AbstractValidator<ZoneDto>
    {
        public ZoneDtoValidator()
        {
            RuleFor(x => x.ArabicName)
                .NotEmpty().WithMessage(Resource.ArabicNameRequired)
                .MaximumLength(100).WithMessage(Resource.ArabicNameMaxLength);

            RuleFor(x => x.EnglishName)
                .NotEmpty().WithMessage(Resource.EnglishNameMaxLength)
                .MaximumLength(100).WithMessage(Resource.EnglishNameMaxLength);
        }
    }
}

